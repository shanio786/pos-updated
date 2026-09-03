using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

namespace supershop.PosWeb
{
    /// <summary>
    /// A tiny built-in web server for the new premium POS UI.
    /// It serves the HTML front-end and a small JSON API backed by the SAME
    /// MS SQL database (through DataAccess). Uses a raw TcpListener bound to
    /// 127.0.0.1 so it needs NO admin rights and NO URL reservation - it just
    /// works for the logged-in user. No external server, the shop stays offline.
    ///
    /// Start it with PosServer.Launch(); it opens the UI in the browser.
    /// </summary>
    public static class PosServer
    {
        static TcpListener _listener;
        static volatile bool _running;
        static readonly JavaScriptSerializer _json = new JavaScriptSerializer();
        public static string Url { get; private set; }

        /// <summary>Start the server (idempotent) and open the POS UI in the browser.</summary>
        public static string Launch()
        {
            Start();
            if (!string.IsNullOrEmpty(Url))
            {
                try { System.Diagnostics.Process.Start(Url); }
                catch (Exception ex) { Logger.Error("PosServer open browser", ex); }
            }
            return Url;
        }

        public static void Start()
        {
            if (_running) return;
            int[] ports = { 8787, 8788, 8799, 8901, 9090, 5088 };
            foreach (int port in ports)
            {
                try
                {
                    TcpListener l = new TcpListener(IPAddress.Loopback, port);
                    l.Start();
                    _listener = l;
                    Url = "http://localhost:" + port + "/";
                    break;
                }
                catch { /* port busy - try next */ }
            }
            if (_listener == null)
            {
                Logger.Error("PosServer", new Exception("No free local port for the POS server."));
                return;
            }
            _running = true;
            Thread t = new Thread(Loop) { IsBackground = true, Name = "PosServer" };
            t.Start();
        }

        static void Loop()
        {
            while (_running)
            {
                TcpClient client;
                try { client = _listener.AcceptTcpClient(); }
                catch { break; }
                ThreadPool.QueueUserWorkItem(delegate { Handle(client); });
            }
        }

        static void Handle(TcpClient client)
        {
            try
            {
                using (client)
                using (NetworkStream ns = client.GetStream())
                {
                    ns.ReadTimeout = 8000;
                    // read just the request line (first line) - enough for GET routing
                    string requestLine = ReadLine(ns);
                    if (string.IsNullOrEmpty(requestLine)) return;
                    // drain the rest of the headers
                    string h; int guard = 0;
                    while (!string.IsNullOrEmpty(h = ReadLine(ns)) && guard++ < 100) { }

                    string[] parts = requestLine.Split(' ');
                    string target = parts.Length > 1 ? parts[1] : "/";
                    string path = target, query = "";
                    int qi = target.IndexOf('?');
                    if (qi >= 0) { path = target.Substring(0, qi); query = target.Substring(qi + 1); }
                    path = path.ToLowerInvariant();

                    if (path == "/" || path == "/index.html") { Respond(ns, 200, "text/html; charset=utf-8", LoadPage()); return; }
                    if (path == "/api/products") { RespondJson(ns, Products(Q(query, "q"))); return; }
                    if (path == "/api/lookup") { RespondJson(ns, Lookup(Q(query, "code"))); return; }
                    if (path == "/api/store") { RespondJson(ns, Store()); return; }
                    if (path == "/favicon.ico") { Respond(ns, 200, "image/x-icon", ""); return; }
                    Respond(ns, 404, "text/plain", "Not found");
                }
            }
            catch (Exception ex) { Logger.Error("PosServer handle", ex); }
        }

        static string ReadLine(NetworkStream ns)
        {
            var sb = new StringBuilder();
            int b; int n = 0;
            while ((b = ns.ReadByte()) != -1)
            {
                if (b == '\n') break;
                if (b != '\r') sb.Append((char)b);
                if (++n > 8192) break;
            }
            return sb.ToString();
        }

        static string Q(string query, string key)
        {
            foreach (string pair in query.Split('&'))
            {
                int eq = pair.IndexOf('=');
                string k = eq >= 0 ? pair.Substring(0, eq) : pair;
                if (k == key) return eq >= 0 ? Uri.UnescapeDataString(pair.Substring(eq + 1).Replace('+', ' ')) : "";
            }
            return "";
        }

        // ---- API ----------------------------------------------------------

        static object Products(string q)
        {
            q = (q ?? "").Trim();
            string sql =
                "SELECT TOP 300 product_id AS code, product_name AS name, " +
                " ISNULL(retail_price,0) AS price, wholesale_price AS whole, " +
                " ISNULL(disc_amount,0) AS discAmt, ISNULL(discount,0) AS discPct, " +
                " ISNULL(taxapply,0) AS tax, ISNULL(product_quantity,0) AS qty, " +
                " ISNULL(category,'') AS category " +
                " FROM purchase " +
                " WHERE (@q = '' OR product_name LIKE '%'+@q+'%' OR product_id LIKE '%'+@q+'%') " +
                " ORDER BY product_name";
            return Rows(DataAccess.GetDataTable(sql, DataAccess.P("@q", q)));
        }

        static object Lookup(string code)
        {
            code = (code ?? "").Trim();
            string sql =
                "SELECT TOP 1 product_id AS code, product_name AS name, " +
                " ISNULL(retail_price,0) AS price, wholesale_price AS whole, " +
                " ISNULL(disc_amount,0) AS discAmt, ISNULL(discount,0) AS discPct, " +
                " ISNULL(taxapply,0) AS tax, ISNULL(product_quantity,0) AS qty " +
                " FROM purchase WHERE product_id = @code";
            List<object> rows = Rows(DataAccess.GetDataTable(sql, DataAccess.P("@code", code)));
            return rows.Count > 0 ? rows[0] : null;
        }

        static object Store()
        {
            var d = new Dictionary<string, object> { { "name", "My Shop" }, { "tax", 17.0 } };
            try
            {
                DataTable dt = DataAccess.GetDataTable("select * from storeconfig");
                if (dt.Rows.Count > 0 && dt.Rows[0].ItemArray.Length > 1)
                    d["name"] = dt.Rows[0].ItemArray[1].ToString();
            }
            catch (Exception ex) { Logger.Error("PosServer store", ex); }
            return d;
        }

        // ---- helpers ------------------------------------------------------

        static List<object> Rows(DataTable dt)
        {
            var list = new List<object>();
            foreach (DataRow r in dt.Rows)
            {
                var o = new Dictionary<string, object>();
                foreach (DataColumn c in dt.Columns)
                    o[c.ColumnName] = r[c] == DBNull.Value ? null : r[c];
                list.Add(o);
            }
            return list;
        }

        static string _cachedPage;
        static string LoadPage()
        {
            if (_cachedPage != null) return _cachedPage;
            try
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                using (Stream s = asm.GetManifestResourceStream("supershop.PosWeb.index.html"))
                {
                    if (s != null)
                        using (StreamReader sr = new StreamReader(s, Encoding.UTF8))
                            _cachedPage = sr.ReadToEnd();
                }
            }
            catch (Exception ex) { Logger.Error("PosServer page", ex); }
            if (string.IsNullOrEmpty(_cachedPage))
                _cachedPage = "<h1 style='font-family:sans-serif'>POS UI not found.</h1>";
            return _cachedPage;
        }

        static void RespondJson(NetworkStream ns, object data)
        {
            Respond(ns, 200, "application/json; charset=utf-8", _json.Serialize(data));
        }

        static void Respond(NetworkStream ns, int status, string contentType, string body)
        {
            byte[] buf = Encoding.UTF8.GetBytes(body ?? "");
            string head = "HTTP/1.1 " + status + " " + (status == 200 ? "OK" : status == 404 ? "Not Found" : "Error") + "\r\n"
                + "Content-Type: " + contentType + "\r\n"
                + "Content-Length: " + buf.Length + "\r\n"
                + "Cache-Control: no-store\r\n"
                + "Connection: close\r\n\r\n";
            byte[] headBytes = Encoding.ASCII.GetBytes(head);
            ns.Write(headBytes, 0, headBytes.Length);
            if (buf.Length > 0) ns.Write(buf, 0, buf.Length);
            ns.Flush();
        }
    }
}

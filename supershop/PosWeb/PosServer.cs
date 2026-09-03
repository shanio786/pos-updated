using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

namespace supershop.PosWeb
{
    /// <summary>
    /// A tiny built-in web server for the new premium POS UI.
    /// It serves the HTML front-end and a small JSON API backed by the SAME
    /// MS SQL database (through DataAccess). No external server to install -
    /// it runs inside Adv_POS itself, so the shop stays offline and simple.
    ///
    /// Start it with PosServer.Launch(); it opens the UI in the browser.
    /// (Later this same URL can be hosted in an embedded WebView2 window so it
    /// looks like one desktop app.)
    /// </summary>
    public static class PosServer
    {
        static HttpListener _listener;
        static readonly JavaScriptSerializer _json = new JavaScriptSerializer();
        public static string Url { get; private set; }

        /// <summary>Start the server (idempotent) and open the POS UI in the browser.</summary>
        public static string Launch()
        {
            Start();
            try { System.Diagnostics.Process.Start(Url); }
            catch (Exception ex) { Logger.Error("PosServer open browser", ex); }
            return Url;
        }

        public static void Start()
        {
            if (_listener != null && _listener.IsListening) return;
            // pick the first free localhost port
            int[] ports = { 8787, 8788, 8799, 8901, 9090 };
            foreach (int port in ports)
            {
                try
                {
                    HttpListener l = new HttpListener();
                    string prefix = "http://localhost:" + port + "/";
                    l.Prefixes.Add(prefix);
                    l.Start();
                    _listener = l;
                    Url = prefix;
                    break;
                }
                catch { /* try next port */ }
            }
            if (_listener == null)
            {
                Logger.Error("PosServer", new Exception("No free local port for the POS server."));
                return;
            }
            Thread t = new Thread(Loop) { IsBackground = true, Name = "PosServer" };
            t.Start();
        }

        static void Loop()
        {
            while (_listener != null && _listener.IsListening)
            {
                HttpListenerContext ctx;
                try { ctx = _listener.GetContext(); }
                catch { break; }
                ThreadPool.QueueUserWorkItem(delegate { Handle(ctx); });
            }
        }

        static void Handle(HttpListenerContext ctx)
        {
            try
            {
                string path = ctx.Request.Url.AbsolutePath.ToLowerInvariant();
                if (path == "/" || path == "/index.html") { ServePage(ctx); return; }
                if (path == "/api/products") { Json(ctx, Products(ctx.Request.QueryString["q"])); return; }
                if (path == "/api/lookup") { Json(ctx, Lookup(ctx.Request.QueryString["code"])); return; }
                if (path == "/api/store") { Json(ctx, Store()); return; }
                ctx.Response.StatusCode = 404;
                Write(ctx, "text/plain", "Not found");
            }
            catch (Exception ex)
            {
                Logger.Error("PosServer handle", ex);
                try { ctx.Response.StatusCode = 500; Json(ctx, new Dictionary<string, object> { { "error", ex.Message } }); }
                catch { }
            }
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
            DataTable dt = DataAccess.GetDataTable(sql, DataAccess.P("@q", q));
            return Rows(dt);
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
            DataTable dt = DataAccess.GetDataTable(sql, DataAccess.P("@code", code));
            List<object> rows = Rows(dt);
            return rows.Count > 0 ? rows[0] : null;
        }

        static object Store()
        {
            var d = new Dictionary<string, object> { { "name", "My Shop" }, { "tax", 17.0 } };
            try
            {
                DataTable dt = DataAccess.GetDataTable("select * from storeconfig");
                if (dt.Rows.Count > 0)
                {
                    d["name"] = dt.Rows[0].ItemArray.Length > 1 ? dt.Rows[0].ItemArray[1].ToString() : "My Shop";
                }
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

        static void ServePage(HttpListenerContext ctx)
        {
            string html = LoadPage();
            Write(ctx, "text/html; charset=utf-8", html);
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

        static void Json(HttpListenerContext ctx, object data)
        {
            Write(ctx, "application/json; charset=utf-8", _json.Serialize(data));
        }

        static void Write(HttpListenerContext ctx, string contentType, string body)
        {
            byte[] buf = Encoding.UTF8.GetBytes(body ?? "");
            ctx.Response.ContentType = contentType;
            ctx.Response.ContentLength64 = buf.Length;
            ctx.Response.Headers["Cache-Control"] = "no-store";
            using (Stream os = ctx.Response.OutputStream) os.Write(buf, 0, buf.Length);
        }
    }
}

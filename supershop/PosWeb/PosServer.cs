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

        /// <summary>Start the server (idempotent) and open the POS UI as a clean
        /// standalone app window (Edge/Chrome "app mode" - no tabs, no address bar,
        /// looks like a desktop program).</summary>
        public static string Launch()
        {
            Start();
            if (!string.IsNullOrEmpty(Url))
            {
                try { OpenAppWindow(Url); }
                catch (Exception ex) { Logger.Error("PosServer open window", ex); try { System.Diagnostics.Process.Start(Url); } catch { } }
            }
            return Url;
        }

        /// <summary>Open the URL in a borderless app window using Edge or Chrome app mode.</summary>
        static void OpenAppWindow(string url)
        {
            string pf   = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string pf86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string lad  = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string[] candidates =
            {
                Path.Combine(pf86, @"Microsoft\Edge\Application\msedge.exe"),
                Path.Combine(pf,   @"Microsoft\Edge\Application\msedge.exe"),
                Path.Combine(pf86, @"Google\Chrome\Application\chrome.exe"),
                Path.Combine(pf,   @"Google\Chrome\Application\chrome.exe"),
                Path.Combine(lad,  @"Google\Chrome\Application\chrome.exe"),
            };
            string args = "--app=" + url + " --window-size=1460,920 --disable-features=Translate";
            foreach (string exe in candidates)
            {
                if (File.Exists(exe))
                {
                    System.Diagnostics.Process.Start(exe, args);
                    return;
                }
            }
            System.Diagnostics.Process.Start(url);   // fallback: default browser
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
                    // read the rest of the headers, capturing Content-Length
                    int contentLength = 0;
                    string h; int guard = 0;
                    while (!string.IsNullOrEmpty(h = ReadLine(ns)) && guard++ < 200)
                    {
                        int c = h.IndexOf(':');
                        if (c > 0 && h.Substring(0, c).Trim().ToLowerInvariant() == "content-length")
                            int.TryParse(h.Substring(c + 1).Trim(), out contentLength);
                    }

                    string[] parts = requestLine.Split(' ');
                    string method = parts.Length > 0 ? parts[0].ToUpperInvariant() : "GET";
                    string target = parts.Length > 1 ? parts[1] : "/";
                    string path = target, query = "";
                    int qi = target.IndexOf('?');
                    if (qi >= 0) { path = target.Substring(0, qi); query = target.Substring(qi + 1); }
                    path = path.ToLowerInvariant();

                    if (method == "POST")
                    {
                        string body = ReadBody(ns, contentLength);
                        if (path == "/api/checkout") { RespondJson(ns, Checkout(body)); return; }
                        Respond(ns, 404, "text/plain", "Not found");
                        return;
                    }

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

        // ---- checkout (save the sale) ------------------------------------

        static object Checkout(string body)
        {
            try
            {
                var req = _json.Deserialize<Dictionary<string, object>>(body ?? "{}");
                object linesObj; req.TryGetValue("lines", out linesObj);
                List<object> lines = AsList(linesObj);
                if (lines.Count == 0)
                    return new Dictionary<string, object> { { "ok", false }, { "error", "Cart is empty." } };

                string payType   = S(req, "payType", "Cash");
                double paid      = D(req, "paid");
                double change    = D(req, "change");
                double due       = D(req, "due");
                double discTotal = D(req, "discTotal");
                double taxTotal  = D(req, "taxTotal");
                double ovDiscRate= D(req, "ovDiscRate");
                double taxRate   = D(req, "taxRate");
                string comment   = S(req, "comment", "");
                string saleType  = due > 0 ? "CreditSale" : "CashSale";
                string custId    = S(req, "customerId", "");
                string salesDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                List<object> tenders = AsList(req.ContainsKey("tenders") ? req["tenders"] : null);
                long newId = 0;

                DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
                {
                    // resolve a customer id (fall back to the walk-in customer)
                    if (string.IsNullOrEmpty(custId))
                    {
                        object cid = tx.Scalar("SELECT TOP 1 ID FROM tbl_customer ORDER BY ID");
                        custId = cid == null ? "0" : cid.ToString();
                    }

                    long salesId = tx.NextSalesId();
                    tx.Execute(" insert into sales_payment (sales_id, payment_type, payment_amount, change_amount, due_amount, dis, vat, " +
                               " sales_time, c_id, emp_id, comment, TrxType, Shopid, ovdisrate, vaterate, SaleType) " +
                               " values (@sales_id, @payment_type, @payment_amount, @change_amount, @due_amount, @dis, @vat, " +
                               " @sales_time, @c_id, @emp_id, @comment, 'POS', @Shopid, @ovdisrate, @vaterate, @SaleType)",
                        DataAccess.P("@sales_id", salesId),
                        DataAccess.P("@payment_type", payType),
                        DataAccess.P("@payment_amount", paid),
                        DataAccess.P("@change_amount", change),
                        DataAccess.P("@due_amount", due),
                        DataAccess.P("@dis", discTotal),
                        DataAccess.P("@vat", taxTotal),
                        DataAccess.P("@sales_time", salesDate),
                        DataAccess.P("@c_id", custId),
                        DataAccess.P("@emp_id", UserInfo.UserName),
                        DataAccess.P("@comment", comment),
                        DataAccess.P("@Shopid", UserInfo.Shopid),
                        DataAccess.P("@ovdisrate", ovDiscRate),
                        DataAccess.P("@vaterate", taxRate),
                        DataAccess.P("@SaleType", saleType));

                    foreach (object lo in lines)
                    {
                        var l = lo as Dictionary<string, object>;
                        if (l == null) continue;
                        string code = S(l, "code", "");
                        string name = S(l, "name", "");
                        double qty  = D(l, "qty");
                        double price= D(l, "price");
                        double total= D(l, "total");
                        double dis  = D(l, "discRate");
                        int taxApply= (int)D(l, "tax");

                        double cost = 0, prodDisc = 0;
                        DataTable dt1 = tx.Query("select cost_price, discount from purchase where product_id = @id", DataAccess.P("@id", code));
                        if (dt1.Rows.Count > 0)
                        {
                            cost = ToD(dt1.Rows[0][0]);
                            prodDisc = ToD(dt1.Rows[0][1]);
                        }
                        double profit = Math.Round(((price - (price * prodDisc) / 100.0) - cost) * qty, 2);

                        tx.Execute(" insert into sales_item (sales_id, itemName, Qty, RetailsPrice, Total, profit, sales_time, itemcode, discount, taxapply, status) " +
                                   " values (@sales_id, @itemName, @Qty, @RetailsPrice, @Total, @profit, @sales_time, @itemcode, @discount, @taxapply, @status)",
                            DataAccess.P("@sales_id", salesId),
                            DataAccess.P("@itemName", name),
                            DataAccess.P("@Qty", qty),
                            DataAccess.P("@RetailsPrice", price),
                            DataAccess.P("@Total", total),
                            DataAccess.P("@profit", profit),
                            DataAccess.P("@sales_time", salesDate),
                            DataAccess.P("@itemcode", code),
                            DataAccess.P("@discount", dis),
                            DataAccess.P("@taxapply", taxApply.ToString()),
                            DataAccess.P("@status", 0));

                        tx.Execute("update purchase set product_quantity = product_quantity - @qty where product_id = @id",
                            DataAccess.P("@qty", qty), DataAccess.P("@id", code));
                    }

                    if (tenders.Count > 0)
                    {
                        foreach (object to in tenders)
                        {
                            var t = to as Dictionary<string, object>;
                            if (t == null) continue;
                            tx.Execute("insert into tbl_sale_tender (sales_id, method, amount) values (@id, @m, @a)",
                                DataAccess.P("@id", salesId), DataAccess.P("@m", S(t, "method", "Cash")), DataAccess.P("@a", D(t, "amount")));
                        }
                    }
                    else
                    {
                        tx.Execute("insert into tbl_sale_tender (sales_id, method, amount) values (@id, @m, @a)",
                            DataAccess.P("@id", salesId), DataAccess.P("@m", payType), DataAccess.P("@a", paid));
                    }
                    newId = salesId;
                });

                // print the receipt (fast GDI thermal printer) - never block the sale on it
                try { ReceiptPrinter.Show(newId.ToString()); } catch (Exception ex) { Logger.Error("PosServer receipt", ex); }

                return new Dictionary<string, object> { { "ok", true }, { "salesId", newId } };
            }
            catch (Exception ex)
            {
                Logger.Error("PosServer checkout", ex);
                return new Dictionary<string, object> { { "ok", false }, { "error", ex.Message } };
            }
        }

        static List<object> AsList(object o)
        {
            var list = new List<object>();
            System.Collections.IEnumerable en = o as System.Collections.IEnumerable;
            if (en != null && !(o is string)) foreach (object x in en) list.Add(x);
            return list;
        }

        static string ReadBody(NetworkStream ns, int length)
        {
            if (length <= 0) return "";
            byte[] buf = new byte[length];
            int read = 0;
            while (read < length)
            {
                int n = ns.Read(buf, read, length - read);
                if (n <= 0) break;
                read += n;
            }
            return Encoding.UTF8.GetString(buf, 0, read);
        }

        static string S(Dictionary<string, object> d, string k, string def)
        {
            object v; return (d != null && d.TryGetValue(k, out v) && v != null) ? v.ToString() : def;
        }
        static double D(Dictionary<string, object> d, string k)
        {
            object v; if (d == null || !d.TryGetValue(k, out v) || v == null) return 0;
            try { return Convert.ToDouble(v); } catch { double r; return double.TryParse(v.ToString(), out r) ? r : 0; }
        }
        static double ToD(object o) { try { return o == null || o == DBNull.Value ? 0 : Convert.ToDouble(o); } catch { return 0; } }

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

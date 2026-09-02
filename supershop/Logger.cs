using System;
using System.IO;
using System.Windows.Forms;

namespace supershop
{
    /// <summary>
    /// Very small file logger.  Log files:  %LocalAppData%\Adv_POS\logs\pos-yyyy-MM-dd.log
    /// Nothing the app does depends on logging succeeding.
    /// </summary>
    public static class Logger
    {
        static readonly object _lock = new object();

        public static string LogDirectory
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    Path.Combine("Adv_POS", "logs"));
            }
        }

        public static void Error(Exception ex)
        {
            Error(null, ex);
        }

        public static void Error(string where, Exception ex)
        {
            if (ex == null) return;
            Write("ERROR", (string.IsNullOrEmpty(where) ? "" : where + " : ") + ex.GetType().Name + " - " + ex.Message
                           + Environment.NewLine + ex.StackTrace);
        }

        public static void Info(string message)
        {
            Write("INFO ", message);
        }

        /// <summary>Logs the error and shows a short message to the user.</summary>
        public static void Show(Exception ex, string friendlyMessage)
        {
            Error(ex);
            try
            {
                MessageBox.Show(friendlyMessage + Environment.NewLine + Environment.NewLine + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch { }
        }

        static void Write(string level, string text)
        {
            try
            {
                lock (_lock)
                {
                    Directory.CreateDirectory(LogDirectory);
                    string file = Path.Combine(LogDirectory, "pos-" + DateTime.Now.ToString("yyyy-MM-dd") + ".log");
                    File.AppendAllText(file,
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " [" + level + "] [" + (UserInfo.UserName ?? "-") + "] "
                        + text + Environment.NewLine);
                }
            }
            catch { /* logging must never throw */ }
        }
    }
}

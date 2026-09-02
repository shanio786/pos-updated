using System;
using System.Threading;
using System.Windows.Forms;

namespace supershop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Every unhandled error is written to the log file and shown once,
            // instead of silently closing the application.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += delegate(object sender, ThreadExceptionEventArgs e)
            {
                Logger.Show(e.Exception, "An unexpected error occurred. Details were written to the log file:\n" + Logger.LogDirectory);
            };
            AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs e)
            {
                Logger.Error("Unhandled", e.ExceptionObject as Exception);
            };

            // folders the application writes into (item / user images, expense attachments, invoice PDFs)
            foreach (string folder in new string[] { "IMAGE", "ITEMIMAGE", "FinalImage", "ExpenseAttachment", "InvoicePdf" })
            {
                try { System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Application.StartupPath, folder)); }
                catch (Exception ex) { Logger.Error("create folder " + folder, ex); }
            }

            string dbError;
            if (!DataAccess.TestConnection(out dbError))
            {
                MessageBox.Show(
                    "Cannot connect to the SQL Server database.\n\n" + dbError +
                    "\n\nCheck the connection string in Adv_POS.exe.config (APOSSQLConnectionString).",
                    "Database connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new Login());
        }
    }
}

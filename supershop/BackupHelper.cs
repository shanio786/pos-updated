using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;

namespace supershop
{
    /// <summary>
    /// Automatic database backup.
    ///
    /// * Runs at most once per calendar day (checked from the newest .bak file).
    /// * Writes  APOSDB_yyyy-MM-dd_HHmm.bak  into the configured folder.
    /// * Deletes backups older than the retention period.
    ///
    /// FREE cloud backup: point BackupFolder at a folder that the Google Drive
    /// (or OneDrive / Dropbox) desktop app keeps in sync — every .bak is then
    /// uploaded to the cloud automatically at no cost.  See Database/Backup.
    ///
    /// app.config (Adv_POS.exe.config) &lt;appSettings&gt;:
    ///   AutoBackup            true|false   (default true)
    ///   BackupFolder          path         (default: MyDocuments\Adv_POS_Backups)
    ///   BackupRetentionDays   number       (default 30)
    ///
    /// The .bak file is written by the SQL Server service, so BackupFolder must
    /// be reachable and writable from the machine that runs SQL Server.  Run the
    /// auto-backup on the server terminal (or use the rclone script in
    /// Database/Backup for a headless server).
    /// </summary>
    public static class BackupHelper
    {
        static string Setting(string key, string fallback)
        {
            try
            {
                string v = ConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(v) ? fallback : v;
            }
            catch { return fallback; }
        }

        public static bool AutoBackupEnabled
        {
            get { return Setting("AutoBackup", "true").Trim().ToLowerInvariant() != "false"; }
        }

        public static string BackupFolder
        {
            get
            {
                string def = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Adv_POS_Backups");
                return Setting("BackupFolder", def);
            }
        }

        static int RetentionDays
        {
            get
            {
                int d;
                return int.TryParse(Setting("BackupRetentionDays", "30"), out d) && d > 0 ? d : 30;
            }
        }

        /// <summary>Backs up once a day. Never throws – failures are logged only.</summary>
        public static void EnsureDailyBackup()
        {
            if (!AutoBackupEnabled) return;
            try
            {
                string folder = BackupFolder;
                Directory.CreateDirectory(folder);

                // already backed up today?
                DirectoryInfo di = new DirectoryInfo(folder);
                FileInfo newest = di.GetFiles("APOSDB_*.bak")
                                    .OrderByDescending(f => f.LastWriteTime)
                                    .FirstOrDefault();
                if (newest != null && newest.LastWriteTime.Date == DateTime.Today)
                    return;

                Backup(Path.Combine(folder, "APOSDB_" + DateTime.Now.ToString("yyyy-MM-dd_HHmm") + ".bak"));
                CleanOld(folder);
                Logger.Info("Automatic backup completed.");
            }
            catch (Exception ex)
            {
                Logger.Error("EnsureDailyBackup", ex);
            }
        }

        /// <summary>Backs up the database to an explicit .bak path (used by the manual menu too).</summary>
        public static void Backup(string path)
        {
            DataAccess.ExecuteSQL(
                "BACKUP DATABASE [APOSDB] TO DISK = @path WITH INIT, FORMAT, NAME = 'Adv_POS backup'",
                DataAccess.P("@path", path));
        }

        static void CleanOld(string folder)
        {
            try
            {
                DateTime cutoff = DateTime.Now.AddDays(-RetentionDays);
                foreach (FileInfo f in new DirectoryInfo(folder).GetFiles("APOSDB_*.bak"))
                {
                    if (f.LastWriteTime < cutoff)
                        try { f.Delete(); } catch (Exception ex) { Logger.Error("delete old backup", ex); }
                }
            }
            catch (Exception ex) { Logger.Error("CleanOld", ex); }
        }
    }
}

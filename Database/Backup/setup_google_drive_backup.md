# Free automatic backup to Google Drive

The POS already makes a **daily local backup** by itself (see `AutoBackup` in
`Adv_POS.exe.config`). This adds **off-site copies on Google Drive for free**,
so the shop is safe even if the server PC is stolen or its disk dies.

You have two options. Pick one.

## Option A – the simple one (no scripts)

1. Install **Google Drive for Desktop** on the server PC and sign in
   (15 GB free — enough for years of `.bak` files).
2. It creates a synced folder, e.g. `G:\My Drive`.
3. Make a folder inside it, e.g. `G:\My Drive\POS_Backups`, and give the
   **SQL Server service account** write permission to it
   (right-click → Properties → Security → add `NT SERVICE\MSSQL$SQLEXPRESS`,
   allow Modify).
4. In `Adv_POS.exe.config` set:
   ```xml
   <add key="BackupFolder" value="G:\My Drive\POS_Backups" />
   ```
5. Done. Every day the POS writes the `.bak` there and Google Drive uploads it.

> The same works with **OneDrive** or **Dropbox** — just point `BackupFolder`
> at their synced folder.

## Option B – rclone + Task Scheduler (best for a headless server)

Use this when the server has no desktop Drive app, or you want backups even
when nobody is logged in.

1. Download **rclone** from https://rclone.org/downloads/ and unzip to
   `C:\rclone`. Add `C:\rclone` to the PATH.
2. Connect Google Drive once:
   ```
   rclone config
   ```
   → `n` (new remote) → name it **gdrive** → storage **drive** → accept the
   defaults → it opens a browser to log in to your Google account → `y` to
   confirm. (You can use a free Gmail account.)
3. Test:
   ```
   rclone mkdir gdrive:POS_Backups
   rclone lsd gdrive:
   ```
4. Edit the settings at the top of `backup_to_google_drive.ps1`
   (`$SqlInstance`, `$LocalFolder`, `$RcloneRemote`).
5. Schedule it:
   - Open **Task Scheduler** → Create Task.
   - General: "Run whether user is logged on or not", "Run with highest privileges".
   - Triggers: Daily, e.g. 11:30 PM (after closing).
   - Actions: Program `powershell.exe`, arguments:
     ```
     -ExecutionPolicy Bypass -File "C:\POS\Database\Backup\backup_to_google_drive.ps1"
     ```
6. Right-click the task → **Run** once to confirm it works, then check the
   file appears in Drive under `POS_Backups`.

## Restore a backup

```sql
-- close the app first, then in SSMS on the server:
USE master;
ALTER DATABASE APOSDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE APOSDB FROM DISK = N'C:\POS_Backups\APOSDB_2026-09-02_2330.bak' WITH REPLACE;
ALTER DATABASE APOSDB SET MULTI_USER;
```

## How much does it cost?

Nothing. Google Drive gives 15 GB free; rclone and Drive for Desktop are free.
A shop database backup is usually a few MB to a few hundred MB.

# =====================================================================
#  Adv_POS  -  Backup APOSDB and upload to Google Drive (FREE)
# ---------------------------------------------------------------------
#  Runs on the machine that has SQL Server. Steps:
#    1. BACKUP DATABASE APOSDB  ->  a local .bak
#    2. delete .bak older than $RetentionDays
#    3. upload the folder to Google Drive with rclone
#
#  One-time setup (see setup_google_drive_backup.md):
#    - install rclone  (https://rclone.org/downloads/)
#    - run:  rclone config     (create a remote named "gdrive")
#
#  Schedule it with Windows Task Scheduler (daily, "Run whether user is
#  logged on or not", highest privileges).
# =====================================================================

# ---- settings you can change ----
$SqlInstance   = ".\SQLEXPRESS"                     # your SQL Server instance
$Database      = "APOSDB"
$LocalFolder   = "C:\POS_Backups"                   # local .bak folder
$RcloneRemote  = "gdrive:POS_Backups"              # rclone remote:folder
$RetentionDays = 30
# ---------------------------------

$ErrorActionPreference = "Stop"
$stamp = Get-Date -Format "yyyy-MM-dd_HHmm"
$bak   = Join-Path $LocalFolder "$($Database)_$stamp.bak"

New-Item -ItemType Directory -Force -Path $LocalFolder | Out-Null

Write-Host "Backing up $Database ..."
$sql = "BACKUP DATABASE [$Database] TO DISK = N'$bak' WITH INIT, FORMAT, NAME = 'Adv_POS backup', STATS = 10"
# Integrated security (-E). For a SQL login use:  -U sa -P yourpassword
sqlcmd -S $SqlInstance -E -Q $sql

Write-Host "Removing local backups older than $RetentionDays days ..."
Get-ChildItem $LocalFolder -Filter "$($Database)_*.bak" |
    Where-Object { $_.LastWriteTime -lt (Get-Date).AddDays(-$RetentionDays) } |
    Remove-Item -Force -ErrorAction SilentlyContinue

Write-Host "Uploading to Google Drive ($RcloneRemote) ..."
# --min-age 0 uploads everything; rclone only sends new/changed files.
rclone copy $LocalFolder $RcloneRemote --progress

# keep only the last $RetentionDays days on Drive too
rclone delete $RcloneRemote --min-age "$($RetentionDays)d" --rmdirs

Write-Host "Backup + upload finished: $bak"

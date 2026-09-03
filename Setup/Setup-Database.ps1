<#
  Adv_POS - one-click database setup
  ----------------------------------
  - Finds your local SQL Server (SQLEXPRESS by default)
  - Creates the APOSDB database from Database\APOSDB_MSSQL.sql
  - Points the app (Adv_POS.exe.config) at that database

  Just double-click Setup.bat. No manual SQL needed.

  Optional overrides:
    powershell -File Setup-Database.ps1 -Server ".\SQLEXPRESS"
#>
param(
    [string]$Server   = "",                  # blank = auto-detect
    [string]$Database = "APOSDB",
    [string]$SqlFile  = "",                  # blank = ..\Database\APOSDB_MSSQL.sql
    [string]$ConfigFile = ""                 # blank = auto-find Adv_POS.exe.config
)

$ErrorActionPreference = "Stop"
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$repoRoot  = Split-Path -Parent $scriptDir

function Say($m, $c = "White") { Write-Host $m -ForegroundColor $c }

Say ""
Say "==============================================" Cyan
Say "   Adv_POS  -  Database Setup" Cyan
Say "==============================================" Cyan
Say ""

# ---- 1. locate the SQL script -------------------------------------------
if ([string]::IsNullOrWhiteSpace($SqlFile)) {
    $SqlFile = Join-Path $repoRoot "Database\APOSDB_MSSQL.sql"
}
if (-not (Test-Path $SqlFile)) {
    Say "ERROR: SQL script not found at:`n  $SqlFile" Red
    Say "Run this from inside the project's Setup folder." Yellow
    Read-Host "Press Enter to exit"; exit 1
}
Say "SQL script : $SqlFile" Gray

# ---- 2. find a working SQL Server instance ------------------------------
function Test-SqlServer([string]$srv) {
    $cs = "Server=$srv;Database=master;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=5;"
    try {
        $c = New-Object System.Data.SqlClient.SqlConnection $cs
        $c.Open(); $c.Close(); return $true
    } catch { return $false }
}

$candidates = @()
if (-not [string]::IsNullOrWhiteSpace($Server)) { $candidates += $Server }
$candidates += @(".\SQLEXPRESS", "localhost\SQLEXPRESS", "(local)\SQLEXPRESS",
                 ".", "localhost", "(local)", "$env:COMPUTERNAME\SQLEXPRESS")

$found = $null
Say ""
Say "Looking for your SQL Server..." White
foreach ($srv in ($candidates | Select-Object -Unique)) {
    Write-Host ("  trying {0} ... " -f $srv) -NoNewline
    if (Test-SqlServer $srv) { Say "OK" Green; $found = $srv; break }
    else { Say "no" DarkGray }
}

if (-not $found) {
    Say ""
    Say "ERROR: Could not connect to any SQL Server instance." Red
    Say "Make sure SQL Server Express is installed and running," Yellow
    Say "then run this again. Or pass your server name:" Yellow
    Say '   powershell -File Setup-Database.ps1 -Server "YOURPC\SQLEXPRESS"' Gray
    Read-Host "Press Enter to exit"; exit 1
}
$Server = $found
Say ""
Say "Using SQL Server: $Server" Green

# ---- 3. run the schema script (split on GO batches) ---------------------
Say ""
Say "Creating database '$Database' ..." White
$sqlText = Get-Content -Path $SqlFile -Raw
# normalise line endings so CRLF files split correctly (.NET $ won't span \r)
$sqlText = $sqlText -replace "`r`n", "`n" -replace "`r", "`n"
# split on lines that contain only GO (case-insensitive), tolerating a stray \r
$batches = [System.Text.RegularExpressions.Regex]::Split(
    $sqlText, "(?im)^[\t ]*GO[\t ]*;?[\t ]*\r?$")

$cs = "Server=$Server;Database=master;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=30;"
$conn = New-Object System.Data.SqlClient.SqlConnection $cs
$conn.Open()
try {
    $n = 0
    foreach ($b in $batches) {
        $t = $b.Trim()
        if ($t.Length -eq 0) { continue }
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = $b
        $cmd.CommandTimeout = 120
        [void]$cmd.ExecuteNonQuery()
        $n++
    }
    Say "  executed $n batches - OK" Green
} catch {
    Say ""
    Say "ERROR while running the schema:" Red
    Say ("  " + $_.Exception.Message) Red
    $conn.Close()
    Read-Host "Press Enter to exit"; exit 1
}
$conn.Close()

# ---- 4. sanity check ----------------------------------------------------
$cs2 = "Server=$Server;Database=$Database;Integrated Security=True;TrustServerCertificate=True;"
$c2 = New-Object System.Data.SqlClient.SqlConnection $cs2
$c2.Open()
$cmd2 = $c2.CreateCommand()
$cmd2.CommandText = "SELECT COUNT(*) FROM sys.tables"
$tableCount = [int]$cmd2.ExecuteScalar()
$c2.Close()
Say "  database '$Database' now has $tableCount tables" Green

# ---- 5. point the app at this database ----------------------------------
if ([string]::IsNullOrWhiteSpace($ConfigFile)) {
    $ConfigFile = Get-ChildItem -Path $repoRoot -Recurse -Filter "Adv_POS.exe.config" -ErrorAction SilentlyContinue |
                  Select-Object -First 1 -ExpandProperty FullName
    if (-not $ConfigFile) {
        # fall back to the source app.config so a later build picks it up
        $ConfigFile = Get-ChildItem -Path $repoRoot -Recurse -Filter "app.config" -ErrorAction SilentlyContinue |
                      Where-Object { $_.FullName -match "supershop" } |
                      Select-Object -First 1 -ExpandProperty FullName
    }
}

if ($ConfigFile -and (Test-Path $ConfigFile)) {
    Say ""
    Say "Updating connection string in:" White
    Say "  $ConfigFile" Gray
    try {
        $newCs = "Data Source=$Server;Initial Catalog=$Database;Integrated Security=True;TrustServerCertificate=True;"
        $xml = New-Object System.Xml.XmlDocument
        $xml.PreserveWhitespace = $true
        $xml.Load($ConfigFile)
        $node = $xml.SelectSingleNode("//connectionStrings/add[contains(@name,'APOSSQLConnectionString')]")
        if ($node -ne $null) {
            $node.SetAttribute("connectionString", $newCs)
            $xml.Save($ConfigFile)
            Say "  connection string set - OK" Green
        } else {
            Say "  (could not find the APOSSQLConnectionString entry - set it by hand)" Yellow
        }
    } catch {
        Say ("  could not update config: " + $_.Exception.Message) Yellow
    }
} else {
    Say ""
    Say "Note: app config not found yet (build the app first, then re-run" Yellow
    Say "this, or set Data Source=$Server in Adv_POS.exe.config by hand)." Yellow
}

Say ""
Say "==============================================" Green
Say "   DONE. Database is ready." Green
Say "==============================================" Green
Say ""
Say "Next: build/open Adv_POS.exe and log in as  admin / admin" White
Say ""
Read-Host "Press Enter to close"

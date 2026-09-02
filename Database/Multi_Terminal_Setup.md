# Running Adv_POS on several terminals (shopping mall)

Adv_POS is built for this: **one SQL Server database, many POS terminals.**
All counters share the same stock, sales, customers and reports in real time.

```
        ┌─────────────────────────────┐
        │      SERVER  PC              │
        │   SQL Server (APOSDB)        │   <-- the ONLY place data lives
        └──────────────┬──────────────┘
                       │  LAN (wired is best)
      ┌────────────────┼────────────────┐
      │                │                │
 ┌────▼────┐      ┌────▼────┐      ┌────▼────┐
 │ POS 1   │      │ POS 2   │      │ POS 3   │   <-- Adv_POS.exe on each
 │ (Shopid)│      │ (Shopid)│      │ (Shopid)│
 └─────────┘      └─────────┘      └─────────┘
```

## 1. Prepare the server PC

1. Install **SQL Server** (Express is free and enough for a shop).
   During setup enable **Mixed Mode** and set a password for `sa`.
2. Run `Database/APOSDB_MSSQL.sql` in SSMS to create `APOSDB`.
3. Create one SQL login for the terminals (safer than using `sa`):
   ```sql
   CREATE LOGIN posuser WITH PASSWORD = 'Strong#Pass123';
   USE APOSDB;
   CREATE USER posuser FOR LOGIN posuser;
   ALTER ROLE db_owner ADD MEMBER posuser;
   ```
4. Let the terminals reach SQL Server over the network:
   - **SQL Server Configuration Manager** → SQL Server Network Configuration →
     Protocols → enable **TCP/IP** → restart the SQL Server service.
   - Start the **SQL Server Browser** service (set it to Automatic).
   - **Windows Firewall** on the server: allow TCP **1433** (and UDP **1434**
     for the Browser) inbound.
5. Give the server PC a **fixed IP** (e.g. 192.168.1.10) so it never changes.

## 2. Set up each POS terminal

1. Copy the built application folder (the `Adv_POS.exe` + DLLs) to the terminal,
   or install with the `Setup` project.
2. Edit `Adv_POS.exe.config` next to the exe – point it at the server:
   ```xml
   <add name="supershop.Properties.Settings.APOSSQLConnectionString"
        connectionString="Data Source=192.168.1.10,1433;Initial Catalog=APOSDB;User ID=posuser;Password=Strong#Pass123;"
        providerName="System.Data.SqlClient" />
   ```
   (Use the server's fixed IP. `\SQLEXPRESS` after the IP only if it is a named
   instance and you did not open port 1433 directly.)
3. Start Adv_POS. If it opens the login screen, the connection works. If not,
   the message tells you what is wrong (firewall, wrong IP, TCP/IP disabled).

## 3. One shop vs. several branches (Shopid)

Every sale, return, purchase and terminal row carries a **Shopid**.

* **Several counters in one shop** – give them all the **same Shopid**. They
  share one stock and the reports add up for the whole shop.
* **Several branches / shops in a mall** – give each branch its **own Shopid**
  (Config → Terminal). Each user is tied to a Shopid (set in
  Users → Manage Users), and reports can be filtered per branch in the
  Sales Report screen (terminal filter).

Because the invoice number is now allocated inside a locking transaction, two
terminals can complete a sale at the same instant without clashing.

## 4. Backups

The server holds all the data, so **back up the server only**. The POS already
takes a daily local backup; add off-site copies with
`Database/Backup/setup_google_drive_backup.md` (free, Google Drive).

## 5. Good practice

* Wired LAN for the terminals; Wi-Fi drops cause "cannot connect" errors.
* Keep the server PC on a UPS (battery) so a power cut can't corrupt the DB.
* Don't run heavy other software on the server PC.
* Test a **restore** once, not just a backup — a backup you never restored is
  not a backup.

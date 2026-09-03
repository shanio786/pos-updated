# Adv POS — install on your Windows PC (step by step)

This is for a **brand-new install** on your own machine (no old data to import).
Follow it top to bottom once; it takes about 20–30 minutes the first time.

---

## 0. What you need (download once, all free)

| # | Software | Why |
|---|----------|-----|
| 1 | **Visual Studio 2022 Community** (free) — pick the *.NET desktop development* workload during setup | to build the app |
| 2 | **.NET Framework 4.8 Developer Pack** (usually already included with VS) | the app targets .NET 4.8 |
| 3 | **SQL Server 2019/2022 Express** (free) | the database |
| 4 | **SQL Server Management Studio (SSMS)** (free) | to create/manage the database |

> A normal shop counter PC only needs **SQL Server Express** + the built **app**.
> You need **Visual Studio** only on the machine where you *build* the app once.

---

## 1. Get the code

You already have this branch. On your PC either:

- **Download ZIP** of the branch `claude/database-feature-review-3zf7xg` from GitHub, or
- Clone it:
  ```
  git clone https://github.com/shanio786/pos-updated.git
  cd pos-updated
  git checkout claude/database-feature-review-3zf7xg
  ```

---

## ⚡ Easy path (do steps 2 & 4 automatically)

After SQL Server Express is installed, just **double-click `Setup\Setup.bat`**.
It finds your SQL Server, creates the `APOSDB` database, and sets the app's
connection string for you — no SSMS, no typing. Then jump to **step 3 (build)**
and **step 5 (license)**.

Prefer to do it by hand? Follow steps 2 and 4 below instead.

---

## 2. Create the database

1. Open **SSMS** and connect to your SQL Server (e.g. server name `.\SQLEXPRESS`).
2. **File → Open → File…** and open `Database/APOSDB_MSSQL.sql`.
3. Press **Execute** (F5).

That creates the `APOSDB` database with all tables, views, indexes, a default
**`admin / admin`** login, and a walk-in customer. Zero manual steps after this.

---

## 3. Build the app

1. Double-click **`Adv_POS.sln`** to open it in Visual Studio.
2. Set configuration to **Release** (top toolbar dropdown).
3. **Build → Build Solution** (Ctrl+Shift+B).

The finished program is at:
```
supershop\bin\Release\Adv_POS.exe
```
(plus its `Adv_POS.exe.config` and DLLs — copy the **whole** `Release` folder when
moving to another PC).

---

## 4. Point the app at your database

Open **`supershop\bin\Release\Adv_POS.exe.config`** in Notepad and check the
connection string near the top:

```xml
<add name="supershop.Properties.Settings.APOSSQLConnectionString"
     connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=APOSDB;Integrated Security=True;" />
```

- Same PC as SQL Server → leave `Data Source=.\SQLEXPRESS` as is.
- Different server PC → change it to `Data Source=SERVER-PC\SQLEXPRESS`
  (put the server computer's name).

Save the file. (For one server + many counters, see
`Database/Multi_Terminal_Setup.md`.)

---

## 5. License / activation (one time per PC)

The app is protected with an **offline machine-locked key** (no internet needed).
Because **you are the supplier**, you make the keys yourself:

1. **Once**, make your own key pair:
   ```
   Tools\LicenseKeyGen\LicenseKeyGen.exe genkeys
   ```
   Paste the contents of the generated `public_key.xml` into
   `supershop\Licensing\LicenseManager.cs` (the `PublicKeyXml` value), then
   **rebuild** (step 3). Keep `private_key.xml` secret — it never leaves your PC.

2. Run `Adv_POS.exe`. The **Activation** screen shows this PC's **Machine ID**.
3. Make a key for that Machine ID:
   ```
   Tools\LicenseKeyGen\LicenseKeyGen.exe sign <MACHINE-ID> 0 STD
   ```
   (`0` = never expires; use e.g. `20271231` for a dated license.)
4. Paste the printed key into the Activation screen → **Activate**. Done — that
   key works only on that computer.

Full details: **`LICENSING.md`**.

---

## 6. First run

1. Start `Adv_POS.exe`.
2. Log in as **`admin`** / **`admin`**.
3. **Change the admin password** immediately.
4. Open **Config** and enter your shop name, address, phone, tax %, receipt
   header/footer, and logo.
5. Add your categories and products (or scan/enter barcodes — use the **Auto**
   button on the Add Item screen to generate a barcode for items that don't have
   one).

You're ready to sell. 🎉

---

## What's new in this build (quick reference)

- **Fast** everywhere — pooled connections, parameterised queries, direct-print
  80 mm thermal receipt (no slow ReportViewer), fast grid reports with CSV export.
- **Discounts** — percentage **and** flat Rs, at the counter **and** per item.
- **Wholesale / 2nd price** — tick *Wholesale price* on the sale screen to bill at
  the wholesale rate.
- **Weighing-scale barcodes** — read price/weight embedded in the barcode
  (enable in `app.config`).
- **Low-stock reminder** at login for admins/managers.
- **Home dashboard** — today's sales, cash, dues, month total, low-stock, top items.
- **In-app backup & restore** of the database (`.bak`).
- **Barcode label printing** (Code 128) and **auto EAN-13** generation.
- **Modern flat theme** (turn off with `ModernTheme=false` in `app.config`).
- **Auto database upgrade** on startup (safety net; never deletes data).

## If something goes wrong

- App won't start / DB error → check the connection string (step 4) and that SQL
  Server is running. Errors are logged to `Logs\` next to the app.
- Receipt printer → set `ReceiptWidthMm` (58 or 80) in `Adv_POS.exe.config`.
- Want the classic look → set `ModernTheme` to `false` in `Adv_POS.exe.config`.

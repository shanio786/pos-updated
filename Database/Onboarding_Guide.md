# Installing Adv POS — new shop and existing (old-software) shop

Plain steps for both cases. The software is designed so this is easy.

## Case A — brand new shop (no old data)

1. Install **SQL Server** (Express is free) on the main/server PC.
2. In SSMS, open `Database/APOSDB_MSSQL.sql` and press **Execute**. This creates
   the `APOSDB` database with everything, plus an `admin / admin` login and a
   walk-in customer.
3. Put the app on each PC and set the connection string in `Adv_POS.exe.config`
   (see `Database/Multi_Terminal_Setup.md` for one server + many counters).
4. Run the app, activate the license, log in as `admin / admin`, change the
   password, and enter your shop details in **Config**.

## Case B — shop that already runs the OLD software (has data)

The customer already has an `APOSDB` in their SQL Server, full of their sales,
stock and customers. You do **not** re-create it — you upgrade it.

**Easiest way (automatic):**
1. Take a backup first (always):
   ```sql
   BACKUP DATABASE APOSDB TO DISK = 'C:\APOSDB_before_upgrade.bak';
   ```
2. Install the **new** software on that PC and point its
   `Adv_POS.exe.config` at the **same** old `APOSDB`.
3. Start the new software. On the first start it **upgrades the old database by
   itself** — it adds the new tables and columns it needs (SaleType, Shopid,
   day-close / hold / split-payment tables, etc.) without deleting anything.
   All the old sales, stock and customers stay exactly as they were.
4. Done. The shop keeps its history and gets all the new features.

> The app only adds what is missing. It never deletes or overwrites old data.

**Optional (recommended for a technician): full ID-type clean-up.**
The old database sometimes stored the same id as different types in different
tables. The app works fine either way, but to make everything one consistent
type, run once in SSMS (after the backup):
```
Database/Migrate_Existing_APOSDB.sql
```
It is safe to run more than once.

## How to move an old database to a new server

1. On the old PC: `BACKUP DATABASE APOSDB TO DISK = 'C:\APOSDB.bak';`
2. Copy `APOSDB.bak` to the new server.
3. On the new server, in SSMS: **Restore Database** → from `APOSDB.bak`.
4. Point the new software at it — it auto-upgrades on first start (Case B).

## Quick checklist

- [ ] Backup taken before touching an existing database
- [ ] New software points at the correct database in `Adv_POS.exe.config`
- [ ] License activated on each PC
- [ ] Shop details set in Config; admin password changed
- [ ] One server, terminals point to its IP (multi-counter shops)

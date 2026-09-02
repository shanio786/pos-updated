# Adv_POS – MS SQL Server database (main)

MS SQL Server is the **only** database this application uses now.
The old SQLite (`psodb.db`) and MySQL editions are not supported by the code any more.

## Files

| File | Use it when |
|------|-------------|
| `APOSDB_MSSQL.sql` | New installation. Creates database `APOSDB`, all tables, views, indexes and minimum seed data (admin user, walk-in customer, one terminal). |
| `Migrate_Existing_APOSDB.sql` | You already have an `APOSDB` created from `POS_SQL/APOSDB_2017-10-05.sql` or `NEW CODE DB SCRIPT.txt`. Adds the missing tables/columns and converts the ID columns to one type. Take a backup first. |

## Setup (new install)

1. Install SQL Server (Express is enough) and SQL Server Management Studio.
2. Open `APOSDB_MSSQL.sql` in SSMS and press **Execute**.
3. Edit the connection string in `supershop/app.config`
   (after publishing: `Adv_POS.exe.config` next to the exe):

   ```xml
   <add name="supershop.Properties.Settings.APOSSQLConnectionString"
        connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=APOSDB;Integrated Security=True;"
        providerName="System.Data.SqlClient" />
   ```

   For a SQL login use  
   `Data Source=SERVER\INSTANCE,1433;Initial Catalog=APOSDB;User ID=posuser;Password=...;`
4. Run the application and log in with `admin` / `admin`. Change the password immediately
   (Users → Manage Users).

## ID conventions (one type per kind of ID)

| Kind of ID | Type | Columns |
|------------|------|---------|
| Row id (surrogate key) | `bigint IDENTITY` | `usermgt.id`, `sales_item.item_id`, `tbl_customer.ID`, `tbl_expense.ID`, … |
| Invoice / sale number | `bigint` | `sales_payment.sales_id` (PK), `sales_item.sales_id`, `tbl_duepayment.sales_id`, `return_item.SoldInvoiceNo` |
| Customer id | `bigint` | `tbl_customer.ID`, `sales_payment.c_id`, `tbl_duepayment.custid`, `return_item.custno`, `tbl_CustCredit.CustID` |
| Sold-line id | `bigint` | `sales_item.item_id` |
| Returned product code | `varchar(50)` | `return_item.item_id` (= `purchase.product_id`) |
| Product / barcode | `varchar(50)` | `purchase.product_id` (PK), `tbl_purchase_history.product_id`, `sales_item.itemcode` |
| Shop / branch | `varchar(50)` | `Shopid` in every table |
| User reference | `varchar(100)` | `usermgt.Username`, `sales_payment.emp_id`, `tbl_duepayment.emp_id`, `return_item.emp`, `tbl_payroll.user_name`, … |

Before this change the same ID was `bigint` in one table and `varchar(50/150/250)` in
another (for example `sales_payment.sales_id bigint` joined to `sales_item.sales_id varchar(150)`),
which forces implicit conversions on every join and lets bad values in.

## What was missing in the old scripts and is now included

* `sales_payment.SaleType` (reports filter on it) – default `'CashSale'`
* `tbl_duepayment.Shopid`, `tbl_duepayment.emp_id`, `return_item.Shopid` (from `DB New Change.txt`)
* `tbl_adv_sal.bal_amnt` (used by PayRoll) and a primary key for `tbl_adv_sal`
* `userattendence`, `tbl_payroll`, `tbl_adv_sal` (not in the 2017 backup)
* Primary keys on `usermgt`, `tbl_customer`, `tbl_CustCredit`, `tbl_category`, `tbl_saleInfo`, `storeconfig`
* Unique `usermgt.Username` and unique `tbl_terminalLocation.Shopid`
* Indexes on the columns the app joins and filters on
* Walk-in customer `10000009` that the sales screens use by default

## Known limitations kept on purpose (need code changes to fix)

* Dates (`sales_time`, `receivedate`, `purchase_date`, `DOB`, payroll dates …) are still `varchar`
  because the code writes and compares them as text (`yyyy-MM-dd`). Converting them to `date`
  needs every query that builds date strings to be changed at the same time.
* No foreign keys yet. The application inserts `''`/`0` customer ids in some paths; add FKs only
  after the data is cleaned.
* Passwords are stored as PBKDF2 hashes (`usermgt.password` is `varchar(255)`); an old plain-text password is upgraded automatically the first time that user logs in.


## Optional features (Day Close, Hold sale, Split payment)

`Feature_Tables.sql` (also inside `APOSDB_MSSQL.sql`) adds:

| Table | Feature |
|-------|---------|
| `tbl_dayclose` | End-of-day cash reconciliation / Z-Report (Reports menu). |
| `tbl_held_sale`, `tbl_held_item` | Hold a cart and Resume it later (Hold / Resume buttons on the sales screens). |
| `tbl_sale_tender` | One row per payment tender; lets a sale be paid part cash + part card (Split Payment button). Normal sales record a single tender too. |

On an existing database run `Feature_Tables.sql` once; it is safe to re-run.

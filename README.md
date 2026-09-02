# Adv_POS – Point of Sale (C# WinForms, MS SQL Server)

Desktop point-of-sale system: sales register, invoices, returns, stock, customers and credit,
expenses, HR (attendance, payroll), reports, barcodes.  Three roles: Admin, Manager, Salesman.

## Requirements

* Windows, .NET Framework 4.8
* SQL Server 2012 or newer (Express is fine)
* Visual Studio 2017 or newer to build
* SAP Crystal Reports runtime for .NET (13.x) on the machine that runs the reports

## First-time setup

1. Create the database: open `Database/APOSDB_MSSQL.sql` in SQL Server Management Studio and execute it.
   (Existing databases: run `Database/Migrate_Existing_APOSDB.sql` instead – take a backup first.)
2. Set the connection string in `supershop/app.config` (after publishing: `Adv_POS.exe.config`):
   ```xml
   <add name="supershop.Properties.Settings.APOSSQLConnectionString"
        connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=APOSDB;Integrated Security=True;"
        providerName="System.Data.SqlClient" />
   ```
3. Open `Adv_POS.sln`, restore NuGet packages, build, run.
4. Log in with `admin` / `admin` and change the password (Users → Manage Users).

## Project layout

| Folder | Contents |
|--------|----------|
| `supershop/` | The application (`Adv_POS.csproj`) |
| `supershop/lib/` | Third-party DLLs that are not on NuGet (itextsharp, Spire.Barcode, MyBarcode) |
| `Database/` | SQL scripts, README with the ID conventions, legacy backups, sample import file |
| `Setup/` | Visual Studio installer project |

## How the code talks to the database

Everything goes through `DataAccess` (`supershop/DataAccess_MSSQLServer.cs`):

```csharp
DataTable dt = DataAccess.GetDataTable("select * from purchase where product_id = @id", DataAccess.P("@id", code));
DataAccess.ExecuteSQL("update purchase set product_quantity = product_quantity - @q where product_id = @id",
                      DataAccess.P("@q", qty), DataAccess.P("@id", code));
DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
{
    long invoice = tx.NextSalesId();          // safe with several terminals
    tx.Execute("insert into sales_payment ...", ...);
});
```

* One pooled connection per call, always parameterised (no SQL injection).
* Passwords are PBKDF2 hashes (`PasswordHasher`); old plain-text passwords are upgraded at first login.
* Errors are written to `%LocalAppData%\Adv_POS\logs\pos-yyyy-MM-dd.log` (`Logger`).

## Runtime folders (created automatically next to the exe)

`IMAGE` (user photos), `ITEMIMAGE` (product images), `FinalImage`, `ExpenseAttachment`, `InvoicePdf`.

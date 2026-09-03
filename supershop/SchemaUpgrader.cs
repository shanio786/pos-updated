using System;
using System.Collections.Generic;

namespace supershop
{
    /// <summary>
    /// Makes an OLD APOSDB work with this new software automatically.
    ///
    /// When the app starts it points at whatever database the connection string
    /// names. This class adds any tables/columns/views/indexes the new code needs
    /// but the old database may be missing. Every statement is written so it is
    /// SAFE to run on every start-up (checks before it changes) and NEVER deletes
    /// or overwrites data. The heavier ID-type unification stays in
    /// Database/Migrate_Existing_APOSDB.sql for a DBA to run with a backup.
    ///
    /// If a step fails (e.g. the SQL login has no ALTER permission) it is logged
    /// and the app carries on.
    /// </summary>
    public static class SchemaUpgrader
    {
        public static void EnsureSchema()
        {
            foreach (string sql in Statements())
            {
                try { DataAccess.ExecuteSQL(sql); }
                catch (Exception ex) { Logger.Error("SchemaUpgrader", ex); }
            }
            // views must each be their own batch (CREATE VIEW must be first) -> drop then create
            RecreateViews();
        }

        static IEnumerable<string> Statements()
        {
            // ---- missing columns the new code writes ----
            yield return Col("usermgt", "basic_salary", "varchar(50)");
            yield return Col("usermgt", "joning_date", "varchar(50)");
            yield return Col("usermgt", "in_time", "varchar(50)");
            yield return Col("usermgt", "out_time", "varchar(50)");
            yield return Col("usermgt", "shopname", "varchar(100)");
            yield return "IF COL_LENGTH('dbo.usermgt','password') < 255 ALTER TABLE dbo.usermgt ALTER COLUMN password varchar(255) NULL";
            yield return "IF COL_LENGTH('dbo.sales_payment','SaleType') IS NULL ALTER TABLE dbo.sales_payment ADD SaleType varchar(50) NULL CONSTRAINT DF_sales_payment_SaleType DEFAULT ('CashSale')";
            yield return "UPDATE dbo.sales_payment SET SaleType = 'CashSale' WHERE SaleType IS NULL";
            yield return Col("tbl_duepayment", "Shopid", "varchar(50)");
            yield return Col("tbl_duepayment", "emp_id", "varchar(100)");
            yield return Col("return_item", "Shopid", "varchar(50)");
            yield return Col("tbl_adv_sal", "bal_amnt", "decimal(18,2)");

            // ---- tables the old database may not have ----
            yield return @"IF OBJECT_ID('dbo.userattendence','U') IS NULL
CREATE TABLE dbo.userattendence(id bigint IDENTITY(1,1) NOT NULL, Name varchar(100) NULL, intime varchar(50) NULL, outtime varchar(50) NULL,
 att_date varchar(50) NOT NULL, userid bigint NULL, att_status varchar(50) NULL, att_month varchar(50) NULL, att_year varchar(50) NULL,
 CONSTRAINT PK_userattendence PRIMARY KEY CLUSTERED (id))";
            yield return @"IF OBJECT_ID('dbo.tbl_payroll','U') IS NULL
CREATE TABLE dbo.tbl_payroll(id bigint IDENTITY(1,1) NOT NULL, user_name varchar(100) NOT NULL, pay_month varchar(50) NOT NULL, pay_year varchar(50) NOT NULL,
 pay_date varchar(50) NULL, basic_pay varchar(50) NULL, bouns varchar(50) NULL, total_salary varchar(50) NULL, bal_amount varchar(50) NULL,
 leaves varchar(50) NULL, deducations varchar(50) NULL, net_amount varchar(50) NULL, pay_status varchar(50) NULL, paid_amount varchar(50) NULL,
 CONSTRAINT PK_tbl_payroll PRIMARY KEY CLUSTERED (id))";
            yield return @"IF OBJECT_ID('dbo.tbl_adv_sal','U') IS NULL
CREATE TABLE dbo.tbl_adv_sal(id bigint IDENTITY(1,1) NOT NULL, user_name varchar(100) NULL, adv_month varchar(50) NULL, adv_year varchar(50) NULL,
 adv_date varchar(50) NULL, adv_amount decimal(18,2) NULL, bal_amnt decimal(18,2) NULL, CONSTRAINT PK_tbl_adv_sal PRIMARY KEY CLUSTERED (id))";
            yield return @"IF OBJECT_ID('dbo.tbl_dayclose','U') IS NULL
CREATE TABLE dbo.tbl_dayclose(id bigint IDENTITY(1,1) NOT NULL, Shopid varchar(50) NULL, close_date varchar(50) NOT NULL,
 opening_cash decimal(18,2) NULL, cash_sales decimal(18,2) NULL, card_sales decimal(18,2) NULL, other_sales decimal(18,2) NULL,
 returns_total decimal(18,2) NULL, expenses_total decimal(18,2) NULL, due_received decimal(18,2) NULL, expected_cash decimal(18,2) NULL,
 counted_cash decimal(18,2) NULL, difference decimal(18,2) NULL, closed_by varchar(100) NULL,
 closed_at datetime NULL CONSTRAINT DF_tbl_dayclose_closedat DEFAULT (GETDATE()), note varchar(450) NULL,
 CONSTRAINT PK_tbl_dayclose PRIMARY KEY CLUSTERED (id))";
            yield return @"IF OBJECT_ID('dbo.tbl_held_sale','U') IS NULL
CREATE TABLE dbo.tbl_held_sale(hold_id bigint IDENTITY(1,1) NOT NULL, label varchar(100) NULL, Shopid varchar(50) NULL, emp_id varchar(100) NULL,
 cust_id varchar(50) NULL, created_at datetime NULL CONSTRAINT DF_tbl_held_sale_created DEFAULT (GETDATE()),
 CONSTRAINT PK_tbl_held_sale PRIMARY KEY CLUSTERED (hold_id))";
            yield return @"IF OBJECT_ID('dbo.tbl_held_item','U') IS NULL
CREATE TABLE dbo.tbl_held_item(id bigint IDENTITY(1,1) NOT NULL, hold_id bigint NOT NULL, itemcode varchar(50) NULL, itemName nvarchar(250) NULL,
 Qty decimal(18,2) NULL, RetailsPrice decimal(18,2) NULL, Total decimal(18,2) NULL, disamt decimal(18,2) NULL, taxamt decimal(18,2) NULL,
 disrate decimal(18,2) NULL, taxapply varchar(10) NULL, kitchendisplay int NULL, CONSTRAINT PK_tbl_held_item PRIMARY KEY CLUSTERED (id))";
            yield return @"IF OBJECT_ID('dbo.tbl_sale_tender','U') IS NULL
CREATE TABLE dbo.tbl_sale_tender(id bigint IDENTITY(1,1) NOT NULL, sales_id bigint NOT NULL, method varchar(50) NULL, amount decimal(18,2) NULL,
 logdate datetime NULL CONSTRAINT DF_tbl_sale_tender_logdate DEFAULT (GETDATE()), CONSTRAINT PK_tbl_sale_tender PRIMARY KEY CLUSTERED (id))";

            // ---- walk-in customer used by the sales screens ----
            yield return @"IF NOT EXISTS (SELECT 1 FROM dbo.tbl_customer WHERE ID = 10000009)
BEGIN SET IDENTITY_INSERT dbo.tbl_customer ON;
 INSERT dbo.tbl_customer (ID, Name, EmailAddress, Phone, Address, City, PeopleType) VALUES (10000009,'Walk-in Customer','','','','','Customer');
 SET IDENTITY_INSERT dbo.tbl_customer OFF; END";

            // ---- helpful indexes (created only if missing) ----
            yield return Idx("IX_sales_item_sales_id", "sales_item", "sales_id");
            yield return Idx("IX_sales_item_sales_time", "sales_item", "sales_time");
            yield return Idx("IX_sales_payment_sales_time", "sales_payment", "sales_time");
            yield return Idx("IX_return_item_SoldInvoiceNo", "return_item", "SoldInvoiceNo");
            yield return Idx("IX_tbl_sale_tender_sales", "tbl_sale_tender", "sales_id");
            yield return Idx("IX_tbl_held_item_hold", "tbl_held_item", "hold_id");
        }

        static string Col(string table, string col, string type)
        {
            return "IF COL_LENGTH('dbo." + table + "','" + col + "') IS NULL ALTER TABLE dbo." + table + " ADD " + col + " " + type + " NULL";
        }

        static string Idx(string name, string table, string cols)
        {
            return "IF OBJECT_ID('dbo." + table + "','U') IS NOT NULL AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='" + name + "') " +
                   "CREATE INDEX " + name + " ON dbo." + table + " (" + cols + ")";
        }

        // Views are (re)created so old databases pick up the corrected definitions.
        static void RecreateViews()
        {
            View("vw_General_Ledger",
                "CREATE VIEW dbo.vw_General_Ledger AS " +
                "SELECT sp.sales_time AS [Date], SUM(sp.payment_amount) AS Sales, ISNULL(SUM(r.ReturnAmount),0) AS [Return] " +
                "FROM dbo.sales_payment sp LEFT OUTER JOIN (SELECT SoldInvoiceNo, " +
                " SUM(ISNULL(Total,0))-SUM(ISNULL(disamt,0))+SUM(ISNULL(vatamt,0)) AS ReturnAmount " +
                " FROM dbo.return_item GROUP BY SoldInvoiceNo) r ON r.SoldInvoiceNo = sp.sales_id GROUP BY sp.sales_time");
            View("vw_CustCreditReport",
                "CREATE VIEW dbo.vw_CustCreditReport AS " +
                "SELECT cc.ID AS TrxID, cc.[Date], c.ID AS CustID, c.Name, cc.OrderID, cc.Credit, cc.Description " +
                "FROM dbo.tbl_CustCredit cc LEFT OUTER JOIN dbo.tbl_customer c ON cc.CustID = c.ID");
            View("vw_itemdisplay_sr", "CREATE VIEW dbo.vw_itemdisplay_sr AS SELECT TOP 12 * FROM dbo.purchase ORDER BY NEWID()");
        }

        static void View(string name, string createSql)
        {
            try { DataAccess.ExecuteSQL("IF OBJECT_ID('dbo." + name + "','V') IS NOT NULL DROP VIEW dbo." + name); }
            catch (Exception ex) { Logger.Error("drop view " + name, ex); }
            try { DataAccess.ExecuteSQL(createSql); }
            catch (Exception ex) { Logger.Error("create view " + name, ex); }
        }
    }
}

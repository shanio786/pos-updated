/* =====================================================================
   Adv_POS  -  Upgrade an EXISTING APOSDB (2017 backup / NEW CODE DB SCRIPT)
   to the unified MS SQL schema in APOSDB_MSSQL.sql
   ---------------------------------------------------------------------
   * Safe to run more than once (every step checks before it changes).
   * TAKE A BACKUP FIRST:  BACKUP DATABASE APOSDB TO DISK = 'C:\APOSDB.bak'
   * Run in SSMS against APOSDB.
   ===================================================================== */
USE [APOSDB];
GO
SET NOCOUNT ON;
GO

/* ---------------------------------------------------------------------
   1. Tables that the newer code needs but the 2017 backup does not have
   --------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.userattendence', N'U') IS NULL
CREATE TABLE dbo.userattendence (
    id bigint IDENTITY(1,1) NOT NULL, Name varchar(100) NULL, intime varchar(50) NULL, outtime varchar(50) NULL,
    att_date varchar(50) NOT NULL, userid bigint NULL, att_status varchar(50) NULL, att_month varchar(50) NULL, att_year varchar(50) NULL,
    CONSTRAINT PK_userattendence PRIMARY KEY CLUSTERED (id));

IF OBJECT_ID(N'dbo.tbl_payroll', N'U') IS NULL
CREATE TABLE dbo.tbl_payroll (
    id bigint IDENTITY(1,1) NOT NULL, user_name varchar(100) NOT NULL, pay_month varchar(50) NOT NULL, pay_year varchar(50) NOT NULL,
    pay_date varchar(50) NULL, basic_pay varchar(50) NULL, bouns varchar(50) NULL, total_salary varchar(50) NULL, bal_amount varchar(50) NULL,
    leaves varchar(50) NULL, deducations varchar(50) NULL, net_amount varchar(50) NULL, pay_status varchar(50) NULL, paid_amount varchar(50) NULL,
    CONSTRAINT PK_tbl_payroll PRIMARY KEY CLUSTERED (id));

IF OBJECT_ID(N'dbo.tbl_adv_sal', N'U') IS NULL
CREATE TABLE dbo.tbl_adv_sal (
    id bigint IDENTITY(1,1) NOT NULL, user_name varchar(100) NULL, adv_month varchar(50) NULL, adv_year varchar(50) NULL,
    adv_date varchar(50) NULL, adv_amount decimal(18,2) NULL, bal_amnt decimal(18,2) NULL,
    CONSTRAINT PK_tbl_adv_sal PRIMARY KEY CLUSTERED (id));

IF OBJECT_ID(N'dbo.MNU_USERROLE', N'U') IS NULL
CREATE TABLE dbo.MNU_USERROLE (
    id bigint IDENTITY(1,1) NOT NULL, UID varchar(100) NOT NULL, FRM_CODE varchar(50) NOT NULL,
    status int NOT NULL CONSTRAINT DF_MNU_USERROLE_status DEFAULT ((1)),
    CONSTRAINT PK_MNU_USERROLE PRIMARY KEY CLUSTERED (id));
GO

/* ---------------------------------------------------------------------
   2. Missing columns
   --------------------------------------------------------------------- */
IF COL_LENGTH('dbo.usermgt','basic_salary') IS NULL ALTER TABLE dbo.usermgt ADD basic_salary varchar(50) NULL;
IF COL_LENGTH('dbo.usermgt','joning_date')  IS NULL ALTER TABLE dbo.usermgt ADD joning_date  varchar(50) NULL;
IF COL_LENGTH('dbo.usermgt','in_time')      IS NULL ALTER TABLE dbo.usermgt ADD in_time      varchar(50) NULL;
IF COL_LENGTH('dbo.usermgt','out_time')     IS NULL ALTER TABLE dbo.usermgt ADD out_time     varchar(50) NULL;
IF COL_LENGTH('dbo.usermgt','shopname')     IS NULL ALTER TABLE dbo.usermgt ADD shopname     varchar(100) NULL;

IF COL_LENGTH('dbo.sales_payment','SaleType') IS NULL
    ALTER TABLE dbo.sales_payment ADD SaleType varchar(50) NULL CONSTRAINT DF_sales_payment_SaleType DEFAULT ('CashSale');

IF COL_LENGTH('dbo.tbl_duepayment','Shopid') IS NULL ALTER TABLE dbo.tbl_duepayment ADD Shopid varchar(50)  NULL;
IF COL_LENGTH('dbo.tbl_duepayment','emp_id') IS NULL ALTER TABLE dbo.tbl_duepayment ADD emp_id varchar(100) NULL;
IF COL_LENGTH('dbo.return_item','Shopid')    IS NULL ALTER TABLE dbo.return_item    ADD Shopid varchar(50)  NULL;

IF COL_LENGTH('dbo.tbl_adv_sal','bal_amnt') IS NULL ALTER TABLE dbo.tbl_adv_sal ADD bal_amnt decimal(18,2) NULL;
IF COL_LENGTH('dbo.tbl_adv_sal','id')       IS NULL ALTER TABLE dbo.tbl_adv_sal ADD id bigint IDENTITY(1,1) NOT NULL;
GO

/* Back-fill the new columns (same values "DB New Change.txt" used) */
UPDATE dbo.sales_payment  SET SaleType = 'CashSale' WHERE SaleType IS NULL;
UPDATE dbo.tbl_duepayment SET Shopid = (SELECT TOP 1 Shopid FROM dbo.tbl_terminalLocation ORDER BY ID) WHERE Shopid IS NULL;
UPDATE dbo.tbl_duepayment SET emp_id = 'admin' WHERE emp_id IS NULL;
UPDATE dbo.return_item    SET Shopid = (SELECT TOP 1 Shopid FROM dbo.tbl_terminalLocation ORDER BY ID) WHERE Shopid IS NULL;
GO

/* ---------------------------------------------------------------------
   3. Unify ID data types  (varchar  ->  bigint)
      Any value that is not a whole number is set to NULL first so the
      conversion cannot fail.  Review the SELECTs below before running if
      you want to see what would be lost.
   --------------------------------------------------------------------- */
-- SELECT * FROM dbo.sales_item     WHERE sales_id      = '' OR sales_id      LIKE '%[^0-9]%';
-- SELECT * FROM dbo.sales_payment  WHERE c_id          = '' OR c_id          LIKE '%[^0-9]%';
-- SELECT * FROM dbo.tbl_duepayment WHERE custid        = '' OR custid        LIKE '%[^0-9]%';
-- SELECT * FROM dbo.return_item    WHERE SoldInvoiceNo = '' OR SoldInvoiceNo LIKE '%[^0-9]%';

DECLARE @sql nvarchar(max);

-- sales_item.sales_id  varchar(150) -> bigint
IF EXISTS (SELECT 1 FROM sys.columns c JOIN sys.types t ON c.user_type_id = t.user_type_id
           WHERE c.object_id = OBJECT_ID('dbo.sales_item') AND c.name = 'sales_id' AND t.name <> 'bigint')
BEGIN
    EXEC('UPDATE dbo.sales_item SET sales_id = NULL WHERE sales_id = '''' OR sales_id LIKE ''%[^0-9]%''');
    EXEC('ALTER TABLE dbo.sales_item ALTER COLUMN sales_id bigint NULL');
END

-- sales_payment.c_id  varchar(150) -> bigint
IF EXISTS (SELECT 1 FROM sys.columns c JOIN sys.types t ON c.user_type_id = t.user_type_id
           WHERE c.object_id = OBJECT_ID('dbo.sales_payment') AND c.name = 'c_id' AND t.name <> 'bigint')
BEGIN
    EXEC('UPDATE dbo.sales_payment SET c_id = NULL WHERE c_id = '''' OR c_id LIKE ''%[^0-9]%''');
    EXEC('ALTER TABLE dbo.sales_payment ALTER COLUMN c_id bigint NULL');
END

-- tbl_duepayment.custid  varchar(50) -> bigint
IF EXISTS (SELECT 1 FROM sys.columns c JOIN sys.types t ON c.user_type_id = t.user_type_id
           WHERE c.object_id = OBJECT_ID('dbo.tbl_duepayment') AND c.name = 'custid' AND t.name <> 'bigint')
BEGIN
    EXEC('UPDATE dbo.tbl_duepayment SET custid = NULL WHERE custid = '''' OR custid LIKE ''%[^0-9]%''');
    EXEC('ALTER TABLE dbo.tbl_duepayment ALTER COLUMN custid bigint NULL');
END

-- return_item.item_id / custno / SoldInvoiceNo  varchar(150) -> bigint
IF EXISTS (SELECT 1 FROM sys.columns c JOIN sys.types t ON c.user_type_id = t.user_type_id
           WHERE c.object_id = OBJECT_ID('dbo.return_item') AND c.name = 'item_id' AND t.name <> 'bigint')
BEGIN
    EXEC('UPDATE dbo.return_item SET item_id = NULL WHERE item_id = '''' OR item_id LIKE ''%[^0-9]%''');
    EXEC('ALTER TABLE dbo.return_item ALTER COLUMN item_id bigint NULL');
END
IF EXISTS (SELECT 1 FROM sys.columns c JOIN sys.types t ON c.user_type_id = t.user_type_id
           WHERE c.object_id = OBJECT_ID('dbo.return_item') AND c.name = 'custno' AND t.name <> 'bigint')
BEGIN
    EXEC('UPDATE dbo.return_item SET custno = NULL WHERE custno = '''' OR custno LIKE ''%[^0-9]%''');
    EXEC('ALTER TABLE dbo.return_item ALTER COLUMN custno bigint NULL');
END
IF EXISTS (SELECT 1 FROM sys.columns c JOIN sys.types t ON c.user_type_id = t.user_type_id
           WHERE c.object_id = OBJECT_ID('dbo.return_item') AND c.name = 'SoldInvoiceNo' AND t.name <> 'bigint')
BEGIN
    EXEC('UPDATE dbo.return_item SET SoldInvoiceNo = NULL WHERE SoldInvoiceNo = '''' OR SoldInvoiceNo LIKE ''%[^0-9]%''');
    EXEC('ALTER TABLE dbo.return_item ALTER COLUMN SoldInvoiceNo bigint NULL');
END

-- tbl_adv_sal.adv_amount  float -> decimal(18,2)
IF EXISTS (SELECT 1 FROM sys.columns c JOIN sys.types t ON c.user_type_id = t.user_type_id
           WHERE c.object_id = OBJECT_ID('dbo.tbl_adv_sal') AND c.name = 'adv_amount' AND t.name = 'float')
    EXEC('ALTER TABLE dbo.tbl_adv_sal ALTER COLUMN adv_amount decimal(18,2) NULL');
GO

/* ---------------------------------------------------------------------
   4. Unify varchar lengths for the string-type IDs (only when the data fits)
   --------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM dbo.purchase WHERE LEN(product_id) > 50)
   AND EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.purchase') AND name = 'product_id' AND max_length > 50)
BEGIN
    -- product_id is the PK: drop, alter, recreate
    DECLARE @pk sysname = (SELECT name FROM sys.key_constraints WHERE parent_object_id = OBJECT_ID('dbo.purchase') AND type = 'PK');
    IF @pk IS NOT NULL EXEC('ALTER TABLE dbo.purchase DROP CONSTRAINT [' + @pk + ']');
    EXEC('ALTER TABLE dbo.purchase ALTER COLUMN product_id varchar(50) NOT NULL');
    EXEC('ALTER TABLE dbo.purchase ADD CONSTRAINT PK_purchase PRIMARY KEY CLUSTERED (product_id)');
END

IF NOT EXISTS (SELECT 1 FROM dbo.sales_item WHERE LEN(itemcode) > 50)
   AND EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.sales_item') AND name = 'itemcode' AND max_length > 50)
    EXEC('ALTER TABLE dbo.sales_item ALTER COLUMN itemcode varchar(50) NULL');

IF NOT EXISTS (SELECT 1 FROM dbo.sales_payment WHERE LEN(Shopid) > 50)
   AND EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.sales_payment') AND name = 'Shopid' AND max_length > 50)
    EXEC('ALTER TABLE dbo.sales_payment ALTER COLUMN Shopid varchar(50) NULL');

IF NOT EXISTS (SELECT 1 FROM dbo.usermgt WHERE LEN(Shopid) > 50)
   AND EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.usermgt') AND name = 'Shopid' AND max_length > 50)
    EXEC('ALTER TABLE dbo.usermgt ALTER COLUMN Shopid varchar(50) NULL');

IF NOT EXISTS (SELECT 1 FROM dbo.tbl_terminalLocation WHERE LEN(Shopid) > 50 OR Shopid IS NULL)
   AND EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.tbl_terminalLocation') AND name = 'Shopid' AND (max_length > 50 OR is_nullable = 1))
    EXEC('ALTER TABLE dbo.tbl_terminalLocation ALTER COLUMN Shopid varchar(50) NOT NULL');
GO

/* ---------------------------------------------------------------------
   5. Primary keys that the old script never created
   --------------------------------------------------------------------- */
IF OBJECTPROPERTY(OBJECT_ID('dbo.usermgt'),      'TableHasPrimaryKey') = 0 ALTER TABLE dbo.usermgt       ADD CONSTRAINT PK_usermgt       PRIMARY KEY CLUSTERED (id);
IF OBJECTPROPERTY(OBJECT_ID('dbo.tbl_customer'), 'TableHasPrimaryKey') = 0 ALTER TABLE dbo.tbl_customer  ADD CONSTRAINT PK_tbl_customer  PRIMARY KEY CLUSTERED (ID);
IF OBJECTPROPERTY(OBJECT_ID('dbo.tbl_CustCredit','TableHasPrimaryKey') = 0 ALTER TABLE dbo.tbl_CustCredit ADD CONSTRAINT PK_tbl_CustCredit PRIMARY KEY CLUSTERED (ID);
IF OBJECTPROPERTY(OBJECT_ID('dbo.tbl_category'), 'TableHasPrimaryKey') = 0 ALTER TABLE dbo.tbl_category  ADD CONSTRAINT PK_tbl_category  PRIMARY KEY CLUSTERED (ID);
IF OBJECTPROPERTY(OBJECT_ID('dbo.tbl_saleInfo'), 'TableHasPrimaryKey') = 0 ALTER TABLE dbo.tbl_saleInfo  ADD CONSTRAINT PK_tbl_saleInfo  PRIMARY KEY CLUSTERED (ID);
IF OBJECTPROPERTY(OBJECT_ID('dbo.storeconfig'),  'TableHasPrimaryKey') = 0 ALTER TABLE dbo.storeconfig   ADD CONSTRAINT PK_storeconfig   PRIMARY KEY CLUSTERED (id);

/* unique username / unique shop id, only when the existing data allows it */
IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'UQ_usermgt_Username')
   AND NOT EXISTS (SELECT Username FROM dbo.usermgt GROUP BY Username HAVING COUNT(*) > 1)
   AND NOT EXISTS (SELECT 1 FROM dbo.usermgt WHERE Username IS NULL)
BEGIN
    EXEC('ALTER TABLE dbo.usermgt ALTER COLUMN Username varchar(100) NOT NULL');
    EXEC('ALTER TABLE dbo.usermgt ADD CONSTRAINT UQ_usermgt_Username UNIQUE (Username)');
END

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'UQ_tbl_terminalLocation_Shopid')
   AND NOT EXISTS (SELECT Shopid FROM dbo.tbl_terminalLocation GROUP BY Shopid HAVING COUNT(*) > 1)
   AND NOT EXISTS (SELECT 1 FROM dbo.tbl_terminalLocation WHERE Shopid IS NULL)
    EXEC('ALTER TABLE dbo.tbl_terminalLocation ADD CONSTRAINT UQ_tbl_terminalLocation_Shopid UNIQUE (Shopid)');
GO

/* ---------------------------------------------------------------------
   6. Indexes
   --------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sales_item_sales_id')        CREATE INDEX IX_sales_item_sales_id       ON dbo.sales_item (sales_id);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sales_item_sales_time')      CREATE INDEX IX_sales_item_sales_time     ON dbo.sales_item (sales_time);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sales_item_itemcode')        CREATE INDEX IX_sales_item_itemcode       ON dbo.sales_item (itemcode);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sales_payment_sales_time')   CREATE INDEX IX_sales_payment_sales_time  ON dbo.sales_payment (sales_time);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sales_payment_c_id')         CREATE INDEX IX_sales_payment_c_id        ON dbo.sales_payment (c_id);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sales_payment_Shopid')       CREATE INDEX IX_sales_payment_Shopid      ON dbo.sales_payment (Shopid);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_return_item_SoldInvoiceNo')  CREATE INDEX IX_return_item_SoldInvoiceNo ON dbo.return_item (SoldInvoiceNo);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tbl_duepayment_sales_id')    CREATE INDEX IX_tbl_duepayment_sales_id   ON dbo.tbl_duepayment (sales_id);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tbl_CustCredit_CustID')      CREATE INDEX IX_tbl_CustCredit_CustID     ON dbo.tbl_CustCredit (CustID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_purchase_product_name')      CREATE INDEX IX_purchase_product_name     ON dbo.purchase (product_name);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tbl_workrecords_Username_logdate') CREATE INDEX IX_tbl_workrecords_Username_logdate ON dbo.tbl_workrecords (Username, logdate);
GO

/* ---------------------------------------------------------------------
   7. Walk-in customer used by the sales screens (id 10000009)
   --------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tbl_customer WHERE ID = 10000009)
BEGIN
    SET IDENTITY_INSERT dbo.tbl_customer ON;
    INSERT dbo.tbl_customer (ID, Name, EmailAddress, Phone, Address, City, PeopleType)
    VALUES (10000009, 'Walk-in Customer', '', '', '', '', 'Customer');
    SET IDENTITY_INSERT dbo.tbl_customer OFF;
END
GO

/* ---------------------------------------------------------------------
   8. Refresh views so they pick up the new column types
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.vw_General_Ledger','V')  IS NOT NULL EXEC sp_refreshview 'dbo.vw_General_Ledger';
IF OBJECT_ID('dbo.vw_CustCreditReport','V') IS NOT NULL EXEC sp_refreshview 'dbo.vw_CustCreditReport';
IF OBJECT_ID('dbo.CustomerCredit','V')     IS NOT NULL EXEC sp_refreshview 'dbo.CustomerCredit';
IF OBJECT_ID('dbo.vw_workrecords','V')     IS NOT NULL EXEC sp_refreshview 'dbo.vw_workrecords';
IF OBJECT_ID('dbo.vw_itemdisplay_sr','V')  IS NOT NULL EXEC sp_refreshview 'dbo.vw_itemdisplay_sr';
GO

PRINT 'APOSDB migration finished.';
GO

/* =====================================================================
   Adv_POS  -  Tables for Day Close, Hold/Resume sale, Split payment
   Run this once on an existing APOSDB (safe to re-run).
   These tables are also included in APOSDB_MSSQL.sql for new installs.
   ===================================================================== */
USE [APOSDB];
GO

/* ---- Day close / Z-report -------------------------------------------- */
IF OBJECT_ID(N'dbo.tbl_dayclose', N'U') IS NULL
CREATE TABLE dbo.tbl_dayclose (
    id             bigint IDENTITY(1,1) NOT NULL,
    Shopid         varchar(50)   NULL,
    close_date     varchar(50)   NOT NULL,        -- 'yyyy-MM-dd'
    opening_cash   decimal(18,2) NULL,
    cash_sales     decimal(18,2) NULL,
    card_sales     decimal(18,2) NULL,
    other_sales    decimal(18,2) NULL,
    returns_total  decimal(18,2) NULL,
    expenses_total decimal(18,2) NULL,
    due_received   decimal(18,2) NULL,
    expected_cash  decimal(18,2) NULL,
    counted_cash   decimal(18,2) NULL,
    difference     decimal(18,2) NULL,
    closed_by      varchar(100)  NULL,
    closed_at      datetime      NULL CONSTRAINT DF_tbl_dayclose_closedat DEFAULT (GETDATE()),
    note           varchar(450)  NULL,
    CONSTRAINT PK_tbl_dayclose PRIMARY KEY CLUSTERED (id)
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tbl_dayclose_shop_date')
    CREATE INDEX IX_tbl_dayclose_shop_date ON dbo.tbl_dayclose (Shopid, close_date);
GO

/* ---- Held (parked) sales --------------------------------------------- */
IF OBJECT_ID(N'dbo.tbl_held_sale', N'U') IS NULL
CREATE TABLE dbo.tbl_held_sale (
    hold_id     bigint IDENTITY(1,1) NOT NULL,
    label       varchar(100)  NULL,              -- e.g. customer name / table no
    Shopid      varchar(50)   NULL,
    emp_id      varchar(100)  NULL,
    cust_id     varchar(50)   NULL,
    created_at  datetime      NULL CONSTRAINT DF_tbl_held_sale_created DEFAULT (GETDATE()),
    CONSTRAINT PK_tbl_held_sale PRIMARY KEY CLUSTERED (hold_id)
);
GO
IF OBJECT_ID(N'dbo.tbl_held_item', N'U') IS NULL
CREATE TABLE dbo.tbl_held_item (
    id          bigint IDENTITY(1,1) NOT NULL,
    hold_id     bigint        NOT NULL,
    itemcode    varchar(50)   NULL,
    itemName    nvarchar(250) NULL,
    Qty         decimal(18,2) NULL,
    RetailsPrice decimal(18,2) NULL,
    Total       decimal(18,2) NULL,
    disamt      decimal(18,2) NULL,
    taxamt      decimal(18,2) NULL,
    disrate     decimal(18,2) NULL,
    taxapply    varchar(10)   NULL,
    kitchendisplay int         NULL,
    CONSTRAINT PK_tbl_held_item PRIMARY KEY CLUSTERED (id)
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tbl_held_item_hold')
    CREATE INDEX IX_tbl_held_item_hold ON dbo.tbl_held_item (hold_id);
GO

/* ---- Split payment tenders ------------------------------------------- */
/* one row per tender (Cash / Card / ...) of a sale.  sales_payment keeps the
   grand total; payment_type is set to 'Split' when more than one tender.   */
IF OBJECT_ID(N'dbo.tbl_sale_tender', N'U') IS NULL
CREATE TABLE dbo.tbl_sale_tender (
    id        bigint IDENTITY(1,1) NOT NULL,
    sales_id  bigint        NOT NULL,
    method    varchar(50)   NULL,               -- Cash / Card / Mobile / ...
    amount    decimal(18,2) NULL,
    logdate   datetime      NULL CONSTRAINT DF_tbl_sale_tender_logdate DEFAULT (GETDATE()),
    CONSTRAINT PK_tbl_sale_tender PRIMARY KEY CLUSTERED (id)
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tbl_sale_tender_sales')
    CREATE INDEX IX_tbl_sale_tender_sales ON dbo.tbl_sale_tender (sales_id);
GO
/* ---- Optional per-product columns: wholesale price + flat (Rs) discount ---- */
IF COL_LENGTH('dbo.purchase','wholesale_price') IS NULL ALTER TABLE dbo.purchase ADD wholesale_price decimal(18,2) NULL;
IF COL_LENGTH('dbo.purchase','disc_amount')     IS NULL ALTER TABLE dbo.purchase ADD disc_amount     decimal(18,2) NULL;
GO
PRINT 'Feature tables ready.';
GO

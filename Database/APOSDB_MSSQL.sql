/* =====================================================================
   Adv_POS  -  MS SQL Server main database script  (APOSDB)
   ---------------------------------------------------------------------
   Purpose : Fresh, clean schema for the POS application with ONE
             consistent data type per kind of ID.
   Target  : SQL Server 2008 R2 / 2012 / 2014 / 2016 / 2019 / 2022 / Express
   Usage   : Open in SSMS  ->  Execute (F5).  Creates database APOSDB.
             For an EXISTING APOSDB use Migrate_Existing_APOSDB.sql instead.

   ID conventions used everywhere in this script
   ---------------------------------------------
     surrogate table id  ..............  bigint IDENTITY
     sales / invoice no  (sales_id) ...  bigint
     customer id (c_id, custid, custno)  bigint
     sold line id (sales_item.item_id)   bigint
     product / barcode id .............  varchar(50)
     shop / branch id (Shopid) ........  varchar(50)
     user reference (emp_id, emp, ...)   varchar(100)  (stores usermgt.Username)
   ===================================================================== */

USE [master];
GO
IF DB_ID(N'APOSDB') IS NULL
    CREATE DATABASE [APOSDB];
GO
USE [APOSDB];
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* =====================================================================
   1.  USERS / HR
   ===================================================================== */
IF OBJECT_ID(N'dbo.usermgt', N'U') IS NULL
CREATE TABLE dbo.usermgt (
    id            bigint IDENTITY(1,1) NOT NULL,
    Name          varchar(100)  NULL,
    Father_name   varchar(100)  NULL,
    Address       varchar(220)  NULL,
    Email         varchar(100)  NULL,
    Contact       varchar(100)  NULL,
    DOB           varchar(100)  NULL,
    Username      varchar(100)  NOT NULL,
    password      varchar(100)  NULL,
    usertype      varchar(10)   NULL,          -- 0=blocked 1=admin 2=manager 3=salesman
    position      varchar(100)  NULL,
    imagename     varchar(100)  NULL,
    Shopid        varchar(50)   NULL,
    logdate       datetime      NULL CONSTRAINT DF_usermgt_logdate DEFAULT (GETDATE()),
    basic_salary  varchar(50)   NULL,
    joning_date   varchar(50)   NULL,
    in_time       varchar(50)   NULL,
    out_time      varchar(50)   NULL,
    shopname      varchar(100)  NULL,
    CONSTRAINT PK_usermgt PRIMARY KEY CLUSTERED (id),
    CONSTRAINT UQ_usermgt_Username UNIQUE (Username)
);
GO

IF OBJECT_ID(N'dbo.userattendence', N'U') IS NULL
CREATE TABLE dbo.userattendence (
    id          bigint IDENTITY(1,1) NOT NULL,
    Name        varchar(100) NULL,               -- usermgt.Username
    intime      varchar(50)  NULL,
    outtime     varchar(50)  NULL,
    att_date    varchar(50)  NOT NULL,
    userid      bigint       NULL,               -- usermgt.id
    att_status  varchar(50)  NULL,
    att_month   varchar(50)  NULL,
    att_year    varchar(50)  NULL,
    CONSTRAINT PK_userattendence PRIMARY KEY CLUSTERED (id)
);
GO

IF OBJECT_ID(N'dbo.tbl_payroll', N'U') IS NULL
CREATE TABLE dbo.tbl_payroll (
    id            bigint IDENTITY(1,1) NOT NULL,
    user_name     varchar(100) NOT NULL,         -- usermgt.Username
    pay_month     varchar(50)  NOT NULL,
    pay_year      varchar(50)  NOT NULL,
    pay_date      varchar(50)  NULL,
    basic_pay     varchar(50)  NULL,
    bouns         varchar(50)  NULL,
    total_salary  varchar(50)  NULL,
    bal_amount    varchar(50)  NULL,
    leaves        varchar(50)  NULL,
    deducations   varchar(50)  NULL,
    net_amount    varchar(50)  NULL,
    pay_status    varchar(50)  NULL,
    paid_amount   varchar(50)  NULL,
    CONSTRAINT PK_tbl_payroll PRIMARY KEY CLUSTERED (id)
);
GO

IF OBJECT_ID(N'dbo.tbl_adv_sal', N'U') IS NULL
CREATE TABLE dbo.tbl_adv_sal (
    id          bigint IDENTITY(1,1) NOT NULL,
    user_name   varchar(100)   NULL,             -- usermgt.Username
    adv_month   varchar(50)    NULL,
    adv_year    varchar(50)    NULL,
    adv_date    varchar(50)    NULL,
    adv_amount  decimal(18,2)  NULL,
    bal_amnt    decimal(18,2)  NULL,             -- used by PayRoll.cs, was missing in old script
    CONSTRAINT PK_tbl_adv_sal PRIMARY KEY CLUSTERED (id)
);
GO

IF OBJECT_ID(N'dbo.tbl_workrecords', N'U') IS NULL
CREATE TABLE dbo.tbl_workrecords (
    id           bigint IDENTITY(1,1) NOT NULL,
    Username     varchar(100)   NULL,
    datatype     varchar(50)    NULL,            -- IN / OUT
    logdate      smalldatetime  NULL,
    logtime      smalldatetime  NULL,
    logdatetime  smalldatetime  NULL,
    status       int            NULL CONSTRAINT DF_tbl_workrecords_status DEFAULT ((1)),
    CONSTRAINT PK_tbl_workrecords PRIMARY KEY CLUSTERED (id)
);
GO

/* User -> form permission matrix (UserRole.cs).  Form is not wired to any
   menu yet, table kept so the code compiles/runs if it is enabled later. */
IF OBJECT_ID(N'dbo.MNU_USERROLE', N'U') IS NULL
CREATE TABLE dbo.MNU_USERROLE (
    id        bigint IDENTITY(1,1) NOT NULL,
    UID       varchar(100) NOT NULL,             -- usermgt.Username
    FRM_CODE  varchar(50)  NOT NULL,
    status    int          NOT NULL CONSTRAINT DF_MNU_USERROLE_status DEFAULT ((1)),
    CONSTRAINT PK_MNU_USERROLE PRIMARY KEY CLUSTERED (id)
);
GO

/* =====================================================================
   2.  STORE / TERMINAL CONFIG
   ===================================================================== */
IF OBJECT_ID(N'dbo.storeconfig', N'U') IS NULL
CREATE TABLE dbo.storeconfig (
    id              int IDENTITY(1,1) NOT NULL,
    companyname     varchar(250)  NULL,
    companyaddress  varchar(250)  NULL,
    companyphone    varchar(250)  NULL,
    vatno           varchar(250)  NULL,
    web             varchar(250)  NULL,
    vatrate         decimal(18,3) NULL,
    disrate         decimal(18,3) NULL,
    footermsg       varchar(450)  NULL,
    updatetime      datetime      NULL CONSTRAINT DF_storeconfig_updatetime DEFAULT (GETDATE()),
    CONSTRAINT PK_storeconfig PRIMARY KEY CLUSTERED (id)
);
GO

IF OBJECT_ID(N'dbo.tbl_terminalLocation', N'U') IS NULL
CREATE TABLE dbo.tbl_terminalLocation (
    ID           int IDENTITY(1,1) NOT NULL,
    CompanyName  varchar(250)  NULL,
    Branchname   varchar(150)  NULL,
    Location     varchar(430)  NULL,
    Phone        varchar(50)   NULL,
    Email        varchar(150)  NULL,
    Web          varchar(150)  NULL,
    VAT          decimal(18,3) NULL,
    Dis          decimal(18,3) NULL,
    VATRegiNo    varchar(150)  NULL,
    Shopid       varchar(50)   NOT NULL,
    Footermsg    varchar(450)  NULL,
    CONSTRAINT PK_tbl_terminalLocation PRIMARY KEY CLUSTERED (ID),
    CONSTRAINT UQ_tbl_terminalLocation_Shopid UNIQUE (Shopid)
);
GO

/* transaction counter (legacy) */
IF OBJECT_ID(N'dbo.trincro', N'U') IS NULL
CREATE TABLE dbo.trincro (
    trno bigint NOT NULL
);
GO

/* =====================================================================
   3.  ITEMS / STOCK
   ===================================================================== */
IF OBJECT_ID(N'dbo.tbl_category', N'U') IS NULL
CREATE TABLE dbo.tbl_category (
    ID             bigint IDENTITY(1,1) NOT NULL,
    category_name  varchar(250) NULL,
    CONSTRAINT PK_tbl_category PRIMARY KEY CLUSTERED (ID)
);
GO

IF OBJECT_ID(N'dbo.purchase', N'U') IS NULL
CREATE TABLE dbo.purchase (                      -- current stock, one row per product
    product_id          varchar(50)    NOT NULL, -- barcode
    product_name        nvarchar(250)  NULL,
    product_quantity    decimal(18,2)  NULL,
    cost_price          decimal(18,2)  NULL,
    retail_price        decimal(18,2)  NULL,
    total_cost_price    decimal(18,2)  NULL,
    total_retail_price  decimal(18,2)  NULL,
    category            nvarchar(150)  NULL,
    supplier            nvarchar(150)  NULL,
    imagename           varchar(250)   NULL,
    discount            decimal(18,2)  NULL,
    taxapply            int            NULL,
    Shopid              varchar(50)    NULL,
    status              int            NULL,     -- 1 active
    logDate             datetime       NULL CONSTRAINT DF_purchase_logDate    DEFAULT (GETDATE()),
    UpdateDate          datetime       NULL CONSTRAINT DF_purchase_UpdateDate DEFAULT (GETDATE()),
    Updateby            varchar(100)   NULL,
    CONSTRAINT PK_purchase PRIMARY KEY CLUSTERED (product_id)
);
GO

IF OBJECT_ID(N'dbo.tbl_purchase_history', N'U') IS NULL
CREATE TABLE dbo.tbl_purchase_history (
    id                bigint IDENTITY(1,1) NOT NULL,
    product_id        varchar(50)    NULL,       -- purchase.product_id
    product_name      nvarchar(150)  NULL,
    product_quantity  decimal(18,2)  NULL,
    cost_price        decimal(18,2)  NULL,
    retail_price      decimal(18,2)  NULL,
    category          varchar(50)    NULL,
    supplier          varchar(50)    NULL,
    purchase_date     varchar(50)    NULL,
    Shopid            varchar(50)    NULL,
    ptype             varchar(50)    NULL,       -- NEW / UPDATE
    status            int            NULL CONSTRAINT DF_tbl_purchase_history_status DEFAULT ((1)),
    CONSTRAINT PK_tbl_purchase_history PRIMARY KEY CLUSTERED (id)
);
GO

/* =====================================================================
   4.  CUSTOMERS
   ===================================================================== */
IF OBJECT_ID(N'dbo.tbl_customer', N'U') IS NULL
CREATE TABLE dbo.tbl_customer (
    ID            bigint IDENTITY(10000001,1) NOT NULL,
    Name          varchar(250) NULL,
    EmailAddress  varchar(250) NULL,
    Phone         varchar(50)  NULL,
    Address       varchar(250) NULL,
    City          varchar(50)  NULL,
    PeopleType    varchar(50)  NULL,             -- Customer / Supplier
    Logtime       datetime     NULL CONSTRAINT DF_tbl_customer_Logtime DEFAULT (GETDATE()),
    CONSTRAINT PK_tbl_customer PRIMARY KEY CLUSTERED (ID)
);
GO

IF OBJECT_ID(N'dbo.tbl_CustCredit', N'U') IS NULL
CREATE TABLE dbo.tbl_CustCredit (
    ID           bigint IDENTITY(1,1) NOT NULL,
    CustID       bigint        NOT NULL,         -- tbl_customer.ID
    OrderID      varchar(250)  NULL,
    Date         varchar(150)  NULL,
    Credit       decimal(18,2) NULL,
    Description  varchar(250)  NULL,
    Logtime      datetime      NULL CONSTRAINT DF_tbl_CustCredit_Logtime DEFAULT (GETDATE()),
    CONSTRAINT PK_tbl_CustCredit PRIMARY KEY CLUSTERED (ID)
);
GO

/* =====================================================================
   5.  SALES
   ===================================================================== */
IF OBJECT_ID(N'dbo.sales_payment', N'U') IS NULL
CREATE TABLE dbo.sales_payment (                 -- one row per invoice
    sales_id        bigint        NOT NULL,      -- invoice no
    payment_type    varchar(150)  NULL,
    payment_amount  decimal(18,2) NULL,
    change_amount   decimal(18,2) NULL,
    due_amount      decimal(18,2) NULL,
    dis             decimal(18,2) NULL,
    vat             decimal(18,2) NULL,
    sales_time      varchar(150)  NULL,          -- 'yyyy-MM-dd'
    c_id            bigint        NULL,          -- tbl_customer.ID
    emp_id          varchar(100)  NULL,          -- usermgt.Username
    comment         nvarchar(350) NULL,
    TrxType         varchar(50)   NULL,
    Shopid          varchar(50)   NULL,
    ovdisrate       decimal(18,2) NULL,
    vaterate        decimal(18,2) NULL,
    SaleType        varchar(50)   NULL CONSTRAINT DF_sales_payment_SaleType DEFAULT ('CashSale'),
    logdate         datetime      NULL CONSTRAINT DF_sales_payment_logdate DEFAULT (GETDATE()),
    CONSTRAINT PK_sales_payment PRIMARY KEY CLUSTERED (sales_id)
);
GO

IF OBJECT_ID(N'dbo.sales_item', N'U') IS NULL
CREATE TABLE dbo.sales_item (                    -- invoice lines
    item_id       bigint IDENTITY(1,1) NOT NULL,
    sales_id      bigint        NULL,            -- sales_payment.sales_id   (was varchar)
    itemName      nvarchar(250) NULL,
    Qty           decimal(18,2) NULL,
    RetailsPrice  decimal(18,2) NULL,
    Total         decimal(18,2) NULL,
    profit        decimal(18,2) NULL,
    sales_time    varchar(150)  NULL,
    itemcode      varchar(50)   NULL,            -- purchase.product_id
    discount      decimal(18,2) NULL,
    taxapply      varchar(50)   NULL,
    status        int           NULL CONSTRAINT DF_sales_item_status DEFAULT ((1)),  -- 1 sold, 2 returned, 3 partial
    logDate       datetime      NULL CONSTRAINT DF_sales_item_logDate DEFAULT (GETDATE()),
    CONSTRAINT PK_sales_item PRIMARY KEY CLUSTERED (item_id)
);
GO

IF OBJECT_ID(N'dbo.tbl_saleInfo', N'U') IS NULL
CREATE TABLE dbo.tbl_saleInfo (
    ID           bigint IDENTITY(1,1) NOT NULL,
    InvoiceNo    varchar(250)  NULL,
    WarehouseNo  varchar(250)  NULL,
    Biller       varchar(250)  NULL,
    Customer     varchar(250)  NULL,
    Note         varchar(250)  NULL,
    DisRate      decimal(18,2) NULL,
    TaxRate      decimal(18,2) NULL,
    ShippingFee  decimal(18,2) NULL,
    SoldBy       varchar(100)  NULL,
    [DateTime]   smalldatetime NULL CONSTRAINT DF_tbl_saleInfo_DateTime DEFAULT (GETDATE()),
    CONSTRAINT PK_tbl_saleInfo PRIMARY KEY CLUSTERED (ID)
);
GO

IF OBJECT_ID(N'dbo.tbl_duepayment', N'U') IS NULL
CREATE TABLE dbo.tbl_duepayment (
    id           bigint IDENTITY(1,1) NOT NULL,
    receivedate  varchar(50)   NULL,
    sales_id     bigint        NULL,             -- sales_payment.sales_id
    totalamt     decimal(18,2) NULL,
    dueamt       decimal(18,2) NULL,
    receiveamt   decimal(18,2) NULL,
    custid       bigint        NULL,             -- tbl_customer.ID   (was varchar)
    emp_id       varchar(100)  NULL,             -- added by "DB New Change.txt"
    Shopid       varchar(50)   NULL,             -- added by "DB New Change.txt"
    status       int           NULL CONSTRAINT DF_tbl_duepayment_status DEFAULT ((1)),
    CONSTRAINT PK_tbl_duepayment PRIMARY KEY CLUSTERED (id)
);
GO

IF OBJECT_ID(N'dbo.return_item', N'U') IS NULL
CREATE TABLE dbo.return_item (
    return_id      bigint IDENTITY(1,1) NOT NULL,
    item_id        bigint        NULL,           -- sales_item.item_id       (was varchar)
    itemName       nvarchar(250) NULL,
    Qty            decimal(18,2) NULL,
    RetailsPrice   decimal(18,2) NULL,
    Total          decimal(18,2) NULL,
    return_time    varchar(150)  NULL,
    custno         bigint        NULL,           -- tbl_customer.ID          (was varchar)
    emp            varchar(100)  NULL,           -- usermgt.Username
    SoldInvoiceNo  bigint        NULL,           -- sales_payment.sales_id   (was varchar)
    Comment        nvarchar(250) NULL,
    disamt         decimal(18,2) NULL,
    vatamt         decimal(18,2) NULL,
    Shopid         varchar(50)   NULL,           -- added by "DB New Change.txt"
    logdate        datetime      NULL CONSTRAINT DF_return_item_logdate DEFAULT (GETDATE()),
    CONSTRAINT PK_return_item PRIMARY KEY CLUSTERED (return_id)
);
GO

/* =====================================================================
   6.  EXPENSES
   ===================================================================== */
IF OBJECT_ID(N'dbo.tbl_expense', N'U') IS NULL
CREATE TABLE dbo.tbl_expense (
    ID             bigint IDENTITY(901,1) NOT NULL,
    [Date]         smalldatetime NULL,
    ReferenceNo    varchar(250)  NULL,
    Category       varchar(150)  NULL,
    Amount         decimal(18,2) NULL,
    Attachment     varchar(450)  NULL,
    fileextension  varchar(50)   NULL,
    Note           varchar(450)  NULL,
    Createdby      varchar(100)  NULL,
    CONSTRAINT PK_tbl_expense PRIMARY KEY CLUSTERED (ID)
);
GO

/* =====================================================================
   7.  INDEXES on the columns the application joins / filters on
   ===================================================================== */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sales_item_sales_id')
    CREATE INDEX IX_sales_item_sales_id        ON dbo.sales_item (sales_id);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sales_item_sales_time')
    CREATE INDEX IX_sales_item_sales_time      ON dbo.sales_item (sales_time);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sales_item_itemcode')
    CREATE INDEX IX_sales_item_itemcode        ON dbo.sales_item (itemcode);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sales_payment_sales_time')
    CREATE INDEX IX_sales_payment_sales_time   ON dbo.sales_payment (sales_time);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sales_payment_c_id')
    CREATE INDEX IX_sales_payment_c_id         ON dbo.sales_payment (c_id);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sales_payment_Shopid')
    CREATE INDEX IX_sales_payment_Shopid       ON dbo.sales_payment (Shopid);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_return_item_SoldInvoiceNo')
    CREATE INDEX IX_return_item_SoldInvoiceNo  ON dbo.return_item (SoldInvoiceNo);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tbl_duepayment_sales_id')
    CREATE INDEX IX_tbl_duepayment_sales_id    ON dbo.tbl_duepayment (sales_id);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tbl_CustCredit_CustID')
    CREATE INDEX IX_tbl_CustCredit_CustID      ON dbo.tbl_CustCredit (CustID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_purchase_product_name')
    CREATE INDEX IX_purchase_product_name      ON dbo.purchase (product_name);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tbl_workrecords_Username_logdate')
    CREATE INDEX IX_tbl_workrecords_Username_logdate ON dbo.tbl_workrecords (Username, logdate);
GO

/* =====================================================================
   8.  VIEWS  (same names the application queries)
   ===================================================================== */
IF OBJECT_ID(N'dbo.vw_workrecords', N'V') IS NOT NULL DROP VIEW dbo.vw_workrecords;
GO
CREATE VIEW dbo.vw_workrecords
AS
    SELECT  username,
            logdate                                         AS [Date],
            MIN(logtime)                                    AS [IN],
            MAX(logtime)                                    AS [OUT],
            CONVERT(varchar(8), DATEADD(MS, DATEDIFF(MS, MIN(logtime), MAX(logtime)), 0), 114) AS [HOURS]
    FROM    dbo.tbl_workrecords
    GROUP BY username, logdate;
GO

IF OBJECT_ID(N'dbo.vw_itemdisplay_sr', N'V') IS NOT NULL DROP VIEW dbo.vw_itemdisplay_sr;
GO
CREATE VIEW dbo.vw_itemdisplay_sr
AS
    SELECT TOP 12 * FROM dbo.purchase ORDER BY NEWID();
GO

IF OBJECT_ID(N'dbo.vw_General_Ledger', N'V') IS NOT NULL DROP VIEW dbo.vw_General_Ledger;
GO
CREATE VIEW dbo.vw_General_Ledger
AS
    SELECT  sp.sales_time                                                   AS [Date],
            SUM(sp.payment_amount)                                          AS Sales,
            ISNULL((SUM(ri.Total) - SUM(ri.disamt)) + SUM(ri.vatamt), 0)    AS [Return]
    FROM    dbo.sales_payment AS sp
            LEFT OUTER JOIN dbo.return_item AS ri ON sp.sales_id = ri.SoldInvoiceNo
    GROUP BY sp.sales_time;
GO

IF OBJECT_ID(N'dbo.vw_CustCreditReport', N'V') IS NOT NULL DROP VIEW dbo.vw_CustCreditReport;
GO
CREATE VIEW dbo.vw_CustCreditReport
AS
    SELECT  cc.ID           AS TrxID,
            cc.[Date],
            Customers.ID    AS CustID,
            Customers.Name,
            cc.OrderID,
            cc.Credit,
            cc.Description
    FROM    dbo.tbl_CustCredit AS cc
            LEFT OUTER JOIN dbo.tbl_customer AS Customers ON cc.CustID = Customers.ID;
GO

IF OBJECT_ID(N'dbo.CustomerCredit', N'V') IS NOT NULL DROP VIEW dbo.CustomerCredit;
GO
CREATE VIEW dbo.CustomerCredit
AS
    SELECT  Customers.ID, Customers.Name, Customers.Phone AS Mobile, Customers.Address,
            Customers.EmailAddress, Customers.City, Customers.PeopleType
    FROM    dbo.tbl_customer AS Customers
            LEFT JOIN dbo.tbl_CustCredit AS cc ON cc.CustID = Customers.ID
    GROUP BY Customers.ID, Customers.Name, Customers.Phone, Customers.Address,
             Customers.EmailAddress, Customers.City, Customers.PeopleType;
GO

/* =====================================================================
   9.  MINIMUM SEED DATA  (only inserted when the table is empty)
   ===================================================================== */
IF NOT EXISTS (SELECT 1 FROM dbo.usermgt)
    INSERT dbo.usermgt (Name, Father_name, Address, Email, Contact, DOB, Username, password, usertype, position, imagename, Shopid, shopname)
    VALUES ('Administrator', '', '', '', '', '', 'admin', 'admin', '1', 'Admin', '1.jpg', 'MTQC02', 'My Shop');

IF NOT EXISTS (SELECT 1 FROM dbo.storeconfig)
    INSERT dbo.storeconfig (companyname, companyaddress, companyphone, vatno, web, vatrate, disrate, footermsg)
    VALUES ('My Shop', 'Shop address', '+92-000-0000000', 'VAT-0000', 'www.example.com', 0.000, 0.000, 'Thanks for your shopping.');

IF NOT EXISTS (SELECT 1 FROM dbo.tbl_terminalLocation)
    INSERT dbo.tbl_terminalLocation (CompanyName, Branchname, Location, Phone, Email, Web, VAT, Dis, VATRegiNo, Shopid, Footermsg)
    VALUES ('My Shop', 'Main Branch', 'Shop address', '+92-000-0000000', '', '', 0.000, 0.000, 'VAT-0000', 'MTQC02', 'Thanks for your shopping.');

IF NOT EXISTS (SELECT 1 FROM dbo.trincro)
    INSERT dbo.trincro (trno) VALUES (0);

/* Walk-in customer: the sales screens use customer id 10000009 by default */
IF NOT EXISTS (SELECT 1 FROM dbo.tbl_customer WHERE ID = 10000009)
BEGIN
    SET IDENTITY_INSERT dbo.tbl_customer ON;
    INSERT dbo.tbl_customer (ID, Name, EmailAddress, Phone, Address, City, PeopleType)
    VALUES (10000009, 'Walk-in Customer', '', '', '', '', 'Customer');
    SET IDENTITY_INSERT dbo.tbl_customer OFF;
END

IF NOT EXISTS (SELECT 1 FROM dbo.tbl_category)
    INSERT dbo.tbl_category (category_name)
    VALUES ('Food'), ('Drink'), ('Electronic'), ('Vegetable'), ('Fruit'), ('Others');
GO

PRINT 'APOSDB schema ready.';
GO

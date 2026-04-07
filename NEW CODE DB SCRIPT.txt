SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[usermgt](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Father_name] [varchar](100) NULL,
	[Address] [varchar](220) NULL,
	[Email] [varchar](100) NULL,
	[Contact] [varchar](100) NULL,
	[DOB] [varchar](100) NULL,
	[Username] [varchar](100) NULL,
	[password] [varchar](100) NULL,
	[usertype] [varchar](100) NULL,
	[position] [varchar](100) NULL,
	[imagename] [varchar](100) NULL,
	[Shopid] [varchar](100) NULL,
	[logdate] [datetime] NULL,
                   [basic_salary] [varchar](50),
 	[joning_date] [varchar](50),
	[in_time] [varchar](50),
 	[out_time] [varchar](50),
 	[shopname] [varchar](50)
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[usermgt] ON
INSERT [dbo].[usermgt] ([id], [Name], [Father_name], [Address], [Email], [Contact], [DOB], [Username], [password], [usertype], [position], [imagename], [Shopid], [logdate]) VALUES (1, N'admin', N'admin', N'Dhaka', N'dhaka', N'389238', N'1990-09-08', N'admin', N'admin', N'1', N'Admin', N'1.jpg', N'MTQC02', CAST(0x0000A4A800E5B2A5 AS DateTime))
INSERT [dbo].[usermgt] ([id], [Name], [Father_name], [Address], [Email], [Contact], [DOB], [Username], [password], [usertype], [position], [imagename], [Shopid], [logdate]) VALUES (2, N'mn', N'mn', N'nm', N'mn', N'yyu', N'1/16/1992', N'mn', N'mn', N'2', N'Manager', N'2.jpg', N'MTQC02', CAST(0x0000A4A8012A1FB1 AS DateTime))
INSERT [dbo].[usermgt] ([id], [Name], [Father_name], [Address], [Email], [Contact], [DOB], [Username], [password], [usertype], [position], [imagename], [Shopid], [logdate]) VALUES (3, N'salesman', N'sa', N'sa', N'sa', N'9089808', N'1992-01-16', N'sa', N'sa', N'3', N'Salesman', N'3.jpg', N'WION05', CAST(0x0000A4A8012AA71B AS DateTime))
INSERT [dbo].[usermgt] ([id], [Name], [Father_name], [Address], [Email], [Contact], [DOB], [Username], [password], [usertype], [position], [imagename], [Shopid], [logdate]) VALUES (4, N'salesman', N'sa', N'sa', N'sa', N'9089808', N'1/16/1992', N'sales', N'sa', N'3', N'Salesman', N'4.jpg', N'WION05', CAST(0x0000A4A8012AB395 AS DateTime))
INSERT [dbo].[usermgt] ([id], [Name], [Father_name], [Address], [Email], [Contact], [DOB], [Username], [password], [usertype], [position], [imagename], [Shopid], [logdate]) VALUES (7, N'ert', N'et', N'et', N'werwer', N'243', N'1992-01-16', N'wrewer', N'Pnyk0LEgAC', N'2', N'Manager', N'7.jpg', N'HamRT5', CAST(0x0000A52A00FC53FA AS DateTime))
INSERT [dbo].[usermgt] ([id], [Name], [Father_name], [Address], [Email], [Contact], [DOB], [Username], [password], [usertype], [position], [imagename], [Shopid], [logdate]) VALUES (5, N'a', N'a', N'a', N'a', N'34', N'1992-01-16', N'a', N'a', N'1', N'Admin', N'5.jpg', N'MTQC02', CAST(0x0000A4A8012B5E4D AS DateTime))
INSERT [dbo].[usermgt] ([id], [Name], [Father_name], [Address], [Email], [Contact], [DOB], [Username], [password], [usertype], [position], [imagename], [Shopid], [logdate]) VALUES (6, N'ff', N'df', N'df', N'ff', N'dfdf', N'1/16/1992', N'ff', N'ff', N'1', N'Admin', N'6.jpg', N'WION05', CAST(0x0000A4A8012BFFFF AS DateTime))
SET IDENTITY_INSERT [dbo].[usermgt] OFF


/****** Object:  Table [dbo].[userattendence]    Script Date: 11-Mar-21 8:46:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[userattendence](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[intime] [varchar](50) NULL,
	[outtime] [varchar](50) NULL,
	[att_date] [varchar](50) NOT NULL,
	[userid] [bigint] NULL,
	[att_status] [varchar](50) NULL,
	[att_month] [varchar](50) NULL,
	[att_year] [varchar](50) NULL,
 CONSTRAINT [PK_userattendence_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


/****** Object:  Table [dbo].[tbl_payroll]    Script Date: 11-Mar-21 8:48:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbl_payroll](
	[user_name] [varchar](50) NOT NULL,
	[pay_month] [varchar](50) NOT NULL,
	[pay_year] [varchar](50) NOT NULL,
	[pay_date] [varchar](50) NULL,
	[basic_pay] [varchar](50) NULL,
	[bouns] [varchar](50) NULL,
	[total_salary] [varchar](50) NULL,
	[bal_amount] [varchar](50) NULL,
	[leaves] [varchar](50) NULL,
	[deducations] [varchar](50) NULL,
	[net_amount] [varchar](50) NULL,
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[pay_status] [varchar](50) NULL,
	[paid_amount] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_payroll_1] PRIMARY KEY CLUSTERED 
(
	[user_name] ASC,
	[pay_month] ASC,
	[pay_year] ASC,
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


/****** Object:  Table [dbo].[tbl_adv_sal]    Script Date: 11-Mar-21 8:48:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbl_adv_sal](
	[user_name] [varchar](50) NULL,
	[adv_month] [varchar](50) NULL,
	[adv_year] [varchar](50) NULL,
	[adv_date] [varchar](50) NULL,
	[adv_amount] [float] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[trincro]    Script Date: 10/05/2017 20:27:57 ******/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trincro](
	[trno] [bigint] NOT NULL
) ON [PRIMARY]
GO
INSERT [dbo].[trincro] ([trno]) VALUES (3)
/****** Object:  Table [dbo].[test]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[test](
	[id] [int] NOT NULL,
	[testtb] [varchar](150) NULL,
	[testtb2] [varchar](150) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[test] ([id], [testtb], [testtb2]) VALUES (1, N'990877', N'2')
/****** Object:  Table [dbo].[tbl_workrecords]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_workrecords](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](150) NULL,
	[datatype] [varchar](50) NULL,
	[logdate] [smalldatetime] NULL,
	[logtime] [smalldatetime] NULL,
	[logdatetime] [smalldatetime] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_tbl_workrecords] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[tbl_terminalLocation]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_terminalLocation](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [varchar](250) NULL,
	[Branchname] [varchar](150) NULL,
	[Location] [varchar](430) NULL,
	[Phone] [varchar](50) NULL,
	[Email] [varchar](150) NULL,
	[Web] [varchar](150) NULL,
	[VAT] [decimal](18, 3) NULL,
	[Dis] [decimal](18, 3) NULL,
	[VATRegiNo] [varchar](150) NULL,
	[Shopid] [varchar](110) NULL,
	[Footermsg] [varchar](450) NULL,
 CONSTRAINT [PK__tbl_term__3214EC2732E0915F] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_terminalLocation] ON
INSERT [dbo].[tbl_terminalLocation] ([ID], [CompanyName], [Branchname], [Location], [Phone], [Email], [Web], [VAT], [Dis], [VATRegiNo], [Shopid], [Footermsg]) VALUES (1, N'Marche Amar', N'Montreal', N'3536 Rue Milton, Montreal QC H2X 2G8', N'514 777 3456', N'MTQC02@citputer.com', N'www.citputer.com', CAST(14.975 AS Decimal(18, 3)), CAST(3.000 AS Decimal(18, 3)), N'VAT982', N'MTQC02', N'Item sold will not be refund only EXCHANGE will be executed with in 5 days.')
INSERT [dbo].[tbl_terminalLocation] ([ID], [CompanyName], [Branchname], [Location], [Phone], [Email], [Web], [VAT], [Dis], [VATRegiNo], [Shopid], [Footermsg]) VALUES (2, N'Marche Amar', N'Windsor', N'James Dr ,ON N9E 24E Canada', N'519 273 3427', N'WION05@citputer.com', N'www.citputer.com', CAST(13.000 AS Decimal(18, 3)), CAST(2.000 AS Decimal(18, 3)), N'WIDGST300', N'WION05', N'Item sold will not be refund only EXCHANGE will be executed with in 15 days.')
INSERT [dbo].[tbl_terminalLocation] ([ID], [CompanyName], [Branchname], [Location], [Phone], [Email], [Web], [VAT], [Dis], [VATRegiNo], [Shopid], [Footermsg]) VALUES (3, N'Marche Amar', N'London', N'Rue losiuo', N'518 784 3423', N'', N'', CAST(13.000 AS Decimal(18, 3)), CAST(1.000 AS Decimal(18, 3)), N'VATRE09', N'Lo13', N'Item sold will not be refund only EXCHANGE will be executed with in 15 days.')
INSERT [dbo].[tbl_terminalLocation] ([ID], [CompanyName], [Branchname], [Location], [Phone], [Email], [Web], [VAT], [Dis], [VATRegiNo], [Shopid], [Footermsg]) VALUES (4, N'Marche Amar', N'Hamilton', N'3490 Jenshon Street , Hamilton ON N4H 9D2 , Canada', N'517 324 4532', N'Hamrt5@citputer.com', N'www.citputer.com', CAST(13.000 AS Decimal(18, 3)), CAST(2.000 AS Decimal(18, 3)), N'RT566', N'HamRT5', N'Your Footer message it will appear on Receipt.')
INSERT [dbo].[tbl_terminalLocation] ([ID], [CompanyName], [Branchname], [Location], [Phone], [Email], [Web], [VAT], [Dis], [VATRegiNo], [Shopid], [Footermsg]) VALUES (5, N'Marche Amar', N'British Columbia BC1', N'British Columbia B2D 2S1', N'543 345 4553', N'bc1@citputer.com', N'www.citputer.com', CAST(12.000 AS Decimal(18, 3)), CAST(1.000 AS Decimal(18, 3)), N'BCVAT342', N'BC3BCV', N'Your Footer message it will appear on Receipt.')
INSERT [dbo].[tbl_terminalLocation] ([ID], [CompanyName], [Branchname], [Location], [Phone], [Email], [Web], [VAT], [Dis], [VATRegiNo], [Shopid], [Footermsg]) VALUES (6, N'My Shop(your Shop name) SQL', N'Alberta Branch1', N'Alberta', N'543 345 4553', N'AL@citputer.com', N'www.citputer.com', CAST(5.000 AS Decimal(18, 3)), CAST(1.000 AS Decimal(18, 3)), N'VAT342AL', N'AlbVAT', N'Your Footer message it will appear on Receipt.')
INSERT [dbo].[tbl_terminalLocation] ([ID], [CompanyName], [Branchname], [Location], [Phone], [Email], [Web], [VAT], [Dis], [VATRegiNo], [Shopid], [Footermsg]) VALUES (7, N'My Shop(your Shop name) SQL', N'SASkatchewan B1', N'Saskatchewan , Canada', N'543 345 4553', N'SA@citputer.com', N'www.citputer.com', CAST(10.000 AS Decimal(18, 3)), CAST(1.000 AS Decimal(18, 3)), N'VAT4542SA', N'SASVAT', N'Your Footer message it will appear on Receipt.')
SET IDENTITY_INSERT [dbo].[tbl_terminalLocation] OFF
/****** Object:  Table [dbo].[tbl_saleInfo]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_saleInfo](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[InvoiceNo] [varchar](250) NULL,
	[WarehouseNo] [varchar](250) NULL,
	[Biller] [varchar](250) NULL,
	[Customer] [varchar](250) NULL,
	[Note] [varchar](250) NULL,
	[DisRate] [decimal](18, 2) NULL,
	[TaxRate] [decimal](18, 2) NULL,
	[ShippingFee] [decimal](18, 2) NULL,
	[SoldBy] [varchar](250) NULL,
	[DateTime] [smalldatetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_saleInfo] ON
INSERT [dbo].[tbl_saleInfo] ([ID], [InvoiceNo], [WarehouseNo], [Biller], [Customer], [Note], [DisRate], [TaxRate], [ShippingFee], [SoldBy], [DateTime]) VALUES (1, N'6', N'Warehouse-1', N'Jeo Cavanni', N'10000009', N'10000009', CAST(0.00 AS Decimal(18, 2)), CAST(14.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'a', CAST(0xF43C0B00 AS Date))
SET IDENTITY_INSERT [dbo].[tbl_saleInfo] OFF
/****** Object:  Table [dbo].[tbl_purchase_history]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_purchase_history](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[product_id] [varchar](50) NULL,
	[product_name] [nvarchar](150) NULL,
	[product_quantity] [decimal](18, 2) NULL,
	[cost_price] [decimal](18, 2) NULL,
	[retail_price] [decimal](18, 2) NULL,
	[category] [varchar](50) NULL,
	[supplier] [varchar](50) NULL,
	[purchase_date] [varchar](50) NULL,
	[Shopid] [varchar](50) NULL,
	[ptype] [varchar](50) NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_tbl_purchase_history] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_purchase_history] ON
INSERT [dbo].[tbl_purchase_history] ([id], [product_id], [product_name], [product_quantity], [cost_price], [retail_price], [category], [supplier], [purchase_date], [Shopid], [ptype], [status]) VALUES (1, N'897463585492', N'Coffe_medium', CAST(78.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.49 AS Decimal(18, 2)), N'Coffe', N'Mcdonalds', N'2017-10-05', N'MTQC02', N'OLD', 1)
INSERT [dbo].[tbl_purchase_history] ([id], [product_id], [product_name], [product_quantity], [cost_price], [retail_price], [category], [supplier], [purchase_date], [Shopid], [ptype], [status]) VALUES (2, N'2309402', N'tee asdas', CAST(3.00 AS Decimal(18, 2)), CAST(4.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), N'Burger', N'Square', N'2017-10-05', N'MTQC02', N'NEW', 2)
INSERT [dbo].[tbl_purchase_history] ([id], [product_id], [product_name], [product_quantity], [cost_price], [retail_price], [category], [supplier], [purchase_date], [Shopid], [ptype], [status]) VALUES (3, N'2309402', N'tee asdas', CAST(43.00 AS Decimal(18, 2)), CAST(4.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), N'Burger', N'Square', N'2017-10-05', N'MTQC02', N'OLD', 1)
INSERT [dbo].[tbl_purchase_history] ([id], [product_id], [product_name], [product_quantity], [cost_price], [retail_price], [category], [supplier], [purchase_date], [Shopid], [ptype], [status]) VALUES (4, N'894500000966', N'Pano_water500ml', CAST(6.50 AS Decimal(18, 2)), CAST(90.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), N'Water', N'AmericanBlend', N'2017-10-05', N'MTQC02', N'NEW', 1)
INSERT [dbo].[tbl_purchase_history] ([id], [product_id], [product_name], [product_quantity], [cost_price], [retail_price], [category], [supplier], [purchase_date], [Shopid], [ptype], [status]) VALUES (5, N'894500000967', N'Cola370ml', CAST(6.50 AS Decimal(18, 2)), CAST(4.90 AS Decimal(18, 2)), CAST(5.49 AS Decimal(18, 2)), N'Drinks', N'Cocacola', N'2017-10-05', N'MTQC02', N'NEW', 1)
SET IDENTITY_INSERT [dbo].[tbl_purchase_history] OFF
/****** Object:  Table [dbo].[tbl_expense]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_expense](
	[ID] [bigint] IDENTITY(901,1) NOT NULL,
	[Date] [smalldatetime] NULL,
	[ReferenceNo] [varchar](250) NULL,
	[Category] [varchar](150) NULL,
	[Amount] [decimal](18, 2) NULL,
	[Attachment] [varchar](450) NULL,
	[fileextension] [varchar](50) NULL,
	[Note] [varchar](450) NULL,
	[Createdby] [varchar](150) NULL,
 CONSTRAINT [PK_tbl_expense] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_expense] ON
INSERT [dbo].[tbl_expense] ([ID], [Date], [ReferenceNo], [Category], [Amount], [Attachment], [fileextension], [Note], [Createdby]) VALUES (905, N'2017-02-02', N'INV12368', N'Buy Accessories', CAST(11.00 AS Decimal(18, 2)), N'20170202025750.png', N'.png', N'ghjhyu', N'a')
INSERT [dbo].[tbl_expense] ([ID], [Date], [ReferenceNo], [Category], [Amount], [Attachment], [fileextension], [Note], [Createdby]) VALUES (906, N'2017-02-02', N'SALS321', N'POS paper Roll', CAST(9.99 AS Decimal(18, 2)), N'20170202025842.png', N'.png', N'Find Receipt', N'a')
INSERT [dbo].[tbl_expense] ([ID], [Date], [ReferenceNo], [Category], [Amount], [Attachment], [fileextension], [Note], [Createdby]) VALUES (907, N'2017-02-11', N'INV7', N'Buy Accessories', CAST(23.00 AS Decimal(18, 2)), N'20170211123415.pdf', N'.pdf', N'Invoice file', N'a')
INSERT [dbo].[tbl_expense] ([ID], [Date], [ReferenceNo], [Category], [Amount], [Attachment], [fileextension], [Note], [Createdby]) VALUES (909, N'2017-05-27', N'FDDS12', N'Buy Accessories', CAST(2.00 AS Decimal(18, 2)), N'', N'', N'asa', N'a')
SET IDENTITY_INSERT [dbo].[tbl_expense] OFF
/****** Object:  Table [dbo].[tbl_duepayment]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_duepayment](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[receivedate] [varchar](50) NULL,
	[sales_id] [bigint] NULL,
	[totalamt] [decimal](18, 2) NULL,
	[dueamt] [decimal](18, 2) NULL,
	[receiveamt] [decimal](18, 2) NULL,
	[custid] [varchar](50) NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_tbl_duepayment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_duepayment] ON
INSERT [dbo].[tbl_duepayment] ([id], [receivedate], [sales_id], [totalamt], [dueamt], [receiveamt], [custid], [status]) VALUES (1, N'2017-06-19', 4, CAST(17.94 AS Decimal(18, 2)), CAST(6.94 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), N'10000009', 1)
INSERT [dbo].[tbl_duepayment] ([id], [receivedate], [sales_id], [totalamt], [dueamt], [receiveamt], [custid], [status]) VALUES (2, N'2017-06-20', 4, CAST(17.94 AS Decimal(18, 2)), CAST(4.94 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), N'10000009', 1)
INSERT [dbo].[tbl_duepayment] ([id], [receivedate], [sales_id], [totalamt], [dueamt], [receiveamt], [custid], [status]) VALUES (3, N'2017-06-19', 4, CAST(17.94 AS Decimal(18, 2)), CAST(3.94 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), N'10000009', 1)
INSERT [dbo].[tbl_duepayment] ([id], [receivedate], [sales_id], [totalamt], [dueamt], [receiveamt], [custid], [status]) VALUES (4, N'2017-06-19', 4, CAST(17.94 AS Decimal(18, 2)), CAST(1.50 AS Decimal(18, 2)), CAST(2.44 AS Decimal(18, 2)), N'10000009', 1)
SET IDENTITY_INSERT [dbo].[tbl_duepayment] OFF
/****** Object:  Table [dbo].[tbl_customer]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_customer](
	[ID] [bigint] IDENTITY(10000001,1) NOT NULL,
	[Name] [varchar](250) NULL,
	[EmailAddress] [varchar](250) NULL,
	[Phone] [varchar](50) NULL,
	[Address] [varchar](250) NULL,
	[City] [varchar](50) NULL,
	[PeopleType] [varchar](50) NULL,
	[Logtime] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_customer] ON
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (32131, N'123', N'', N'123', N'123', N'123', N'Biller', CAST(0x0000A528012083BD AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (10000001, N'Jamal', N'Jamal@gmail.com', N'49583958', N'Uttara,Dhaka', N'Dhaka', N'Customer', CAST(0x0000A52700E6A8CD AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (10000002, N'Kamal', N'Kamal@gmail.com', N'384097908', N'Uttara,Dhaka', N'Dhaka', N'Customer', CAST(0x0000A52700E6A8D7 AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (10000003, N'Mohammad Farhad', N'Farhad@citputer.com', N'6.61717E+13', N'Malaysia', N'Kuala Lampur', N'Customer', CAST(0x0000A52700E6A8E0 AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (10000004, N'A H Halim', N'Farhad@citputer.com', N'6.61717E+13', N'Bukit hill , KL,Malaysia.', N'Kuala Lampur', N'Customer', CAST(0x0000A52700E6A8FC AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (10000005, N'Jeo Cavanni', N'Jeo@alo.com', N'1438758349', N'South Carelina [SC], USA', N'SC', N'Biller', CAST(0x0000A52700E6A906 AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (10000006, N'G M Gulam', N'Gulam@citputer.com', N'6.61717E+12', N'Bukit hill , KL,Malaysia', N'Kuala Lampur', N'Biller', CAST(0x0000A52700E6A90A AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (10000008, N'Nusrul Hakim', N'Hakim@gmail.com', N'346868', N'KL,MY', N'Kuala Lampur', N'Biller', CAST(0x0000A52700E6A914 AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (10000009, N'Guest', N'Guest', N'294', N'Uttara,Mohakhali', N'Dhaka', N'Customer', CAST(0x0000A52700E6A91D AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (10000010, N'So Xa Huang.', N'Huang@gmail.com', N'90283498', N'Japan', N'Yamaha', N'Customer', CAST(0x0000A52700E6A927 AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (10000011, N'Jinat', N'jinat@Khan.com', N'898666986', N'Windsor,Canada', N'Windsor', N'Customer', CAST(0x0000A52700E6A92B AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (10000012, N'SQLTest', N'SQLTest@sda.com', N'984534', N'Seul, S.Korea', N'Suel', N'Customer', CAST(0x0000A527010BBF23 AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (10000014, N'Uniliver', N'sdsd', N'7878', N'hh', N'ytyu', N'Supplier', CAST(0x0000A528011FA9A2 AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (123012038, N'SQLtest3', N'43', N'34', N'43', N'34', N'Biller', CAST(0x0000A52A00F84EDD AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (123012039, N'Square', N'88yuy', N'908789', N'ON,NY', N'Dhaka', N'Supplier', CAST(0x0000A5500129F466 AS DateTime))
INSERT [dbo].[tbl_customer] ([ID], [Name], [EmailAddress], [Phone], [Address], [City], [PeopleType], [Logtime]) VALUES (123012040, N'McDonuls', N'', N'324r', N'ny', N'ny', N'Supplier', CAST(0x0000A71700E12B7E AS DateTime))
SET IDENTITY_INSERT [dbo].[tbl_customer] OFF
/****** Object:  Table [dbo].[tbl_CustCredit]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_CustCredit](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CustID] [bigint] NOT NULL,
	[OrderID] [varchar](250) NULL,
	[Date] [varchar](150) NULL,
	[Credit] [decimal](18, 2) NULL,
	[Description] [varchar](250) NULL,
	[Logtime] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_CustCredit] ON
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (1, 10000001, N'10121', N'2015-10-02', CAST(90.00 AS Decimal(18, 2)), N'Add rewards', CAST(0x0000A52700E87776 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (2, 10000001, N'10122', N'2015-10-03', CAST(-10.00 AS Decimal(18, 2)), N'Subtract rewards order expired', CAST(0x0000A52700E8778D AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (3, 10000001, N'SyS', N'2015-10-03', CAST(30.00 AS Decimal(18, 2)), N'Rewards by system admin', CAST(0x0000A52700E87796 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (4, 10000002, N'SyS', N'2015-10-03', CAST(10.00 AS Decimal(18, 2)), N'SysAdmin', CAST(0x0000A52700E877A0 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (5, 10000004, N'SyS', N'2015-10-03', CAST(10.00 AS Decimal(18, 2)), N'Add r', CAST(0x0000A52700E877A9 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (6, 10000006, N'SyS', N'2015-10-03', CAST(10.00 AS Decimal(18, 2)), N'Initial Point', CAST(0x0000A52700E877AE AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (7, 10000002, N'57', N'2015-10-03', CAST(5.04 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A52700E877B7 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (8, 10000009, N'58', N'2015-10-03', CAST(1.55 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A52700E877C1 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (9, 10000009, N'59', N'2015-10-03', CAST(14.46 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A52700E877CA AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (10, 10000003, N'60', N'2015-10-03', CAST(3.49 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A52700E877D3 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (11, 10000010, N'61', N'2015-10-03', CAST(5.49 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A52700E877DD AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (12, 10000010, N'61', N'2015-10-03', CAST(-6.60 AS Decimal(18, 2)), N'Subtract rewards order Return', CAST(0x0000A52700E877E1 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (13, 10000011, N'SyS', N'2015-10-04', CAST(10.00 AS Decimal(18, 2)), N'Bonus Credit from Store', CAST(0x0000A52700E877EB AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (14, 10000011, N'62', N'2015-10-04', CAST(17.01 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A52700E877F9 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (15, 10000012, N'SyS', N'2015-10-04', CAST(20.00 AS Decimal(18, 2)), N'Test Bonus', CAST(0x0000A527010C01BA AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (16, 10000012, N'13', N'2015-10-04', CAST(2.44 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A527010CA53D AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (17, 10000012, N'14', N'2015-10-04', CAST(2.44 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A527010CED0D AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (18, 10000012, N'14', N'2015-10-04', CAST(-2.94 AS Decimal(18, 2)), N'Subtract rewards order Return', CAST(0x0000A5270113E898 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (19, 10000002, N'15', N'2015-10-05', CAST(2.44 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A528011BF90A AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (20, 10000002, N'15', N'2015-10-05', CAST(-2.94 AS Decimal(18, 2)), N'Subtract rewards order Return', CAST(0x0000A528011C7B9D AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (21, 10000002, N'16', N'2015-10-05', CAST(2.44 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A5280121B0E1 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (22, 10000002, N'SyS', N'2015-10-05', CAST(1.00 AS Decimal(18, 2)), N'Bonus', CAST(0x0000A5280121F7C1 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (23, 10000011, N'17', N'2015-10-07', CAST(2.44 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A52A00F86C45 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (24, 10000011, N'17', N'2015-10-07', CAST(-2.94 AS Decimal(18, 2)), N'Subtract rewards order Return', CAST(0x0000A52A00F8E18C AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (25, 10000010, N'18', N'2015-10-07', CAST(1.50 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A52A00F914B9 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (26, 10000009, N'19', N'2015-10-07', CAST(4.39 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A52A00FAC552 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (27, 10000009, N'20', N'2015-11-14', CAST(5.94 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A55001274473 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (28, 10000009, N'21', N'2015-11-14', CAST(1.50 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A55001276BC9 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (29, 10000009, N'23', N'2016-09-18', CAST(3.76 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A685013A0534 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (30, 10000009, N'27', N'2016-09-21', CAST(5.70 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A6880155E85A AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (31, 10000009, N'31', N'2017-01-07', CAST(5.75 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A6F4000126F2 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (32, 10000009, N'33', N'2017-01-08', CAST(0.03 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A6F5011DA9A6 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (33, 10000009, N'35', N'2017-03-15', CAST(14.05 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A737000CD2C8 AS DateTime))
INSERT [dbo].[tbl_CustCredit] ([ID], [CustID], [OrderID], [Date], [Credit], [Description], [Logtime]) VALUES (34, 10000009, N'38', N'2017-03-25', CAST(6.86 AS Decimal(18, 2)), N'Add Rewards', CAST(0x0000A741012A155A AS DateTime))
SET IDENTITY_INSERT [dbo].[tbl_CustCredit] OFF
/****** Object:  Table [dbo].[tbl_category]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_category](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[category_name] [varchar](250) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_category] ON
INSERT [dbo].[tbl_category] ([ID], [category_name]) VALUES (1, N'Food')
INSERT [dbo].[tbl_category] ([ID], [category_name]) VALUES (2, N'Cigarettes')
INSERT [dbo].[tbl_category] ([ID], [category_name]) VALUES (3, N'Drink')
INSERT [dbo].[tbl_category] ([ID], [category_name]) VALUES (4, N'Electronic')
INSERT [dbo].[tbl_category] ([ID], [category_name]) VALUES (5, N'Milk')
INSERT [dbo].[tbl_category] ([ID], [category_name]) VALUES (6, N'Vegetable')
INSERT [dbo].[tbl_category] ([ID], [category_name]) VALUES (7, N'Burger')
INSERT [dbo].[tbl_category] ([ID], [category_name]) VALUES (8, N'soft_drink')
INSERT [dbo].[tbl_category] ([ID], [category_name]) VALUES (9, N'Fruit')
INSERT [dbo].[tbl_category] ([ID], [category_name]) VALUES (10, N'CoolDrink')
INSERT [dbo].[tbl_category] ([ID], [category_name]) VALUES (11, N'Others')
INSERT [dbo].[tbl_category] ([ID], [category_name]) VALUES (12, N'Coffe')
SET IDENTITY_INSERT [dbo].[tbl_category] OFF
/****** Object:  Table [dbo].[storeconfig]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[storeconfig](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[companyname] [varchar](250) NULL,
	[companyaddress] [varchar](250) NULL,
	[companyphone] [varchar](250) NULL,
	[vatno] [varchar](250) NULL,
	[web] [varchar](250) NULL,
	[vatrate] [decimal](18, 3) NULL,
	[disrate] [decimal](18, 3) NULL,
	[footermsg] [varchar](450) NULL,
	[updatetime] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[storeconfig] ON
INSERT [dbo].[storeconfig] ([id], [companyname], [companyaddress], [companyphone], [vatno], [web], [vatrate], [disrate], [footermsg], [updatetime]) VALUES (1, N'My Shop(your Shop name) SQL', N'MS SQL server Edition. Uttara, Dhaka.', N'+8801727535397', N'T345', N'www.citputer.com', CAST(13.975 AS Decimal(18, 3)), CAST(0.000 AS Decimal(18, 3)), N'Thanks for your shopping.', CAST(0x0000A4A800E6BE85 AS DateTime))
SET IDENTITY_INSERT [dbo].[storeconfig] OFF
/****** Object:  Table [dbo].[sales_payment]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sales_payment](
	[sales_id] [bigint] NOT NULL,
	[payment_type] [varchar](150) NULL,
	[payment_amount] [decimal](18, 2) NULL,
	[change_amount] [decimal](18, 2) NULL,
	[due_amount] [decimal](18, 2) NULL,
	[dis] [decimal](18, 2) NULL,
	[vat] [decimal](18, 2) NULL,
	[sales_time] [varchar](150) NULL,
	[c_id] [varchar](150) NULL,
	[emp_id] [varchar](150) NULL,
	[comment] [nvarchar](350) NULL,
	[TrxType] [varchar](50) NULL,
	[Shopid] [varchar](100) NULL,
	[ovdisrate] [decimal](18, 2) NULL,
	[vaterate] [decimal](18, 2) NULL,
	[logdate] [datetime] NULL,
 CONSTRAINT [PK_sales_payment] PRIMARY KEY CLUSTERED 
(
	[sales_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[sales_payment] ([sales_id], [payment_type], [payment_amount], [change_amount], [due_amount], [dis], [vat], [sales_time], [c_id], [emp_id], [comment], [TrxType], [Shopid], [ovdisrate], [vaterate], [logdate]) VALUES (1, N'Cash', CAST(11.82 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.20 AS Decimal(18, 2)), CAST(0.53 AS Decimal(18, 2)), N'2017-06-19', N'10000009', N'a', N'Guest  ', N'POS', N'MTQC02', CAST(0.00 AS Decimal(18, 2)), CAST(14.00 AS Decimal(18, 2)), CAST(0x0000A7990121C8A9 AS DateTime))
INSERT [dbo].[sales_payment] ([sales_id], [payment_type], [payment_amount], [change_amount], [due_amount], [dis], [vat], [sales_time], [c_id], [emp_id], [comment], [TrxType], [Shopid], [ovdisrate], [vaterate], [logdate]) VALUES (2, N'Cash', CAST(10.24 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1.26 AS Decimal(18, 2)), N'2017-06-19', N'10000010', N'a', N'So Xa Huang  ', N'POS', N'MTQC02', CAST(0.00 AS Decimal(18, 2)), CAST(14.00 AS Decimal(18, 2)), CAST(0x0000A7990121C8CE AS DateTime))
INSERT [dbo].[sales_payment] ([sales_id], [payment_type], [payment_amount], [change_amount], [due_amount], [dis], [vat], [sales_time], [c_id], [emp_id], [comment], [TrxType], [Shopid], [ovdisrate], [vaterate], [logdate]) VALUES (3, N'Cash', CAST(6.54 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.56 AS Decimal(18, 2)), N'2017-06-19', N'10000009', N'a', N'Guest  ', N'POS', N'MTQC02', CAST(0.00 AS Decimal(18, 2)), CAST(14.00 AS Decimal(18, 2)), CAST(0x0000A7990121C8EA AS DateTime))
INSERT [dbo].[sales_payment] ([sales_id], [payment_type], [payment_amount], [change_amount], [due_amount], [dis], [vat], [sales_time], [c_id], [emp_id], [comment], [TrxType], [Shopid], [ovdisrate], [vaterate], [logdate]) VALUES (4, N'Cash', CAST(17.94 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1.50 AS Decimal(18, 2)), CAST(0.14 AS Decimal(18, 2)), CAST(2.20 AS Decimal(18, 2)), N'2017-06-19', N'10000009', N'a', N'Guest  ', N'POS', N'MTQC02', CAST(0.00 AS Decimal(18, 2)), CAST(14.00 AS Decimal(18, 2)), CAST(0x0000A7990121C8F4 AS DateTime))
INSERT [dbo].[sales_payment] ([sales_id], [payment_type], [payment_amount], [change_amount], [due_amount], [dis], [vat], [sales_time], [c_id], [emp_id], [comment], [TrxType], [Shopid], [ovdisrate], [vaterate], [logdate]) VALUES (5, N'Cash', CAST(11.14 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1.14 AS Decimal(18, 2)), CAST(0.40 AS Decimal(18, 2)), CAST(1.06 AS Decimal(18, 2)), N'2017-06-20', N'10000011', N'a', N'Jinat  ', N'POS', N'MTQC02', CAST(0.00 AS Decimal(18, 2)), CAST(14.00 AS Decimal(18, 2)), CAST(0x0000A7990121C8FD AS DateTime))
INSERT [dbo].[sales_payment] ([sales_id], [payment_type], [payment_amount], [change_amount], [due_amount], [dis], [vat], [sales_time], [c_id], [emp_id], [comment], [TrxType], [Shopid], [ovdisrate], [vaterate], [logdate]) VALUES (6, N'Invoice', CAST(166.52 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.14 AS Decimal(18, 2)), N'2017-06-21', N'10000009', N'a', N'Guest  ', N'Inventory', N'MTQC02', CAST(0.00 AS Decimal(18, 2)), CAST(14.00 AS Decimal(18, 2)), CAST(0x0000A7990121C907 AS DateTime))
INSERT [dbo].[sales_payment] ([sales_id], [payment_type], [payment_amount], [change_amount], [due_amount], [dis], [vat], [sales_time], [c_id], [emp_id], [comment], [TrxType], [Shopid], [ovdisrate], [vaterate], [logdate]) VALUES (7, N'Cash', CAST(1.70 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.21 AS Decimal(18, 2)), N'2017-06-21', N'10000009', N'a', N'Guest  ', N'POS', N'MTQC02', CAST(0.00 AS Decimal(18, 2)), CAST(14.00 AS Decimal(18, 2)), CAST(0x0000A7990121C90B AS DateTime))
INSERT [dbo].[sales_payment] ([sales_id], [payment_type], [payment_amount], [change_amount], [due_amount], [dis], [vat], [sales_time], [c_id], [emp_id], [comment], [TrxType], [Shopid], [ovdisrate], [vaterate], [logdate]) VALUES (8, N'Cash', CAST(1.43 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.18 AS Decimal(18, 2)), N'2017-06-21', N'10000011', N'a', N'Jinat  1 milk, 1 salt, 2 sugur.', N'POS', N'MTQC02', CAST(0.00 AS Decimal(18, 2)), CAST(14.00 AS Decimal(18, 2)), CAST(0x0000A7990121C915 AS DateTime))
INSERT [dbo].[sales_payment] ([sales_id], [payment_type], [payment_amount], [change_amount], [due_amount], [dis], [vat], [sales_time], [c_id], [emp_id], [comment], [TrxType], [Shopid], [ovdisrate], [vaterate], [logdate]) VALUES (9, N'Cash', CAST(6.99 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'2017-06-21', N'10000009', N'a', N'Guest  ', N'POS', N'MTQC02', CAST(0.00 AS Decimal(18, 2)), CAST(14.98 AS Decimal(18, 2)), CAST(0x0000A799012BF773 AS DateTime))
INSERT [dbo].[sales_payment] ([sales_id], [payment_type], [payment_amount], [change_amount], [due_amount], [dis], [vat], [sales_time], [c_id], [emp_id], [comment], [TrxType], [Shopid], [ovdisrate], [vaterate], [logdate]) VALUES (10, N'Cash', CAST(5.98 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'2017-06-21', N'10000009', N'a', N'Guest  ', N'POS', N'MTQC02', CAST(0.00 AS Decimal(18, 2)), CAST(14.98 AS Decimal(18, 2)), CAST(0x0000A7990137D5DB AS DateTime))
INSERT [dbo].[sales_payment] ([sales_id], [payment_type], [payment_amount], [change_amount], [due_amount], [dis], [vat], [sales_time], [c_id], [emp_id], [comment], [TrxType], [Shopid], [ovdisrate], [vaterate], [logdate]) VALUES (11, N'Cash', CAST(4.59 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.60 AS Decimal(18, 2)), N'2017-10-05', N'10000009', N'a', N'Guest  ', N'POS', N'MTQC02', CAST(0.00 AS Decimal(18, 2)), CAST(14.98 AS Decimal(18, 2)), CAST(0x0000A803013B3567 AS DateTime))
/****** Object:  Table [dbo].[sales_item]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sales_item](
	[sales_id] [varchar](150) NULL,
	[item_id] [bigint] IDENTITY(1,1) NOT NULL,
	[itemName] [nvarchar](250) NULL,
	[Qty] [decimal](18, 2) NULL,
	[RetailsPrice] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NULL,
	[profit] [decimal](18, 2) NULL,
	[sales_time] [varchar](150) NULL,
	[itemcode] [varchar](150) NULL,
	[discount] [decimal](18, 2) NULL,
	[taxapply] [varchar](50) NULL,
	[status] [int] NULL,
	[logDate] [datetime] NULL,
 CONSTRAINT [PK_sales_item] PRIMARY KEY CLUSTERED 
(
	[item_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1= Sold 2= Returned' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sales_item', @level2type=N'COLUMN',@level2name=N'status'
GO
SET IDENTITY_INSERT [dbo].[sales_item] ON
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'1', 1, N'Cocacola 200ml', CAST(3.00 AS Decimal(18, 2)), CAST(2.50 AS Decimal(18, 2)), CAST(7.50 AS Decimal(18, 2)), CAST(0.60 AS Decimal(18, 2)), N'2017-06-19', N'8940000000002', CAST(0.00 AS Decimal(18, 2)), N'0', 1, CAST(0x0000A79901214BA4 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'1', 2, N'Cocktail_juice', CAST(1.00 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(1.54 AS Decimal(18, 2)), N'2017-06-19', N'8940000000007', CAST(5.00 AS Decimal(18, 2)), N'1', 3, CAST(0x0000A79901214BCE AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'2', 3, N'Mc_Chicken_sm', CAST(1.00 AS Decimal(18, 2)), CAST(1.50 AS Decimal(18, 2)), CAST(1.50 AS Decimal(18, 2)), CAST(0.50 AS Decimal(18, 2)), N'2017-06-19', N'8940000000012', CAST(0.00 AS Decimal(18, 2)), N'1', 1, CAST(0x0000A79901214BD8 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'2', 4, N'HamBurger_Small', CAST(1.00 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(0.99 AS Decimal(18, 2)), N'2017-06-19', N'8940000000011', CAST(0.00 AS Decimal(18, 2)), N'1', 1, CAST(0x0000A79901214BE1 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'2', 5, N'Mc_Double_sm', CAST(1.00 AS Decimal(18, 2)), CAST(1.99 AS Decimal(18, 2)), CAST(1.99 AS Decimal(18, 2)), CAST(0.74 AS Decimal(18, 2)), N'2017-06-19', N'8940000000013', CAST(0.00 AS Decimal(18, 2)), N'1', 1, CAST(0x0000A79901214BEB AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'3', 6, N'Mc_Double_sm', CAST(1.00 AS Decimal(18, 2)), CAST(1.99 AS Decimal(18, 2)), CAST(1.99 AS Decimal(18, 2)), CAST(0.74 AS Decimal(18, 2)), N'2017-06-19', N'8940000000013', CAST(0.00 AS Decimal(18, 2)), N'1', 3, CAST(0x0000A79901214BF4 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'3', 7, N'Apple_LB', CAST(1.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), N'2017-06-19', N'211212', CAST(0.00 AS Decimal(18, 2)), N'0', 1, CAST(0x0000A79901214BFE AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'4', 8, N'HamBurger_Small', CAST(1.00 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(0.99 AS Decimal(18, 2)), N'2017-06-19', N'8940000000011', CAST(0.00 AS Decimal(18, 2)), N'1', 3, CAST(0x0000A79901214C07 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'4', 9, N'Cocacola150ml', CAST(1.00 AS Decimal(18, 2)), CAST(1.75 AS Decimal(18, 2)), CAST(1.75 AS Decimal(18, 2)), CAST(0.75 AS Decimal(18, 2)), N'2017-06-19', N'8940000000010', CAST(0.00 AS Decimal(18, 2)), N'1', 1, CAST(0x0000A79901214C10 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'4', 10, N'CocalcolaCan', CAST(1.00 AS Decimal(18, 2)), CAST(1.15 AS Decimal(18, 2)), CAST(1.15 AS Decimal(18, 2)), CAST(0.16 AS Decimal(18, 2)), N'2017-06-19', N'8940000000009', CAST(0.00 AS Decimal(18, 2)), N'1', 1, CAST(0x0000A79901214C19 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'4', 11, N'MountainDew770', CAST(1.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), N'2017-06-19', N'8940000000014', CAST(0.00 AS Decimal(18, 2)), N'1', 1, CAST(0x0000A79901214C23 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'4', 12, N'Dell_Mouse', CAST(1.00 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(1.85 AS Decimal(18, 2)), N'2017-06-19', N'8940000000008', CAST(3.50 AS Decimal(18, 2)), N'1', 1, CAST(0x0000A79901214C2C AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'5', 13, N'Cocktail_juice', CAST(2.00 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(7.98 AS Decimal(18, 2)), CAST(1.54 AS Decimal(18, 2)), N'2017-06-20', N'8940000000007', CAST(5.00 AS Decimal(18, 2)), N'1', 3, CAST(0x0000A79901214C36 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'5', 14, N'Cocacola 200ml', CAST(1.00 AS Decimal(18, 2)), CAST(2.50 AS Decimal(18, 2)), CAST(2.50 AS Decimal(18, 2)), CAST(0.60 AS Decimal(18, 2)), N'2017-06-20', N'8940000000002', CAST(0.00 AS Decimal(18, 2)), N'0', 1, CAST(0x0000A79901214C3F AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'6', 15, N'Cocacola 200ml', CAST(1.00 AS Decimal(18, 2)), CAST(2.50 AS Decimal(18, 2)), CAST(2.50 AS Decimal(18, 2)), CAST(0.60 AS Decimal(18, 2)), N'2017-06-21', N'8940000000002', CAST(0.00 AS Decimal(18, 2)), N'0', 1, CAST(0x0000A79901214C49 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'6', 16, N'Pizaa_Mid400gm', CAST(12.00 AS Decimal(18, 2)), CAST(9.99 AS Decimal(18, 2)), CAST(119.88 AS Decimal(18, 2)), CAST(2.99 AS Decimal(18, 2)), N'2017-06-21', N'8940000000003', CAST(0.00 AS Decimal(18, 2)), N'1', 1, CAST(0x0000A79901214C52 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'6', 17, N'Compass', CAST(6.00 AS Decimal(18, 2)), CAST(4.00 AS Decimal(18, 2)), CAST(24.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), N'2017-06-21', N'8940000000004', CAST(0.00 AS Decimal(18, 2)), N'1', 1, CAST(0x0000A79901214C5B AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'7', 18, N'Coffe_medium', CAST(1.00 AS Decimal(18, 2)), CAST(1.49 AS Decimal(18, 2)), CAST(1.49 AS Decimal(18, 2)), CAST(0.49 AS Decimal(18, 2)), N'2017-06-21', N'897463585492', CAST(0.00 AS Decimal(18, 2)), N'1', 3, CAST(0x0000A79901214C64 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'8', 19, N'Tea_sm', CAST(1.00 AS Decimal(18, 2)), CAST(1.25 AS Decimal(18, 2)), CAST(1.25 AS Decimal(18, 2)), CAST(0.25 AS Decimal(18, 2)), N'2017-06-21', N'8940000000022', CAST(0.00 AS Decimal(18, 2)), N'1', 3, CAST(0x0000A79901214C6E AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'9', 20, N'Grapes_LB', CAST(1.00 AS Decimal(18, 2)), CAST(2.99 AS Decimal(18, 2)), CAST(2.99 AS Decimal(18, 2)), CAST(0.99 AS Decimal(18, 2)), N'2017-06-21', N'134586798323', CAST(0.00 AS Decimal(18, 2)), N'0', 1, CAST(0x0000A799012BF786 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'9', 21, N'Apple_LB', CAST(1.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), N'2017-06-21', N'211212', CAST(0.00 AS Decimal(18, 2)), N'0', 1, CAST(0x0000A799012BF7A2 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'10', 22, N'Grapes_LB', CAST(1.00 AS Decimal(18, 2)), CAST(2.99 AS Decimal(18, 2)), CAST(2.99 AS Decimal(18, 2)), CAST(0.99 AS Decimal(18, 2)), N'2017-06-21', N'134586798323', CAST(0.00 AS Decimal(18, 2)), N'0', 1, CAST(0x0000A7990137D5E5 AS DateTime))
INSERT [dbo].[sales_item] ([sales_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [profit], [sales_time], [itemcode], [discount], [taxapply], [status], [logDate]) VALUES (N'11', 23, N'HamBurger_Small', CAST(1.00 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(0.99 AS Decimal(18, 2)), N'2017-10-05', N'8940000000011', CAST(0.00 AS Decimal(18, 2)), N'1', 3, CAST(0x0000A803013B356C AS DateTime))
SET IDENTITY_INSERT [dbo].[sales_item] OFF
/****** Object:  Table [dbo].[return_item]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[return_item](
	[return_id] [bigint] IDENTITY(1,1) NOT NULL,
	[item_id] [varchar](150) NULL,
	[itemName] [nvarchar](250) NULL,
	[Qty] [decimal](18, 2) NULL,
	[RetailsPrice] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NULL,
	[return_time] [varchar](150) NULL,
	[custno] [varchar](150) NULL,
	[emp] [varchar](150) NULL,
	[SoldInvoiceNo] [varchar](150) NULL,
	[Comment] [nvarchar](250) NULL,
	[disamt] [decimal](18, 2) NULL,
	[vatamt] [decimal](18, 2) NULL,
	[logdate] [datetime] NULL,
 CONSTRAINT [PK_return_item] PRIMARY KEY CLUSTERED 
(
	[return_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[return_item] ON
INSERT [dbo].[return_item] ([return_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [return_time], [custno], [emp], [SoldInvoiceNo], [Comment], [disamt], [vatamt], [logdate]) VALUES (1, N'8940000000012', N'Mc_Chicken_sm', CAST(1.00 AS Decimal(18, 2)), CAST(1.50 AS Decimal(18, 2)), CAST(1.50 AS Decimal(18, 2)), N'2017-06-19', N'10000010', N'a', N'2', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.21 AS Decimal(18, 2)), CAST(0x0000A799011FAA51 AS DateTime))
INSERT [dbo].[return_item] ([return_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [return_time], [custno], [emp], [SoldInvoiceNo], [Comment], [disamt], [vatamt], [logdate]) VALUES (2, N'8940000000013', N'Mc_Double_sm', CAST(1.00 AS Decimal(18, 2)), CAST(1.99 AS Decimal(18, 2)), CAST(1.99 AS Decimal(18, 2)), N'2017-06-19', N'10000009', N'a', N'3', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.28 AS Decimal(18, 2)), CAST(0x0000A799011FAA5B AS DateTime))
INSERT [dbo].[return_item] ([return_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [return_time], [custno], [emp], [SoldInvoiceNo], [Comment], [disamt], [vatamt], [logdate]) VALUES (3, N'211212', N'Apple_LB', CAST(1.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), N'2017-06-21', N'10000009', N'a', N'9', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0x0000A799012C0ED3 AS DateTime))
INSERT [dbo].[return_item] ([return_id], [item_id], [itemName], [Qty], [RetailsPrice], [Total], [return_time], [custno], [emp], [SoldInvoiceNo], [Comment], [disamt], [vatamt], [logdate]) VALUES (4, N'134586798323', N'Grapes_LB', CAST(1.00 AS Decimal(18, 2)), CAST(2.99 AS Decimal(18, 2)), CAST(2.99 AS Decimal(18, 2)), N'2017-06-21', N'10000009', N'a', N'10', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0x0000A7990137E169 AS DateTime))
SET IDENTITY_INSERT [dbo].[return_item] OFF
/****** Object:  Table [dbo].[purchase]    Script Date: 10/05/2017 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[purchase](
	[product_id] [varchar](250) NOT NULL,
	[product_name] [nvarchar](250) NULL,
	[product_quantity] [decimal](18, 2) NULL,
	[cost_price] [decimal](18, 2) NULL,
	[retail_price] [decimal](18, 2) NULL,
	[total_cost_price] [decimal](18, 2) NULL,
	[total_retail_price] [decimal](18, 2) NULL,
	[category] [nvarchar](150) NULL,
	[supplier] [nvarchar](150) NULL,
	[imagename] [varchar](250) NULL,
	[discount] [decimal](18, 2) NULL,
	[taxapply] [int] NULL,
	[Shopid] [varchar](50) NULL,
	[status] [int] NULL,
	[logDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[Updateby] [varchar](150) NULL,
 CONSTRAINT [PK_purchase] PRIMARY KEY CLUSTERED 
(
	[product_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'134586798323', N'Grapes_LB', CAST(209.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(2.99 AS Decimal(18, 2)), CAST(422.00 AS Decimal(18, 2)), CAST(630.89 AS Decimal(18, 2)), N'Fruit', N'Fruit_company', N'134586798323.png', CAST(0.00 AS Decimal(18, 2)), 0, N'MTQC02', 1, CAST(0x0000A79901093429 AS DateTime), CAST(0x0000A79901093429 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'211212', N'Apple_LB', CAST(998.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(999.00 AS Decimal(18, 2)), CAST(1998.00 AS Decimal(18, 2)), N'Cigarettes', N'CocaCola', N'211212.png', CAST(0.00 AS Decimal(18, 2)), 0, N'MTQC02', 1, CAST(0x0000A79901093429 AS DateTime), CAST(0x0000A79901093429 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'5656', N'tert88888888', CAST(95.00 AS Decimal(18, 2)), CAST(56.00 AS Decimal(18, 2)), CAST(66.00 AS Decimal(18, 2)), CAST(5320.00 AS Decimal(18, 2)), CAST(6270.00 AS Decimal(18, 2)), N'Cigarettes', N'Unknown', N'5656.png', CAST(2.00 AS Decimal(18, 2)), 0, N'MTQC02', 1, CAST(0x0000A79901093420 AS DateTime), CAST(0x0000A79901093420 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'78579846', N'Ice_cream_330gm', CAST(12122.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(2.99 AS Decimal(18, 2)), CAST(24246.00 AS Decimal(18, 2)), CAST(36247.77 AS Decimal(18, 2)), N'CoolDrink', N'Tim Horton', N'78579846.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 1, CAST(0x0000A7990109342E AS DateTime), CAST(0x0000A7990109342E AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'80045332', N'Tomato1LB', CAST(19.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(2.99 AS Decimal(18, 2)), CAST(38.00 AS Decimal(18, 2)), CAST(56.81 AS Decimal(18, 2)), N'Food', N'FoodCo', N'80045332.png', CAST(0.00 AS Decimal(18, 2)), 0, N'MTQC02', 1, CAST(0x0000A79901093425 AS DateTime), CAST(0x0000A79901093425 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'89234500012', N'Shopping cart', CAST(7.00 AS Decimal(18, 2)), CAST(45.00 AS Decimal(18, 2)), CAST(49.00 AS Decimal(18, 2)), CAST(315.00 AS Decimal(18, 2)), CAST(343.00 AS Decimal(18, 2)), N'Metal', N'Uniliver', N'89234500012.png', CAST(3.00 AS Decimal(18, 2)), 1, N'MTQC02', 1, CAST(0x0000A79901093425 AS DateTime), CAST(0x0000A79901093425 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000002', N'Cocacola 200ml', CAST(279.50 AS Decimal(18, 2)), CAST(1.90 AS Decimal(18, 2)), CAST(2.50 AS Decimal(18, 2)), CAST(531.05 AS Decimal(18, 2)), CAST(698.75 AS Decimal(18, 2)), N'SoftDrink', N'CocaCola', N'8940000000002.png', CAST(0.00 AS Decimal(18, 2)), 0, N'MTQC02', 1, CAST(0x0000A799010933F1 AS DateTime), CAST(0x0000A799010933F1 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000003', N'Pizaa_Mid400gm', CAST(60.00 AS Decimal(18, 2)), CAST(7.00 AS Decimal(18, 2)), CAST(9.99 AS Decimal(18, 2)), CAST(427.00 AS Decimal(18, 2)), CAST(609.39 AS Decimal(18, 2)), N'Food', N'Mcdonalds', N'8940000000003.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 3, CAST(0x0000A799010933F6 AS DateTime), CAST(0x0000A799010933F6 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000004', N'Compass', CAST(792.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(4.00 AS Decimal(18, 2)), CAST(1584.00 AS Decimal(18, 2)), CAST(3168.00 AS Decimal(18, 2)), N'Electronics', N'Dell', N'8940000000004.png', CAST(0.00 AS Decimal(18, 2)), 1, N'Lo13', 1, CAST(0x0000A799010933FB AS DateTime), CAST(0x0000A799010933FB AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000005', N'iPhone5', CAST(12.00 AS Decimal(18, 2)), CAST(600.00 AS Decimal(18, 2)), CAST(710.00 AS Decimal(18, 2)), CAST(7200.00 AS Decimal(18, 2)), CAST(8520.00 AS Decimal(18, 2)), N'Electronics', N'Dell', N'8940000000005.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 1, CAST(0x0000A799010933FB AS DateTime), CAST(0x0000A799010933FB AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000006', N'Money_bag', CAST(18.50 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(110.00 AS Decimal(18, 2)), CAST(1850.00 AS Decimal(18, 2)), CAST(2035.00 AS Decimal(18, 2)), N'Leather', N'AmericanBlend', N'8940000000006.png', CAST(5.00 AS Decimal(18, 2)), 1, N'MTQC02', 1, CAST(0x0000A799010933FB AS DateTime), CAST(0x0000A799010933FB AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000007', N'Cocktail_juice', CAST(1944.00 AS Decimal(18, 2)), CAST(2.25 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(4374.00 AS Decimal(18, 2)), CAST(7756.56 AS Decimal(18, 2)), N'Food', N'Mcdonalds', N'8940000000007.png', CAST(5.00 AS Decimal(18, 2)), 1, N'HamRT5', 3, CAST(0x0000A799010933FF AS DateTime), CAST(0x0000A799010933FF AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000008', N'Dell_Mouse', CAST(20.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(79.80 AS Decimal(18, 2)), N'Electronics', N'Dell', N'8940000000008.png', CAST(3.50 AS Decimal(18, 2)), 1, N'MTQC02', 1, CAST(0x0000A799010933FF AS DateTime), CAST(0x0000A799010933FF AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000009', N'CocalcolaCan', CAST(758.00 AS Decimal(18, 2)), CAST(0.99 AS Decimal(18, 2)), CAST(1.15 AS Decimal(18, 2)), CAST(751.41 AS Decimal(18, 2)), CAST(872.85 AS Decimal(18, 2)), N'SoftDrink', N'CocaCola', N'8940000000009.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 1, CAST(0x0000A79901093404 AS DateTime), CAST(0x0000A79901093404 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000010', N'Cocacola150ml', CAST(283.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.75 AS Decimal(18, 2)), CAST(283.00 AS Decimal(18, 2)), CAST(495.25 AS Decimal(18, 2)), N'SoftDrink', N'CocaCola', N'8940000000010.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 1, CAST(0x0000A79901093404 AS DateTime), CAST(0x0000A79901093404 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000011', N'HamBurger_Small', CAST(375.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(3.99 AS Decimal(18, 2)), CAST(1131.00 AS Decimal(18, 2)), CAST(1504.23 AS Decimal(18, 2)), N'Burger', N'Mcdonalds', N'8940000000011.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 3, CAST(0x0000A79901093404 AS DateTime), CAST(0x0000A79901093404 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000012', N'Mc_Chicken_sm', CAST(7686.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.50 AS Decimal(18, 2)), CAST(7688.00 AS Decimal(18, 2)), CAST(11532.00 AS Decimal(18, 2)), N'Burger', N'Mcdonalds', N'8940000000012.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 3, CAST(0x0000A79901093409 AS DateTime), CAST(0x0000A79901093409 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000013', N'Mc_Double_sm', CAST(877.00 AS Decimal(18, 2)), CAST(1.25 AS Decimal(18, 2)), CAST(1.99 AS Decimal(18, 2)), CAST(1097.50 AS Decimal(18, 2)), CAST(1747.22 AS Decimal(18, 2)), N'Burger', N'Mcdonalds', N'8940000000013.png', CAST(0.00 AS Decimal(18, 2)), 1, N'WION05', 3, CAST(0x0000A79901093409 AS DateTime), CAST(0x0000A79901093409 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000014', N'MountainDew770', CAST(36.00 AS Decimal(18, 2)), CAST(4.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), CAST(144.00 AS Decimal(18, 2)), CAST(180.00 AS Decimal(18, 2)), N'CoolDrink', N'Dew', N'8940000000014.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 1, CAST(0x0000A79901093409 AS DateTime), CAST(0x0000A79901093409 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000015', N'MountainDew355', CAST(37.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(76.00 AS Decimal(18, 2)), CAST(114.00 AS Decimal(18, 2)), N'CoolDrink', N'Dew', N'8940000000015.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 1, CAST(0x0000A7990109340D AS DateTime), CAST(0x0000A7990109340D AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000016', N'Pall mall 20 Cigarette', CAST(35.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), CAST(6.50 AS Decimal(18, 2)), CAST(180.00 AS Decimal(18, 2)), CAST(234.00 AS Decimal(18, 2)), N'Cigarettes', N'AmericanBlend', N'8940000000016.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 1, CAST(0x0000A7990109340D AS DateTime), CAST(0x0000A7990109340D AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000017', N'Chicken_Bhona', CAST(390.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.25 AS Decimal(18, 2)), CAST(390.00 AS Decimal(18, 2)), CAST(487.50 AS Decimal(18, 2)), N'Food', N'Subway', N'8940000000017.png', CAST(0.00 AS Decimal(18, 2)), 0, N'MTQC02', 1, CAST(0x0000A7990109340D AS DateTime), CAST(0x0000A7990109340D AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000019', N'PUDDING', CAST(195.00 AS Decimal(18, 2)), CAST(0.99 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(194.04 AS Decimal(18, 2)), CAST(196.00 AS Decimal(18, 2)), N'Food', N'Mcdonalds', N'8940000000019.png', CAST(0.00 AS Decimal(18, 2)), 0, N'MTQC02', 3, CAST(0x0000A79901093412 AS DateTime), CAST(0x0000A79901093412 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000020', N'SENSODYNE_65ML', CAST(388.00 AS Decimal(18, 2)), CAST(2.50 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(970.00 AS Decimal(18, 2)), CAST(1164.00 AS Decimal(18, 2)), N'Medicine', N'Square', N'8940000000020.PNG', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 1, CAST(0x0000A79901093412 AS DateTime), CAST(0x0000A79901093412 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000021', N'strawberry_400gm', CAST(375.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.25 AS Decimal(18, 2)), CAST(375.00 AS Decimal(18, 2)), CAST(468.75 AS Decimal(18, 2)), N'Fruit', N'Uniliver', N'8940000000021.png', CAST(0.00 AS Decimal(18, 2)), 0, N'MTQC02', 1, CAST(0x0000A79901093416 AS DateTime), CAST(0x0000A79901093416 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000022', N'Tea_sm', CAST(893.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.25 AS Decimal(18, 2)), CAST(895.00 AS Decimal(18, 2)), CAST(1118.75 AS Decimal(18, 2)), N'Drink', N'Tim Horton', N'8940000000022.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 3, CAST(0x0000A79901093416 AS DateTime), CAST(0x0000A79901093416 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000023', N'Juice-250gm', CAST(39.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.99 AS Decimal(18, 2)), CAST(39.00 AS Decimal(18, 2)), CAST(77.61 AS Decimal(18, 2)), N'soft_drink', N'Tim Horton', N'8940000000023.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 3, CAST(0x0000A79901093416 AS DateTime), CAST(0x0000A79901093416 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000024', N'Apple_400gm', CAST(39.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(78.00 AS Decimal(18, 2)), CAST(117.00 AS Decimal(18, 2)), N'Fruit', N'AmericanBlend', N'8940000000024.png', CAST(0.00 AS Decimal(18, 2)), 0, N'MTQC02', 1, CAST(0x0000A7990109341B AS DateTime), CAST(0x0000A7990109341B AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000025', N'Banana_lb', CAST(39.00 AS Decimal(18, 2)), CAST(0.56 AS Decimal(18, 2)), CAST(0.79 AS Decimal(18, 2)), CAST(21.84 AS Decimal(18, 2)), CAST(30.81 AS Decimal(18, 2)), N'Fruit', N'AmericanBlend', N'8940000000025.png', CAST(0.00 AS Decimal(18, 2)), 0, N'MTQC02', 1, CAST(0x0000A7990109341B AS DateTime), CAST(0x0000A7990109341B AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000026', N'Grapes_lb', CAST(38.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.99 AS Decimal(18, 2)), CAST(38.00 AS Decimal(18, 2)), CAST(75.62 AS Decimal(18, 2)), N'Fruit', N'Fruit_company', N'8940000000026.png', CAST(0.00 AS Decimal(18, 2)), 0, N'MTQC02', 1, CAST(0x0000A79901093420 AS DateTime), CAST(0x0000A79901093420 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000027', N'Coffe_Mix_milk', CAST(2476.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(2.49 AS Decimal(18, 2)), CAST(2476.00 AS Decimal(18, 2)), CAST(6165.24 AS Decimal(18, 2)), N'Coffe', N'Tim Horton', N'8940000000027.png', CAST(5.00 AS Decimal(18, 2)), 0, N'MTQC02', 3, CAST(0x0000A79901093420 AS DateTime), CAST(0x0000A79901093420 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'8940000000035', N'New one', CAST(342.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(2.25 AS Decimal(18, 2)), CAST(684.00 AS Decimal(18, 2)), CAST(769.50 AS Decimal(18, 2)), N'SoftDrink', N'Uniliver', N'8940000000035item.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 1, CAST(0x0000A79901093425 AS DateTime), CAST(0x0000A79901093425 AS DateTime), NULL)
INSERT [dbo].[purchase] ([product_id], [product_name], [product_quantity], [cost_price], [retail_price], [total_cost_price], [total_retail_price], [category], [supplier], [imagename], [discount], [taxapply], [Shopid], [status], [logDate], [UpdateDate], [Updateby]) VALUES (N'897463585492', N'Coffe_medium', CAST(308.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.49 AS Decimal(18, 2)), CAST(230.00 AS Decimal(18, 2)), CAST(342.70 AS Decimal(18, 2)), N'Coffe', N'Mcdonalds', N'897463585492.png', CAST(0.00 AS Decimal(18, 2)), 1, N'MTQC02', 3, CAST(0x0000A79901093429 AS DateTime), CAST(0x0000A79901093429 AS DateTime), NULL)
/****** Object:  View [dbo].[vw_workrecords]    Script Date: 10/05/2017 20:27:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_workrecords]
AS		 
		-- Get the Result in HH:MI:SS:MMM(24H) format
		--SELECT CONVERT(VARCHAR(12), DATEADD(MS, DATEDIFF(MS, '20:50:59', '21:59:59'), 0), 114) AS TimeDiff

		SELECT  username , logdate as 'Date' ,  min( logtime) as 'IN' , max( logtime)  as 'OUT', 
		 CONVERT(VARCHAR(8), DATEADD(MS, DATEDIFF(MS, MIN(logtime), MAX(logtime)), 0), 114) AS 'HOURS'
		from tbl_workrecords group by username , logdate
GO
/****** Object:  View [dbo].[vw_itemdisplay_sr]    Script Date: 10/05/2017 20:27:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_itemdisplay_sr]
AS		 
		  select Top 12 * from purchase ORDER BY NEWID()
GO
/****** Object:  View [dbo].[vw_General_Ledger]    Script Date: 10/05/2017 20:27:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_General_Ledger] 
AS 
		SELECT     sp.sales_time AS Date, SUM(sp.payment_amount) AS Sales, ISNULL((SUM(ri.Total) - SUM(ri.disamt)) + SUM(ri.vatamt) ,0)  as 'Return'
		FROM         dbo.sales_payment AS sp LEFT OUTER JOIN
		dbo.return_item AS ri 
		ON sp.sales_id = ri.SoldInvoiceNo
		GROUP BY sp.sales_time
GO
/****** Object:  View [dbo].[vw_CustCreditReport]    Script Date: 10/05/2017 20:27:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_CustCreditReport]
AS
		SELECT cc.ID AS TrxID, cc.Date, Customers.ID AS CustID, Customers.Name, cc.OrderID, cc.Credit, cc.Description
		FROM         dbo.tbl_CustCredit AS cc 
		LEFT OUTER JOIN  dbo.tbl_customer AS Customers 
		ON cc.CustID = Customers.ID
		--ORDER BY cc.Date DESC
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "cc"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 177
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Customers"
            Begin Extent = 
               Top = 6
               Left = 236
               Bottom = 188
               Right = 412
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_CustCreditReport'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_CustCreditReport'
GO
/****** Object:  View [dbo].[CustomerCredit]    Script Date: 10/05/2017 20:27:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CustomerCredit]
AS
 SELECT Customers.ID, Customers.Name, Customers.Phone as Mobile, Customers.Address ,Customers.EmailAddress, Customers.City,Customers.PeopleType 
	FROM tbl_customer Customers
	LEFT JOIN tbl_CustCredit  cc
	ON CC.CustID=Customers.ID
	group by  Customers.ID, Customers.Name, Customers.Phone, Customers.Address ,Customers.EmailAddress, Customers.City,Customers.PeopleType
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[31] 4[30] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Customers"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 126
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cc"
            Begin Extent = 
               Top = 6
               Left = 236
               Bottom = 126
               Right = 396
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CustomerCredit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CustomerCredit'
GO
/****** Object:  Default [DF_usermgt_logdate]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[usermgt] ADD  CONSTRAINT [DF_usermgt_logdate]  DEFAULT (getdate()) FOR [logdate]
GO
/****** Object:  Default [DF__tbl_workr__statu__5DCAEF64]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[tbl_workrecords] ADD  CONSTRAINT [DF__tbl_workr__statu__5DCAEF64]  DEFAULT ((1)) FOR [status]
GO
/****** Object:  Default [DF_tbl_saleInfo_DateTime]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[tbl_saleInfo] ADD  CONSTRAINT [DF_tbl_saleInfo_DateTime]  DEFAULT (getdate()) FOR [DateTime]
GO
/****** Object:  Default [DF__tbl_purch__statu__6A30C649]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[tbl_purchase_history] ADD  CONSTRAINT [DF__tbl_purch__statu__6A30C649]  DEFAULT ((1)) FOR [status]
GO
/****** Object:  Default [DF_tbl_duepayment_status]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[tbl_duepayment] ADD  CONSTRAINT [DF_tbl_duepayment_status]  DEFAULT ((1)) FOR [status]
GO
/****** Object:  Default [DF_tbl_customer_Logtime]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[tbl_customer] ADD  CONSTRAINT [DF_tbl_customer_Logtime]  DEFAULT (getdate()) FOR [Logtime]
GO
/****** Object:  Default [DF_tbl_CustCredit_Logtime]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[tbl_CustCredit] ADD  CONSTRAINT [DF_tbl_CustCredit_Logtime]  DEFAULT (getdate()) FOR [Logtime]
GO
/****** Object:  Default [DF_storeconfig_updatetime]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[storeconfig] ADD  CONSTRAINT [DF_storeconfig_updatetime]  DEFAULT (getdate()) FOR [updatetime]
GO
/****** Object:  Default [DF_sales_payment_logdate]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[sales_payment] ADD  CONSTRAINT [DF_sales_payment_logdate]  DEFAULT (getdate()) FOR [logdate]
GO
/****** Object:  Default [DF_sales_item_status]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[sales_item] ADD  CONSTRAINT [DF_sales_item_status]  DEFAULT ((1)) FOR [status]
GO
/****** Object:  Default [DF_sales_item_logDate]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[sales_item] ADD  CONSTRAINT [DF_sales_item_logDate]  DEFAULT (getdate()) FOR [logDate]
GO
/****** Object:  Default [DF_return_item_logdate]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[return_item] ADD  CONSTRAINT [DF_return_item_logdate]  DEFAULT (getdate()) FOR [logdate]
GO
/****** Object:  Default [DF_purchase_logDate]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[purchase] ADD  CONSTRAINT [DF_purchase_logDate]  DEFAULT (getdate()) FOR [logDate]
GO
/****** Object:  Default [DF_purchase_UpdateDate]    Script Date: 10/05/2017 20:27:57 ******/
ALTER TABLE [dbo].[purchase] ADD  CONSTRAINT [DF_purchase_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO

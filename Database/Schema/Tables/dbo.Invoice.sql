CREATE TABLE [dbo].[Invoice]
(
[invid] [bigint] NOT NULL,
[invARItemKey] [nvarchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[invInvoiceNum] [nvarchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[invInvoiceDate] [datetime] NULL,
[invAmount] [decimal] (18, 0) NULL,
[invPayorID] [float] NULL,
[invClaimID] [bigint] NULL
) ON [PRIMARY]
GO

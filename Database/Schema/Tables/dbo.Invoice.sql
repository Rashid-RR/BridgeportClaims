CREATE TABLE [dbo].[Invoice]
(
[InvoiceID] [int] NOT NULL IDENTITY(1, 1),
[ARItemKey] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InvoiceNumber] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InvoiceDate] [datetime2] NOT NULL,
[Amount] [money] NOT NULL,
[PayorID] [int] NULL,
[ClaimID] [int] NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfInvoiceCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfInvoiceUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = PAGE
)
GO
ALTER TABLE [dbo].[Invoice] ADD CONSTRAINT [pkInvoice] PRIMARY KEY CLUSTERED  ([InvoiceID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxInvoiceClaimID] ON [dbo].[Invoice] ([ClaimID]) WITH (DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxInvoiceClaimIDInclude] ON [dbo].[Invoice] ([ClaimID]) INCLUDE ([InvoiceNumber]) WITH (DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Invoice] ADD CONSTRAINT [fkInvoiceClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[Invoice] ADD CONSTRAINT [fkInvoicePayorIDPayorPayorID] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payor] ([PayorID])
GO

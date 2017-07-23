CREATE TABLE [dbo].[Invoice]
(
[InvoiceID] [int] NOT NULL IDENTITY(1, 1),
[ARItemKey] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InvoiceNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InvoiceDate] [date] NOT NULL,
[Amount] [money] NOT NULL,
[PayorID] [int] NOT NULL,
[ClaimID] [int] NOT NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfInvoiceCreatedOnUTC] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfInvoiceUpdatedOnUTC] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Invoice] ADD CONSTRAINT [pkInvoice] PRIMARY KEY CLUSTERED  ([InvoiceID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxInvoiceClaimIDClaimClaimIDIncludeAll] ON [dbo].[Invoice] ([ClaimID], [PayorID]) INCLUDE ([Amount], [ARItemKey], [CreatedOn], [InvoiceDate], [InvoiceID], [InvoiceNumber], [UpdatedOn]) WITH (DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Invoice] ADD CONSTRAINT [fkInvoiceClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[Invoice] ADD CONSTRAINT [fkInvoicePayorIDPayorPayorID] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payor] ([PayorID])
GO

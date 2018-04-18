CREATE TABLE [dbo].[Prescription]
(
[PrescriptionID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NOT NULL,
[RxNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DateSubmitted] [datetime2] NOT NULL,
[DateFilled] [datetime2] NOT NULL,
[LabelName] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NDC] [varchar] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Quantity] [float] NOT NULL,
[DaySupply] [float] NOT NULL,
[Generic] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PharmacyNABP] [varchar] (7) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AWPUnit] [float] NULL,
[Usual] [decimal] (18, 0) NULL,
[Prescriber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PayableAmount] [money] NOT NULL,
[BilledAmount] [money] NOT NULL,
[TransactionType] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Compound] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TranID] [varchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RefillDate] [date] NULL,
[RefillNumber] [smallint] NULL,
[MONY] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DAW] [smallint] NULL,
[GPI] [varchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BillIngrCost] [float] NULL,
[BillDispFee] [float] NULL,
[BilledTax] [float] NULL,
[BilledCopay] [float] NULL,
[PayIngrCost] [float] NULL,
[PayDispFee] [float] NULL,
[PayTax] [float] NULL,
[DEA] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PrescriberNPI] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Strength] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GPIGenName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TheraClass] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InvoiceID] [int] NULL,
[ETLRowID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AWP] AS ([Quantity]*[AWPUnit]),
[ReversedDate] [datetime2] NULL,
[IsReversed] AS (CONVERT([bit],case  when [ReversedDate] IS NOT NULL then (1) else (0) end,(0))),
[PrescriptionStatusID] [int] NULL,
[Adjudicated] [money] NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL,
[BilledAmountOriginal] [money] NULL,
[PayableAmountOriginal] [money] NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       4/18/2018
 Description:       Trigger to update the BilledAmountOriginal or PayableAmountOriginal columns.
 =============================================
*/
CREATE TRIGGER [dbo].[utPrescriptionReversedPricingUpdates]
ON [dbo].[Prescription]
AFTER UPDATE
AS
SET NOCOUNT ON;

IF UPDATE([BilledAmount])
    BEGIN
        IF EXISTS
        (
            SELECT  *
            FROM    [Inserted] AS [i]
                    JOIN [Deleted] AS [d] ON [i].[PrescriptionID] = [d].[PrescriptionID]
                                             AND   [i].[BilledAmount] != [d].[BilledAmount]
        )
            BEGIN
                UPDATE  [p]
                SET     [p].[BilledAmountOriginal] = [d].[BilledAmount]
                FROM    [dbo].[Prescription] AS [p]
                        INNER JOIN [Deleted] AS [d] ON [d].[PrescriptionID] = [p].[PrescriptionID]
            END
    END

IF UPDATE([PayableAmount])
    BEGIN
        IF EXISTS
        (
            SELECT  *
            FROM    [Inserted] AS [i]
                    JOIN [Deleted] AS [d] ON [i].[PrescriptionID] = [d].[PrescriptionID]
                                             AND   [i].[PayableAmount] != [d].[PayableAmount]
        )
            BEGIN
                UPDATE  [p]
                SET     [p].[PayableAmountOriginal] = [d].[PayableAmount]
                FROM    [dbo].[Prescription] AS [p]
                        INNER JOIN [Deleted] AS [d] ON [d].[PrescriptionID] = [p].[PrescriptionID]
            END
    END
GO
ALTER TABLE [dbo].[Prescription] ADD CONSTRAINT [pkPrescription] PRIMARY KEY CLUSTERED  ([PrescriptionID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionBilledAmountIncludes] ON [dbo].[Prescription] ([BilledAmount]) INCLUDE ([ClaimID], [InvoiceID], [PharmacyNABP]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionClaimIDIncludes] ON [dbo].[Prescription] ([ClaimID]) INCLUDE ([LabelName], [PayableAmount], [RefillDate], [RxNumber]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionCreatedOnUTCIncludeIsReversed] ON [dbo].[Prescription] ([CreatedOnUTC]) INCLUDE ([IsReversed]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionETLRowID] ON [dbo].[Prescription] ([ETLRowID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionInvoiceIDIncludes] ON [dbo].[Prescription] ([InvoiceID]) INCLUDE ([ClaimID], [DateFilled], [LabelName], [RxNumber]) WHERE ([InvoiceID] IS NOT NULL) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionIsReversedIncludes] ON [dbo].[Prescription] ([IsReversed]) INCLUDE ([BilledAmount], [ClaimID], [DateFilled], [DateSubmitted], [InvoiceID], [LabelName], [PayableAmount], [PharmacyNABP], [RxNumber]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionIsReversedETLRowIDIncludes] ON [dbo].[Prescription] ([IsReversed], [ETLRowID]) INCLUDE ([DateSubmitted], [MONY], [PharmacyNABP], [PrescriptionID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionPharmacyNABPIncludes] ON [dbo].[Prescription] ([PharmacyNABP]) INCLUDE ([BilledAmount], [ClaimID], [DateFilled], [DateSubmitted], [IsReversed], [LabelName], [MONY], [NDC], [PayableAmount], [Prescriber], [Quantity], [ReversedDate], [RxNumber]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionReversedDateBilledAmountIncludes] ON [dbo].[Prescription] ([ReversedDate], [BilledAmount]) INCLUDE ([ClaimID], [DateFilled], [PrescriptionStatusID], [RxNumber]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionRxNumberIncludeClaimID] ON [dbo].[Prescription] ([RxNumber]) INCLUDE ([ClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionUpdatedOnUTC] ON [dbo].[Prescription] ([UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Prescription] ADD CONSTRAINT [fkPrescriptionClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[Prescription] ADD CONSTRAINT [fkPrescriptionInvoiceIDInvoiceInvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [dbo].[Invoice] ([InvoiceID])
GO
ALTER TABLE [dbo].[Prescription] ADD CONSTRAINT [fkPrescriptionPharmacyNABPPharmacyNABP] FOREIGN KEY ([PharmacyNABP]) REFERENCES [dbo].[Pharmacy] ([NABP])
GO
ALTER TABLE [dbo].[Prescription] ADD CONSTRAINT [fkPrescriptionPrescriptionStatusIDPrescriptionStatusPrescriptionStatusID] FOREIGN KEY ([PrescriptionStatusID]) REFERENCES [dbo].[PrescriptionStatus] ([PrescriptionStatusID])
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO

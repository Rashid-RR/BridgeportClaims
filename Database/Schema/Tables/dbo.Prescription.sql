CREATE TABLE [dbo].[Prescription]
(
[PrescriptionID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NULL,
[RxNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DateSubmitted] [datetime2] NOT NULL,
[DateFilled] [datetime2] NULL,
[LabelName] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ndc] [nvarchar] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Quantity] [float] NULL,
[DailySupply] [float] NULL,
[Generic] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PharmacyNABP] [nvarchar] (7) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AWP] [float] NULL,
[AWPUnit] [float] NULL,
[Usual] [decimal] (18, 0) NULL,
[Prescriber] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PayableAmount] [money] NOT NULL,
[BilledAmount] [money] NOT NULL,
[TransactionType] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Compound] [float] NULL,
[Tran] [nvarchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RefillDate] [nvarchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RefillNumber] [smallint] NULL,
[MONY] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DAW] [smallint] NULL,
[GPI] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BillIngrCost] [float] NULL,
[BillDispFee] [int] NULL,
[BilledTax] [int] NULL,
[BilledCopay] [int] NULL,
[PayIngrCost] [int] NULL,
[PayDispFee] [int] NULL,
[PayTax] [int] NULL,
[DEA] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PrescriberNPI] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Strength] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GPIGenName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TheraClass] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Prescription] ADD CONSTRAINT [pkPrescription] PRIMARY KEY CLUSTERED  ([PrescriptionID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Prescription] ADD CONSTRAINT [fkPrescriptionClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO

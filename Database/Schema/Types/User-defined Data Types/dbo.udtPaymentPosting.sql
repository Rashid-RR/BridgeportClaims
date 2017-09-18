CREATE TYPE [dbo].[udtPaymentPosting] AS TABLE
(
[PatientName] [varchar] (311) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RxDate] [datetime2] NOT NULL,
[AmountPosted] [money] NOT NULL,
[InvoiceAmount] [money] NOT NULL,
[Outstanding] [money] NOT NULL,
[PrescriptionID] [int] NOT NULL
)
GO

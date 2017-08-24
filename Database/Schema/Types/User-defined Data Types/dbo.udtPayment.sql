CREATE TYPE [dbo].[udtPayment] AS TABLE
(
[CheckNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LastName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FirstName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RxNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RxDate] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InvoiceNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AmountPaid] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DatePosted] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PrescriptionID] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ClaimID] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

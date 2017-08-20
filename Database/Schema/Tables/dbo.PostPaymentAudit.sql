CREATE TABLE [dbo].[PostPaymentAudit]
(
[RowID] [int] NOT NULL IDENTITY(1, 1),
[PrescriptionID] [int] NULL,
[CheckNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CheckAmount] [money] NULL,
[AmountSelected] [money] NULL,
[AmountToPost] [money] NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[PostPaymentAudit] ADD CONSTRAINT [pkPaymentProcessesAudit] PRIMARY KEY CLUSTERED  ([RowID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO

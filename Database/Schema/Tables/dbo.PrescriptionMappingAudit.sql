CREATE TABLE [dbo].[PrescriptionMappingAudit]
(
[PrescriptionID] [int] NULL,
[ExecutionTime] [datetime2] NOT NULL CONSTRAINT [DF__Prescript__Execu__19FFD4FC] DEFAULT (sysdatetime())
) ON [PRIMARY]
GO

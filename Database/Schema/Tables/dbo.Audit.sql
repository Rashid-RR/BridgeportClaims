CREATE TABLE [dbo].[Audit]
(
[ClaimID] [int] NULL,
[PrescriptionNoteTypeID] [int] NULL,
[NoteText] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EnteredByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PrescriptionNoteID] [int] NULL,
[ExecutionTime] [datetime2] NOT NULL CONSTRAINT [DF__Audit__Execution__190BB0C3] DEFAULT (sysdatetime())
) ON [PRIMARY]
GO

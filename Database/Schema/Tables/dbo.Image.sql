CREATE TABLE [dbo].[Image]
(
[imaid] [int] NOT NULL,
[imaclaid] [int] NULL,
[imanumber] [int] NULL,
[imatype] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[imadaterec] [datetime] NULL
) ON [PRIMARY]
GO

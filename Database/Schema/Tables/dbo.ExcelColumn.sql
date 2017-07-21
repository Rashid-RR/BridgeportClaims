CREATE TABLE [dbo].[ExcelColumn]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[AlphabetColum] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ExcelColumn] ADD CONSTRAINT [pkExcelColumn] PRIMARY KEY CLUSTERED  ([ID]) ON [PRIMARY]
GO

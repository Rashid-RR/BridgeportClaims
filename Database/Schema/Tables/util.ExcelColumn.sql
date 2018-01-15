CREATE TABLE [util].[ExcelColumn]
(
[ExcelColumnID] [int] NOT NULL IDENTITY(1, 1),
[AlphabetColum] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [util].[ExcelColumn] ADD CONSTRAINT [pkExcelColumn] PRIMARY KEY CLUSTERED  ([ExcelColumnID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idxUqExcelColumnAlphabetColumn] ON [util].[ExcelColumn] ([AlphabetColum]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO

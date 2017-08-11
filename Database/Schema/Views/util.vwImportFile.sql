SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE VIEW [util].[vwImportFile]
AS
SELECT [if].[ImportFileID]
     , [if].[FileName]
     , [if].[FileExtension]
	 , [if].[FileSize]
	 , [ift].[TypeName] FileType
	 , IIF([if].[Processed] = 1, 'True', 'False') [Processed]
     , [dtme].[udfGetLocalDateTime]([if].[CreatedOnUTC]) CreatedOnLocal
     , [dtme].[udfGetLocalDateTime]([if].[UpdatedOnUTC]) UpdatedOnLocal
FROM   [util].[ImportFile] AS [if] JOIN [util].[ImportFileType] AS [ift] ON [ift].[ImportFileTypeID] = [if].[ImportFileTypeID]

GO

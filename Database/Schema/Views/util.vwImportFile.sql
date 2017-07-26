SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [util].[vwImportFile]
AS
SELECT [if].[ImportFileID]
     , [if].[FileName]
     , [if].[FileExtension]
     , [if].[FileDescription]
     , [dtme].[udfGetLocalDateTime]([if].[CreatedOnUTC]) CreatedOnLocal
     , [dtme].[udfGetLocalDateTime]([if].[UpdatedOnUTC]) UpdatedOnLocal
FROM   [util].[ImportFile] AS [if]
GO

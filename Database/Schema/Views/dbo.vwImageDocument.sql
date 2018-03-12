SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwImageDocument]
AS
SELECT  [d].[DocumentID]
      , [d].[FileName]
      , [d].[Extension]
      , [d].[FileSize]
      , [d].[CreationTimeLocal]
      , [d].[LastAccessTimeLocal]
      , [d].[LastWriteTimeLocal]
      , [d].[DirectoryName]
      , [d].[FullFilePath]
      , [d].[FileUrl]
      , [d].[DocumentDate]
      , [d].[ByteCount]
      , [d].[Archived]
      , [d].[ModifiedByUserID]
      , [d].[FileTypeID]
      , [d].[CreatedOnUTC]
      , [d].[UpdatedOnUTC]
FROM    [dbo].[Document] AS [d]
WHERE   [d].[FileTypeID] = 1
GO

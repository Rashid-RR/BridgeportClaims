SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwDocumentIndex]
AS
SELECT          [d].[DocumentID]
              , [d].[FileName]
              , [d].[Extension]
              , [d].[FileSize]
              , [d].[CreationTimeLocal]
              , [d].[LastAccessTimeLocal]
              , [d].[LastWriteTimeLocal]
              , [d].[DirectoryName]
              , [d].[FullFilePath]
              , [d].[FileUrl]
			  , IsIndexed = CONVERT(BIT,CASE WHEN [di].[DocumentID] IS NOT NULL THEN 1 ELSE 0 END,0)
              , [di].[ClaimID]
              , [di].[DocumentTypeID]
              , [di].[RxDate]
              , [di].[RxNumber]
              , [di].[InvoiceNumber]
              , [di].[InjuryDate]
              , [di].[AttorneyName]
FROM            [dbo].[Document]      AS [d]
    LEFT JOIN   [dbo].[DocumentIndex] AS [di] ON [di].[DocumentID] = [d].[DocumentID]
GO

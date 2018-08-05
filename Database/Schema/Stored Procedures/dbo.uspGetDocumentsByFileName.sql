SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/22/2018
 Description:       Get all of the Documents that match a Document File Name 
 Example Execute:
                    EXECUTE [dbo].[uspGetDocumentsByFileName] 'BELIND'
 =============================================
*/
CREATE PROC [dbo].[uspGetDocumentsByFileName] (@FileName VARCHAR(1000))
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        DECLARE @WildCard CHAR(1) = '%'
        SELECT  DocumentId = d.DocumentID
               ,d.[FileName]
               ,d.Extension
               ,d.FileSize
               ,d.CreationTimeLocal
               ,d.LastAccessTimeLocal
               ,d.LastWriteTimeLocal
               ,d.FullFilePath
               ,d.FileUrl
        FROM    dbo.Document AS d
        WHERE   d.[FileName] LIKE CONCAT(@WildCard, @FileName, @WildCard);
    END
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       9/29/2019
 Description:       Retrieves the file bytes for the oldest (chronologically), unprocessed Envision File
 Example Execute:
                    EXECUTE [dbo].[uspGetOldestEnvisionFileBytes]
 =============================================
*/
CREATE PROCEDURE [dbo].[uspGetOldestEnvisionFileBytes]
AS BEGIN
    SET NOCOUNT ON;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;
	SET DEADLOCK_PRIORITY HIGH;
	SET XACT_ABORT ON;

	DECLARE @Top INT = 1;

	WITH OldestUnprocessedEnvisionFileCTE AS
	(
		SELECT TOP (@Top) i.[ImportFileID]
		FROM [util].[ImportFile] AS [i]
			 INNER JOIN [util].[ImportFileType] AS [ift] ON [ift].[ImportFileTypeID] = [i].[ImportFileTypeID]
		WHERE [ift].[Code] = 'EI'
			 AND i.[Processed] = 0
		ORDER BY TRY_CAST(SUBSTRING([i].[FileName], 15, 8) AS DATE) ASC
	)
	SELECT i.[FileName], i.FileBytes
	FROM OldestUnprocessedEnvisionFileCTE AS c
		 INNER JOIN util.ImportFile AS i ON [i].[ImportFileID] = [c].[ImportFileID]
	IF @@ROWCOUNT NOT IN (1, 0)
		RAISERROR(N'Error, more than one Envision File was found',16, 1) WITH NOWAIT  
END
GO

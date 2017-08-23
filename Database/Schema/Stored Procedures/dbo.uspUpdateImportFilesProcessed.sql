SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/21/2017
	Description:	
	Sample Execute:
					EXEC dbo.uspUpdateImportFilesProcessed N'Billing_Claim_File_20170821.csv'
*/
CREATE PROC [dbo].[uspUpdateImportFilesProcessed] @FileName NVARCHAR(255)
AS BEGIN
	SET NOCOUNT ON;

	UPDATE etl.LatestStagedLakerFileLoaded
	SET LastFileNameLoaded = (SELECT i.[FileName] FROM util.vwImportFile AS i WHERE i.[FileName] = @FileName)

	DECLARE @LatestDate DATE
	SELECT @LatestDate = CAST(SUBSTRING(i.LastFileNameLoaded, 20, 8) AS DATE)
	FROM   etl.LatestStagedLakerFileLoaded AS i
	IF @LatestDate IS NULL
		BEGIN
			PRINT 'The @LatestDate variable could not be populated. Exiting routine...'
			RETURN
		END;
	WITH UpdateProcessedFilesCTE AS
    (
		SELECT i.ImportFileID
		FROM   util.vwImportFile AS i
		WHERE  i.Processed = 0
			   AND i.FileName LIKE 'Billing_Claim_File_%'
	)
	UPDATE i SET i.Processed = 1
	FROM   util.ImportFile AS i 
		   INNER JOIN UpdateProcessedFilesCTE c ON c.ImportFileID = i.ImportFileID
	WHERE  i.Processed = 0
		   AND CAST(SUBSTRING(i.FileName, 20, 8) AS DATE) <= @LatestDate
END
GO

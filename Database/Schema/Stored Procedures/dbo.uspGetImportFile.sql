SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/3/2017
	Description:	Select the DTO fields from the ImportFile table.
	Sample Execute:
					EXEC dbo.uspGetImportFile
*/
CREATE PROC [dbo].[uspGetImportFile]
AS
    BEGIN
        SET NOCOUNT ON;
        SELECT [i].[ImportFileID] ImportFileId
             , [i].[FileName]
             , [i].[FileExtension]
             , [i].[FileSize]
             , [i].[FileType]
             , [i].[Processed]
             , [i].[CreatedOnLocal]
			 , [FileDate] = CASE WHEN i.FileTypeCode = 'LI' THEN TRY_CONVERT(DATE, REPLACE(REPLACE(i.[FileName], 'Billing_Claim_File_', ''), '.csv', ''))
								 WHEN i.FileTypeCode = 'EI' THEN TRY_CONVERT(DATE, SUBSTRING(i.[FileName], 15, 8)) END
        FROM   [util].[vwImportFile] AS [i]
    END
GO
GRANT EXECUTE ON  [dbo].[uspGetImportFile] TO [bridgeportclaimslakerimporter]
GO

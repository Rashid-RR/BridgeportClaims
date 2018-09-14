SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       9/13/2018
 Description:       Gets the oldest, non-processed Laker file to process.
 Example Execute:
                    EXECUTE [dbo].[uspGetOldestLakerFileName] 'Billing_Claim_File_'
 =============================================
*/
CREATE PROC [dbo].[uspGetOldestLakerFileName]
(
	@FileNameStartsWith NVARCHAR(255)
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	DECLARE @WildCard CHAR(1) = '%';
	DECLARE @One INT = 1;
	SELECT	TOP (@One) x.[FileName]
	FROM    util.vwImportFile AS x
	WHERE   x.Processed = 0
			AND x.[FileName] LIKE CONCAT(@FileNameStartsWith, @WildCard)
	ORDER BY x.CreatedOnLocal ASC;
END
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	9/4/2017
	Description:	Retrieves the file bytes for the oldest (chronologically), unprocessed Laker File
	Sample Execute:
					EXEC dbo.uspGetOldestLakerFileBytes
*/
CREATE PROC [dbo].[uspGetOldestLakerFileBytes]
AS BEGIN
	SET NOCOUNT ON;
	SET TRAN ISOLATION LEVEL SERIALIZABLE
	SET DEADLOCK_PRIORITY HIGH
	SET XACT_ABORT ON;

	DECLARE @Top INT = 1;

	WITH OldestUnprocessedFileCTE AS
	(
		SELECT TOP (@Top) SubstringDate=CAST(SUBSTRING(i.[FileName], 20, 8) AS DATE)
		FROM util.ImportFile AS i
		WHERE 1 = 1 
			AND ISNUMERIC(SUBSTRING(i.[FileName], 20, 8)) = 1 
			AND i.Processed=0
			AND i.[FileName] LIKE N'Billing_Claim_File_%'
		ORDER BY SubstringDate ASC
	)
	SELECT i.[FileName], i.FileBytes
	FROM OldestUnprocessedFileCTE AS c
		 INNER JOIN util.ImportFile AS i ON FORMAT(c.SubstringDate, 'yyyyMMdd') =
		 SUBSTRING(i.[FileName], 20, 8)
	IF @@ROWCOUNT NOT IN (1, 0)
		RAISERROR(N'Error, more than one Laker File was found',16, 1) WITH NOWAIT
END
GO

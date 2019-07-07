SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/6/2019
	Description:	Retrieves the file bytes for specified Envision file.
	Sample Execute:
					EXEC [dbo].[uspGetEnvisionFileBytes] 770
*/
CREATE PROC [dbo].[uspGetEnvisionFileBytes]
(
	@ImportFileID INT
)
AS BEGIN
	SET NOCOUNT ON;
	SET TRAN ISOLATION LEVEL SERIALIZABLE
	SET DEADLOCK_PRIORITY HIGH
	SET XACT_ABORT ON;
	DECLARE @ImportFileTypeID INT = [util].[udfGetImportFileTypeIDByCode]('EI')

	SELECT  x.[FileName],
            x.FileBytes
	FROM	util.ImportFile AS x
	WHERE	x.ImportFileID = @ImportFileID
			AND x.Processed = 0
			AND x.ImportFileTypeID = @ImportFileTypeID;
END
GO

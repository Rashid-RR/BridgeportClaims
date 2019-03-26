SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       3/23/2019
 Description:       Gets rid of all of the previous import file prior to the latest successful one.
 Example Execute:
                    EXECUTE [util].[uspCleanupImportFiles]
 =============================================
*/
CREATE   PROCEDURE [util].[uspCleanupImportFiles]
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @ImportFileID INT;
    SELECT @ImportFileID = MAX(vif.ImportFileID)
    FROM util.vwImportFile AS vif
    WHERE vif.Processed = 1;

    DELETE x
    FROM util.ImportFile AS x
    WHERE x.ImportFileID < @ImportFileID;
END;

GO

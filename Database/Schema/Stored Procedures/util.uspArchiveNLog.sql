SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       10/6/2018
 Description:       Dangerous Proc! Moves ALL of the rows from the util.NLog table into the util.ArchivedNLog,
					truncated the destination table first, and truncating the source table last.
 Example Execute:
                    EXECUTE [util].[uspArchiveNLog]
 =============================================
*/
CREATE PROC [util].[uspArchiveNLog]
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	SET DEADLOCK_PRIORITY HIGH;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRY
        BEGIN TRAN;
        
		-- Cleanup Archived
		TRUNCATE TABLE util.ArchivedNLog;

		DECLARE @TotalRows BIGINT, @RowCount BIGINT;

		SELECT @TotalRows = COUNT_BIG(*) FROM util.NLog AS nl
		    
        INSERT util.ArchivedNLog (NLogID,MachineName,SiteName,Logged,[Level],UserName,[Message],Logger,Properties,ServerName,[Port],[Url]
			,Https,ServerAddress,RemoteAddress,Callsite,Exception)
		SELECT NLogID,MachineName,SiteName,Logged,[Level],UserName,[Message],Logger,Properties,ServerName,[Port],[Url]
			,Https,ServerAddress,RemoteAddress,Callsite,Exception
		FROM util.NLog AS nl
		SET @RowCount = @@ROWCOUNT;

		IF (ISNULL(@RowCount, 0) != ISNULL(@TotalRows, 0))
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				RAISERROR(N'Error, the count of rows in the util.NLog table did not match the row(s) archived.', 16, 1) WITH NOWAIT;
				RETURN -1;
			END

		-- Cleanup NLog
		TRUNCATE TABLE util.NLog;

        IF (@@TRANCOUNT > 0)
            COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
            ROLLBACK;
                
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

        RAISERROR(N'%s (line %d): %s', @ErrSeverity, @ErrState, @ErrProc, @ErrLine, @ErrMsg);
    END CATCH
END
GO

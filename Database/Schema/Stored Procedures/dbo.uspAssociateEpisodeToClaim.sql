SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/8/2018
 Description:       Updates an Episode to tie it to a Claim. 
 Example Execute:
                    EXECUTE [dbo].[uspAssociateEpisodeToClaim] 7800, 1
 =============================================
*/
CREATE PROC [dbo].[uspAssociateEpisodeToClaim]
(
	@EpisodeID INTEGER,
	@ClaimID INTEGER
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
            
		DECLARE @PrntMsg NVARCHAR(1000), @RowCount INTEGER;

        IF NOT EXISTS (SELECT * FROM dbo.Episode AS e WHERE e.EpisodeID = @EpisodeID)
			BEGIN
				IF (@@TRANCOUNT > 0) ROLLBACK;
				SET @PrntMsg = N'Error, Episode Id ' + CONVERT(NVARCHAR(20), @EpisodeID) + N' does not exist.';
				RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
				RETURN -1;
			END

		-- Proceed.
		UPDATE dbo.Episode SET ClaimID = @ClaimID WHERE EpisodeID = @EpisodeID;
		SET @RowCount = @@ROWCOUNT;

		IF (@RowCount != 1)
			BEGIN
				IF (@@TRANCOUNT > 0) ROLLBACK;
				SET @PrntMsg = N'Error, Episode Id ' + CONVERT(NVARCHAR(20), @EpisodeID) + N' was not updated correctly with Claim Id ' + CONVERT(NVARCHAR(20), @ClaimID);
				RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
				RETURN -1;
			END
    
        IF (@@TRANCOUNT > 0)
            COMMIT;

		RETURN @RowCount;

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

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/15/2018
 Description:       Makes an assignment, or an acquisition of an Episode by a user. 
 Example Execute:
                    EXECUTE [dbo].[uspAssignOrAcquireEpisode]
 =============================================
*/
CREATE PROC [dbo].[uspAssignOrAcquireEpisode]
(
	@EpisodeID INT,
	@UserID NVARCHAR(128),
	@ModifiedByUserID NVARCHAR(128)
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
            
		DECLARE @UtcNow DATETIME2 = dtme.udfGetDate();

        UPDATE  e
		SET		e.AcquiredUserID = CASE WHEN e.AcquiredUserID IS NULL THEN @UserID ELSE e.AcquiredUserID END
				,e.AssignedUserID = @UserID
				,e.ModifiedByUserID = @ModifiedByUserID
				,e.UpdatedOnUTC = @UtcNow
		FROM    dbo.Episode AS e
		WHERE   e.EpisodeID = @EpisodeID;
            
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

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/15/2018
 Description:       Inserts a new Episode Note
 Example Execute:
                    EXECUTE [dbo].[uspEpisodeNoteInsert]
 =============================================
*/
CREATE PROC [dbo].[uspEpisodeNoteInsert]
(
	@EpisodeID INT,
	@NoteText VARCHAR(8000),
	@UserID NVARCHAR(128),
	@Today DATE
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
            
		DECLARE @UtcNow DATETIME2 = dtme.udfGetDate();

        INSERT dbo.EpisodeNote (EpisodeID, NoteText, WrittenByUserID, Created, CreatedOnUTC, UpdatedOnUTC)
        VALUES (@EpisodeID, @NoteText, @UserID, @Today, @UtcNow, @UtcNow);
            
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

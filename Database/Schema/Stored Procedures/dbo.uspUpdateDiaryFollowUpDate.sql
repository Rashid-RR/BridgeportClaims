SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		5/5/2018
 Description:		Updates the Follow-Up date to a Diary
 Example Execute:
					EXECUTE [dbo].[uspUpdateDiaryFollowUpDate]
 =============================================
*/
CREATE PROC [dbo].[uspUpdateDiaryFollowUpDate] @DiaryID INT, @FollowUpDate DATE
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
			
		UPDATE	[d] SET [d].[FollowUpDate] = @FollowUpDate
		FROM    [dbo].[Diary] AS [d]
		WHERE   [d].[DiaryID] = @DiaryID

		IF (@@ROWCOUNT != 1)
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				RAISERROR(N'Error, the Diary updated count was not one.', 16, 1) WITH NOWAIT;
				RETURN -1;
			END
			
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

		RAISERROR(N'%s (line %d): %s',	-- Message text w formatting
			@ErrSeverity,		-- Severity
			@ErrState,			-- State
			@ErrProc,			-- First argument (string)
			@ErrLine,			-- Second argument (int)
			@ErrMsg);			-- First argument (string)
    END CATCH
END
GO

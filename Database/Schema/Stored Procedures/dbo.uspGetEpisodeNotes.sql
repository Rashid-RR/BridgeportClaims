SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		2/7/2018
 Description:		Gets the Episode Note(s) for the Episode Notes Modal
 Example Execute:
					EXECUTE [dbo].[uspGetEpisodeNotes] 2718
 =============================================
*/
CREATE PROC [dbo].[uspGetEpisodeNotes]
(
	@EpisodeID INTEGER
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
		DECLARE @Space VARCHAR(5) = ' ';
		SELECT          [en].[EpisodeID] Id, CONCAT(own.[FirstName], @Space,own.[LastName]) [Owner],
						e.[Created] EpisodeCreated, CONCAT(p.[FirstName], @Space, p.[LastName]) PatientName, c.[ClaimNumber],
						CONCAT(u.[FirstName], @Space, u.[LastName]) WrittenBy, en.[Created] NoteCreated, en.[NoteText]
		FROM            [dbo].[EpisodeNote] AS [en]
			INNER JOIN [dbo].[Episode] AS [e] ON [e].[EpisodeID] = [en].[EpisodeID]
			LEFT JOIN [dbo].[Claim] AS [c] ON [c].[ClaimID] = [e].[ClaimID]
			LEFT JOIN [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
			LEFT JOIN [dbo].[AspNetUsers] AS own ON own.[ID] = e.[AssignedUserID]
			LEFT JOIN  [dbo].[AspNetUsers] AS [u] ON [u].[ID] = [en].[WrittenByUserID]
		WHERE           [en].[EpisodeID] = @EpisodeID
			
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

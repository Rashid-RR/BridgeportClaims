SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		2/11/2018
 Description:		Saves a new Episode and Episode Note
 Example Execute:
					EXECUTE [dbo].[uspSaveNewEpisode]
 =============================================
*/
CREATE   PROC [dbo].[uspSaveNewEpisode]
(
	@ClaimID INTEGER,
	@EpisodeTypeID TINYINT,
	@PharmacyNABP VARCHAR(7),
	@RxNumber VARCHAR(100),
	@NoteText VARCHAR(8000),
	@UserID NVARCHAR(128)
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
		DECLARE @UtcNow DATETIME2 = SYSUTCDATETIME(),
				@Today DATE = CONVERT(DATE, [dtme].[udfGetLocalDate]());
		
		DECLARE @EpisodeCategoryID INTEGER, @EpisodeID INTEGER

		SELECT  @EpisodeCategoryID = [ec].[EpisodeCategoryID]
		FROM    [dbo].[EpisodeCategory] AS [ec]
		WHERE   [ec].[Code] = 'CALL'

		INSERT INTO [dbo].[Episode]
		(   [ClaimID]
		  , [EpisodeTypeID]
		  , [AcquiredUserID]
		  , [AssignedUserID]
		  , [RxNumber]
		  , [Created]
		  , [PharmacyNABP]
		  , [ModifiedByUserID]
		  , [EpisodeCategoryID]
		  , [CreatedOnUTC]
		  , [UpdatedOnUTC])
		SELECT  @ClaimID
			  , @EpisodeTypeID
			  , @UserID
			  , @UserID
			  , @RxNumber
			  , @Today
			  , @PharmacyNABP
			  , @UserID
			  , @EpisodeCategoryID
			  , @UtcNow
			  , @UtcNow
		SET @EpisodeID = SCOPE_IDENTITY();
		INSERT INTO [dbo].[EpisodeNote]
		(   [EpisodeID]
		  , [NoteText]
		  , [WrittenByUserID]
		  , [Created]
		  , [CreatedOnUTC]
		  , [UpdatedOnUTC])
		SELECT @EpisodeID,@NoteText,@UserID,@Today,@UtcNow,@UtcNow

		SELECT  [ve].[Id]
              , [ve].[Created]
              , [ve].[Owner]
              , [ve].[Type]
              , [ve].[Role]
              , [ve].[Pharmacy]
              , [ve].[RxNumber]
              , [ve].[Category]
              , [ve].[Resolved]
              , [ve].[NoteCount]
		FROM    [dbo].[vwEpisode] AS [ve]
		WHERE   [ve].[Id] = @EpisodeID
		
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

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		2/3/2018
 Description:		Saves a new "Image" category Episode record if certain
					criteria are met - which can be found below.
 Example Execute:
					EXECUTE [dbo].[uspCreateImageCategoryEpisode]
 =============================================
*/
CREATE PROCEDURE [dbo].[uspCreateImageCategoryEpisode]
(
	@DocumentTypeID TINYINT,
	@ClaimID INTEGER,
	@RxNumber VARCHAR(100),
	@UserID NVARCHAR(128),
	@DocumentID INTEGER,
	@EpisodeCreated BIT OUTPUT
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

		DECLARE @CreateEpisode BIT, 
				@UtcNow DATETIME2 = SYSUTCDATETIME(),
				@LocalNow DATE = CONVERT(DATE, [dtme].[udfGetLocalDate]()),
				@PrntMsg NVARCHAR(500),
				@EpisodeID INTEGER;

		IF NOT EXISTS (SELECT * FROM [dbo].[Claim] AS [c] WHERE [c].[ClaimID] = @ClaimID)
			BEGIN
				SET @PrntMsg = N'Error, Claim ID ' + CONVERT(NVARCHAR, @ClaimID) + ' doesn''t exist.';
				RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				RETURN;
			END

		DECLARE @EpisodeCategoryID INTEGER
		SELECT  @EpisodeCategoryID = [ec].[EpisodeCategoryID]
		FROM    [dbo].[EpisodeCategory] AS [ec]
		WHERE   [ec].[Code] = 'IMAGE'
			
		SELECT  @CreateEpisode = CAST(COUNT(*) AS BIT)
		FROM    [dbo].[DocumentType] AS [dt]
		WHERE   [dt].[DocumentTypeID] = @DocumentTypeID
				AND [dt].[CreatesEpisode] = 1

		IF @CreateEpisode = 0
			BEGIN
				SET @EpisodeCreated = 0;
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				RAISERROR(N'No action necessary', 1, 1) WITH NOWAIT;
				RETURN;
			END
		ELSE
			BEGIN
				IF EXISTS (
							SELECT  *
							FROM    [dbo].[Episode] AS [e]
							WHERE   [e].[DocumentID] = @DocumentID
						  )
					BEGIN
						SELECT  @EpisodeID = [e].[EpisodeID]
						FROM    [dbo].[Episode] AS [e]
						WHERE   [e].[DocumentID] = @DocumentID

						IF (@@TRANCOUNT > 0)
							ROLLBACK;
						SET @PrntMsg = N'Episode Id ' + CONVERT(NVARCHAR, @EpisodeID) + 
									' has already been created for Document Id ' + CONVERT(NVARCHAR, @DocumentID)
						RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
						RETURN -1;
					END

				SET @EpisodeCreated = 1;
				INSERT INTO [dbo].[Episode]
				(   [ClaimID]
				  , [EpisodeTypeID]
				  , [AssignedUserID]
				  , [RxNumber]
				  , [Created]
				  , [DocumentID]
				  , [ModifiedByUserID]
				  , [EpisodeCategoryID]
				  , [CreatedOnUTC]
				  , [UpdatedOnUTC])
				SELECT  @ClaimID
					  ,(SELECT  [m].[EpisodeTypeID]
						FROM    dbo.[DocumentTypeEpisodeTypeMapping] AS [m]
						WHERE   [m].[DocumentTypeID] = @DocumentTypeID)
					  , NULL
					  , @RxNumber
					  , @LocalNow
					  , @DocumentID
					  , @UserID
					  , @EpisodeCategoryID
					  , @UtcNow
					  , @UtcNow
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

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
CREATE PROC [dbo].[uspSaveNewEpisode]
(
	@ClaimID INTEGER,
	@EpisodeTypeID TINYINT,
	@PharmacyNABP VARCHAR(7),
	@RxNumber VARCHAR(100),
	@NoteText VARCHAR(8000),
	@UserID NVARCHAR(128),
	@DecisionTreeChoiceID INT = NULL
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
		DECLARE @UtcNow DATETIME2 = SYSUTCDATETIME(),
				@Today DATE = CONVERT(DATE, [dtme].[udfGetLocalDate]());
		
		DECLARE @EpisodeCategoryID INTEGER, @EpisodeID INTEGER
		SET @EpisodeCategoryID = [dbo].[udfGetEpisodeCategoryFromCode]('CALL');

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
		  , [DecisionTreeChoiceID]
		  , [CreatedOnUTC]
		  , [UpdatedOnUTC])
		SELECT @EpisodeID,@NoteText,@UserID,@Today,@DecisionTreeChoiceID,@UtcNow,@UtcNow

		SELECT  [ve].[Id]
              , [ve].[Created]
              , [ve].[Owner]
              , [ve].[Type]
              , [ve].[Pharmacy]
              , [ve].[RxNumber]
              , [ve].[Resolved]
              , [ve].[NoteCount]
			  , [ve].[HasTree]
		FROM    dbo.vwEpisodeBlade AS [ve]
		WHERE   [ve].[Id] = @EpisodeID
		
		IF (@@TRANCOUNT > 0)
			COMMIT;
    END TRY
    BEGIN CATCH     
		IF (@@TRANCOUNT > 0)
			ROLLBACK;	
		DECLARE @ErrLine INT = ERROR_LINE()
              , @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
		DECLARE @Msg NVARCHAR(2000) = FORMATMESSAGE(N'An error occurred: %s Line Number: %u', @ErrMsg, @ErrLine);
		THROW 50000, @Msg, 0;
    END CATCH
END

GO

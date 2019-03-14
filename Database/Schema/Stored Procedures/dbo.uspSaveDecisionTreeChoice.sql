SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		3/12/2019
 Description:		Saves a new Episode, Episode Note, and Decision Tree Choice
 Example Execute:
					EXECUTE [dbo].[uspSaveDecisionTreeChoice]
 =============================================
*/
CREATE PROCEDURE [dbo].[uspSaveDecisionTreeChoice]
(
	@LeafTreeID INT,
	@ClaimID INT,
	@EpisodeTypeID TINYINT,
	@PharmacyNABP VARCHAR(7),
	@RxNumber VARCHAR(100),
	@EpisodeText VARCHAR(8000),
	@ModifiedByUserID NVARCHAR(128)
)
AS BEGIN
	DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();
	DECLARE @DecisionTreeChoiceID INT;
	INSERT [dbo].[DecisionTreeChoice]
	(
	    [ClaimID]
	  , [LeafTreeID]
	  , [ModifiedByUserID]
	  , [CreatedOnUTC]
	  , [UpdatedOnUTC]
	)
	VALUES
	     (@ClaimID
	    , @LeafTreeID
	    , @ModifiedByUserID
	    , @UtcNow
	    , @UtcNow
	    );
	SET @DecisionTreeChoiceID = SCOPE_IDENTITY();
	EXEC [dbo].[uspSaveNewEpisode] @ClaimID = @ClaimID
                             , @EpisodeTypeID = @EpisodeTypeID
                             , @PharmacyNABP = @PharmacyNABP
                             , @RxNumber = @RxNumber
                             , @NoteText = @EpisodeText
                             , @UserID = @ModifiedByUserID
                             , @DecisionTreeChoiceID = @DecisionTreeChoiceID;
END
GO

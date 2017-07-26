SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/7/2017
	Description:	Adds or Inserts an Episode.
	Sample Execute:
					EXEC [dbo].[uspSaveEpisode] 15, 95, '3/15/2017', 'hello!!!!!!!!!!!!!!!!!!!!!!', 'yo yo y oyo '
*/
CREATE PROC [dbo].[uspSaveEpisode]
(
	@EpisodeID INT,
	@ClaimID INT,
	@CreatedDateUTC DATETIME2,
	@AssignUser VARCHAR(100),
	@Note VARCHAR(1000)
)
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @UTCNow DATETIME2 = SYSUTCDATETIME()
	IF @ClaimID IS NULL
		BEGIN
			RAISERROR(N'Error. The ClaimID is a required arguement, and was not supplied', 16, 1) WITH NOWAIT
			RETURN
		END
	IF @EpisodeID IS NOT NULL AND NOT EXISTS
	(
		SELECT * FROM [dbo].[Episode] AS [e] WHERE [e].[EpisodeID] = @EpisodeID
	)
		BEGIN
			RAISERROR(N'Error. The EpisodeID that you passsed in does not exist.', 16, 1) WITH NOWAIT
			RETURN
		END
	-- Upsert Episode
	MERGE [dbo].[Episode] AS tgt
	USING (SELECT @EpisodeID EpisodeID,
				  @ClaimID ClaimID,
				  @CreatedDateUTC CreatedDateUTC,
				  @AssignUser AssignUser,
				  @Note Note) AS src
	       ON [tgt].[EpisodeID] = [src].[EpisodeID]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([ClaimID], [CreatedDateUTC], [AssignUser], [Note])
		VALUES (src.[ClaimID], src.[CreatedDateUTC], src.[AssignUser], src.[Note])
	WHEN MATCHED THEN
		UPDATE SET  [tgt].[ClaimID] = [src].[ClaimID],
					[tgt].[CreatedDateUTC] = [src].[CreatedDateUTC],
					[tgt].[AssignUser] = [src].[AssignUser],
					[tgt].[Note] = [src].[Note],
					[tgt].[UpdatedOnUTC] = @UTCNow;
END

GO

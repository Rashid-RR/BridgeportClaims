SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE   FUNCTION [dbo].[udfGetEpisodeCategoryFromCode](@Code VARCHAR(50))
RETURNS INTEGER
AS BEGIN
	RETURN
		(
			SELECT  [ec].[EpisodeCategoryID]
			FROM    [dbo].[EpisodeCategory] AS [ec]
			WHERE   [ec].[Code] = @Code
		)
END

GO

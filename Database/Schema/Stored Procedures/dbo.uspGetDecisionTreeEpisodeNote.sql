SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Date:			3/15/2019
	Description:	Returns the data for the Decision Tree Modal
	Example Execute:
					EXECUTE [dbo].[uspGetDecisionTreeEpisodeNote] 14370
*/
CREATE PROCEDURE [dbo].[uspGetDecisionTreeEpisodeNote] @EpisodeID AS INT
AS BEGIN
	DECLARE @LeafTreeID INT;
	SELECT	@LeafTreeID = d.LeafTreeID
	FROM	dbo.DecisionTreeChoice AS d
			INNER JOIN dbo.EpisodeNote AS en ON en.DecisionTreeChoiceID = d.DecisionTreeChoiceID
	WHERE	en.EpisodeID = @EpisodeID;

    SELECT e.EpisodeID EpisodeId,
           u.FirstName + ' ' + u.LastName CreatedBy,
           en.NoteText EpisodeNote,
           en.Created
    FROM dbo.Episode AS e
        INNER JOIN dbo.EpisodeNote AS en ON en.EpisodeID = e.EpisodeID
        INNER JOIN dbo.DecisionTreeChoice AS dtc ON dtc.DecisionTreeChoiceID = en.DecisionTreeChoiceID
        INNER JOIN dbo.AspNetUsers AS u ON u.ID = dtc.ModifiedByUserID
    WHERE e.EpisodeID = @EpisodeID;

	SELECT upl.TreeLevel, upl.NodeName FROM dbo.udfGetUpline(@LeafTreeID) AS upl
END;
GO

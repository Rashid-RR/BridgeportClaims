SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwEpisode]
AS
SELECT e.EpisodeID, e.ClaimID, e.Note, EpisodeType=et.TypeName, e.Role, e.Type,
    ResolvedUser=u.FirstName+' '+u.LastName, AcquiredUser=uu.FirstName+' '+uu.LastName,
    AssignedUserID=uuu.FirstName+' '+uuu.LastName, e.RxNumber, e.Status, e.CreatedDateUTC,
    e.Description, e.ResolvedDate, e.CreatedOnUTC, e.UpdatedOnUTC
FROM dbo.Episode AS e
     LEFT JOIN dbo.EpisodeType AS et ON et.EpisodeTypeID=e.EpisodeTypeID
     LEFT JOIN dbo.AspNetUsers AS u ON e.ResolvedUser=u.ID
     LEFT JOIN dbo.AspNetUsers AS uu ON e.AcquiredUser=uu.ID
     LEFT JOIN dbo.AspNetUsers AS uuu ON e.AssignedUserID=uuu.ID
GO

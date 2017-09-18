SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwEpisode]
AS
SELECT e.EpisodeID, e.ClaimID, e.Note, EpisodeType=et.TypeName, e.[Role],
    ResolvedUser=u.FirstName+' '+u.LastName, AcquiredUser=uu.FirstName+' '+uu.LastName,
    AssignedUser=uuu.FirstName+' '+uuu.LastName, e.RxNumber, e.[Status],
	dtme.udfGetLocalDateTime(e.CreatedDateUTC) CreatedDateLocal,
    e.[Description], dtme.udfGetLocalDateTime(e.ResolvedDateUTC) ResolvedDateLocal,
	dtme.udfGetLocalDateTime(e.CreatedOnUTC) CreatedOnLocal,
	dtme.udfGetLocalDateTime(e.UpdatedOnUTC) UpdatedOnLocal
FROM dbo.Episode AS e
     LEFT JOIN dbo.EpisodeType AS et ON et.EpisodeTypeID=e.EpisodeTypeID
     LEFT JOIN dbo.AspNetUsers AS u ON e.ResolvedUserID=u.ID
     LEFT JOIN dbo.AspNetUsers AS uu ON e.AcquiredUserID=uu.ID
     LEFT JOIN dbo.AspNetUsers AS uuu ON e.AssignedUserID=uuu.ID
GO

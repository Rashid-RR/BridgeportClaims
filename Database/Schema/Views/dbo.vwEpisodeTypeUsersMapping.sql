SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE   VIEW [dbo].[vwEpisodeTypeUsersMapping]
AS
SELECT  [e].[EpisodeTypeID]
      , [et].[TypeName] EpisodeTypeName
	  , [u].[FirstName] + ' ' + [u].[LastName] UserName
	  , u.[ID] AspNetUserID
      , [e].[CreatedOnUTC]
      , [e].[UpdatedOnUTC]
FROM    [dbo].[EpisodeTypeUsersMapping] AS [e]
	INNER JOIN [dbo].[EpisodeType] AS [et] ON [et].[EpisodeTypeID] = [e].[EpisodeTypeID]
	INNER JOIN [dbo].[AspNetUsers] AS [u] ON u.[ID] = e.[UserID]
GO

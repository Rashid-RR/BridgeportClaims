SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwEpisode]
AS
SELECT          Id        = e.[EpisodeID]
              , e.[Created]
              , [Owner]   = u.[FirstName] + ' ' + u.[LastName]
              , [Type]    = et.[TypeName]
              , e.[Role]
              , Pharmacy  = ph.[PharmacyName]
              , e.[RxNumber]
              , Category  = ec.[CategoryName]
              , Resolved  = CONVERT(BIT, IIF(e.[ResolvedDateUTC] IS NOT NULL, 1, 0))
              , NoteCount =
                (   SELECT  COUNT(*)
                    FROM    [dbo].[EpisodeNote] AS [en]
                    WHERE   en.[EpisodeID] = e.[EpisodeID])
			  , e.[ClaimID]
			  , et.[EpisodeTypeID]
FROM            [dbo].[Episode]         AS [e]
	INNER JOIN  [dbo].[EpisodeType]     AS [et] ON [et].[EpisodeTypeID] = [e].[EpisodeTypeID]
    INNER JOIN  [dbo].[Claim]           AS [c] ON [c].[ClaimID] = [e].[ClaimID]
    INNER JOIN  [dbo].[Patient]         AS [p] ON [p].[PatientID] = [c].[PatientID]
    INNER JOIN  [dbo].[EpisodeCategory] AS [ec] ON [ec].[EpisodeCategoryID] = [e].[EpisodeCategoryID]
    LEFT JOIN   [dbo].[AspNetUsers]     AS [u] ON [u].[ID] = [e].[AssignedUserID]
    LEFT JOIN   [dbo].[Pharmacy]        AS [ph] ON [ph].[NABP] = [e].[PharmacyNABP]
GO

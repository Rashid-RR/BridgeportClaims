SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwDocumentTypeEpisodeTypes]
AS
SELECT dt.[DocumentTypeID], dt.[TypeName] DocumentTypeName, et.[EpisodeTypeID], et.[TypeName] EpisodeTypeName FROM [dbo].[DocumentTypeEpisodeTypeMapping] AS [m]
INNER JOIN [dbo].[DocumentType] AS [dt] ON [dt].[DocumentTypeID] = [m].[DocumentTypeID]
INNER JOIN [dbo].[EpisodeType] AS [et] ON [et].[EpisodeTypeID] = [m].[EpisodeTypeID]
GO

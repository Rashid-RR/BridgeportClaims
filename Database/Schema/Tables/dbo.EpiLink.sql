CREATE TABLE [dbo].[EpiLink]
(
[EpiLinkID] [int] NOT NULL,
[LinkType] [nvarchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LinkTransNbr] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EpisodeNumber] [int] NULL
) ON [PRIMARY]
GO

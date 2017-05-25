CREATE TABLE [dbo].[Episode]
(
[epiId] [int] NOT NULL,
[epiClaimID] [int] NULL,
[epiEpisodeNum] [int] NULL,
[epiNote] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[epiRole] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[epiType] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[epiResolvedUser] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[epiAcquiredUser] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[epiAssignUser] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[epiRxNum] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[epiStatus] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[epiCreatedDate] [datetime] NULL,
[epiDescription] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[epiResolvedDate] [datetime] NULL
) ON [PRIMARY]
GO

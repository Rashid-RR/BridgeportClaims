CREATE TABLE [dbo].[Diary]
(
[DiaryID] [int] NOT NULL IDENTITY(1, 1),
[NoteText] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DiaryTypeID] [int] NOT NULL,
[EnteredByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ClaimID] [int] NOT NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfDiaryCreatedOnUTC] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfDiaryUpdatedOnUTC] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Diary] ADD CONSTRAINT [pkDiary] PRIMARY KEY CLUSTERED  ([DiaryID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDiaryEnteredByUserIDClaimIDDiaryTypeIDIncludeAll] ON [dbo].[Diary] ([EnteredByUserID], [ClaimID], [DiaryTypeID]) INCLUDE ([DiaryID], [NoteText]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Diary] ADD CONSTRAINT [FK__Diary__EnteredBy__1466F737] FOREIGN KEY ([EnteredByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[Diary] ADD CONSTRAINT [FK__Diary__ClaimID__155B1B70] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[Diary] ADD CONSTRAINT [FK__Diary__DiaryType__1372D2FE] FOREIGN KEY ([DiaryTypeID]) REFERENCES [dbo].[DiaryType] ([DiaryTypeID])
GO

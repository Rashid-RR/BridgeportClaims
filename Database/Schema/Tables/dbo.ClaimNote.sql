CREATE TABLE [dbo].[ClaimNote]
(
[ClaimNoteID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NOT NULL,
[ClaimNoteTypeID] [int] NOT NULL,
[NoteText] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EnteredByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfClaimNoteCreatedOnUTC] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfClaimNoteUpdatedOnUTC] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[ClaimNote] ADD CONSTRAINT [pkClaimNote] PRIMARY KEY CLUSTERED  ([ClaimNoteID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimNoteClaimIDClaimClaimID] ON [dbo].[ClaimNote] ([ClaimID], [ClaimNoteTypeID], [EnteredByUserID]) INCLUDE ([ClaimNoteID], [CreatedOn], [NoteText], [UpdatedOn]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ClaimNote] ADD CONSTRAINT [fkClaimNoteClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[ClaimNote] ADD CONSTRAINT [fkClaimNoteClaimNoteTypeIDClaimNoteTypeClaimNoteTypeID] FOREIGN KEY ([ClaimNoteTypeID]) REFERENCES [dbo].[ClaimNoteType] ([ClaimNoteTypeID])
GO
ALTER TABLE [dbo].[ClaimNote] ADD CONSTRAINT [fkClaimNoteEnteredByUserIDAspNetUsersID] FOREIGN KEY ([EnteredByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO

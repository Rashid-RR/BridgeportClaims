CREATE TABLE [dbo].[ClaimNote]
(
[ClaimNoteID] [int] NOT NULL IDENTITY(1, 1),
[ClaimNoteTypeID] [int] NOT NULL,
[NoteText] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EnteredByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfClaimNoteCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfClaimNoteUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ClaimNote] ADD CONSTRAINT [pkClaimNote] PRIMARY KEY CLUSTERED  ([ClaimNoteID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ClaimNote] ADD CONSTRAINT [fkClaimNoteClaimNoteTypeIDClaimNoteTypeClaimNoteTypeID] FOREIGN KEY ([ClaimNoteTypeID]) REFERENCES [dbo].[ClaimNoteType] ([ClaimNoteTypeID])
GO
ALTER TABLE [dbo].[ClaimNote] ADD CONSTRAINT [fkClaimNoteEnteredByUserIDAspNetUsersID] FOREIGN KEY ([EnteredByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO

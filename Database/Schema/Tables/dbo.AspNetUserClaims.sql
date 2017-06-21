CREATE TABLE [dbo].[AspNetUserClaims]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ClaimType] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ClaimValue] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[AspNetUserClaims] ADD CONSTRAINT [pkAspNetUserClaims] PRIMARY KEY CLUSTERED  ([ID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxAspNetUserClaimsUserIDIncludeAll] ON [dbo].[AspNetUserClaims] ([UserID]) INCLUDE ([ClaimType], [ClaimValue], [ID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUserClaims] ADD CONSTRAINT [fkAspNetUserClaimsUserIDAspNetUsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID]) ON DELETE CASCADE
GO

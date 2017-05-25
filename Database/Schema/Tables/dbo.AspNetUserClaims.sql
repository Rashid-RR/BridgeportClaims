CREATE TABLE [dbo].[AspNetUserClaims]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ClaimType] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ClaimValue] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUserClaims] ADD CONSTRAINT [pkAspNetUserClaims] PRIMARY KEY CLUSTERED  ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxAspNetUserClaimsUserID] ON [dbo].[AspNetUserClaims] ([UserID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUserClaims] ADD CONSTRAINT [fkAspNetUserClaimsAspNetUsersUserID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID]) ON DELETE CASCADE
GO

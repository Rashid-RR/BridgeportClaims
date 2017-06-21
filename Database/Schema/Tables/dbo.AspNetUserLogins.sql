CREATE TABLE [dbo].[AspNetUserLogins]
(
[LoginProvider] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ProviderKey] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[AspNetUserLogins] ADD CONSTRAINT [pkAspNetUserLogins] PRIMARY KEY CLUSTERED  ([LoginProvider], [ProviderKey], [UserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxAspNetUserLoginsUserIDIncludeAll] ON [dbo].[AspNetUserLogins] ([UserID]) INCLUDE ([LoginProvider], [ProviderKey]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUserLogins] ADD CONSTRAINT [fkAspNetUserLoginsUserIDAspNetUsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID]) ON DELETE CASCADE
GO

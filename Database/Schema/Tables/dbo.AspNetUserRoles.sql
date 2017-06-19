CREATE TABLE [dbo].[AspNetUserRoles]
(
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RoleID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[AspNetUserRoles] ADD CONSTRAINT [pkAspNetUserRoles] PRIMARY KEY CLUSTERED  ([UserID], [RoleID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxAspNetUserRolesRoleID] ON [dbo].[AspNetUserRoles] ([RoleID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxAspNetUserRolesUserID] ON [dbo].[AspNetUserRoles] ([UserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUserRoles] ADD CONSTRAINT [fkAspNetUserRolesRoleIDAspNetRolesID] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[AspNetRoles] ([ID]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] ADD CONSTRAINT [fkAspNetUserRolesUserIDAspNetUsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO

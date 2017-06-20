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
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/20/2017
	Description:	Trigger designed to automatically assign a user to the "User" role,
					if they are inserted into any other relationship to any other role.
*/
CREATE TRIGGER [dbo].[utAspNetUserRolesAddAllUsersToUserRole] ON [dbo].[AspNetUserRoles]
	AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @UserID NVARCHAR(128)
	SELECT @UserID = i.UserId FROM Inserted i
	IF @UserID IS NULL
		RETURN
    EXEC dbo.uspAddUsersToUserRoleGroup @UserID
END
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

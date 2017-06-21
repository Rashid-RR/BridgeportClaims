SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/20/2017
	Description:	Adds a user to the "User" role group, if they are a member of any other role
	Sample Execute:
					EXEC dbo.uspAddUsersToUserRoleGroup
*/
CREATE PROC [dbo].[uspAddUsersToUserRoleGroup] @UserID NVARCHAR(128) = NULL
AS BEGIN
    SET NOCOUNT ON;
	DECLARE @InternalUserID NVARCHAR(128), @UserRoleID NVARCHAR(128)

	SELECT @UserRoleID = r.Id FROM dbo.AspNetRoles r WHERE r.Name = 'User'

	DECLARE C CURSOR LOCAL FAST_FORWARD FOR
	SELECT u.ID
	FROM   dbo.AspNetUsers u
		   INNER JOIN dbo.AspNetUserRoles ur ON ur.UserId = u.ID
		   LEFT JOIN dbo.AspNetUserRoles uur INNER JOIN dbo.AspNetRoles r ON r.Id = uur.RoleId ON uur.UserId = u.ID AND r.Name = 'User'
	WHERE
			uur.UserId IS NULL
			AND (@UserID IS NULL OR @UserID = u.ID)
	OPEN C;
	FETCH NEXT FROM C INTO @InternalUserID
	WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT dbo.AspNetUserRoles (UserId, RoleId)
			VALUES (   @InternalUserID -- UserId - nvarchar(128)
			         , @UserRoleID -- RoleId - nvarchar(128)
			       )
			FETCH NEXT FROM C INTO @InternalUserID
		END
	CLOSE C;
	DEALLOCATE C;

END
GO

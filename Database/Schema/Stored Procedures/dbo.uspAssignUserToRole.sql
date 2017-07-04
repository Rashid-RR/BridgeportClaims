SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- Stored Procedure

/*
	Author:			Jordan Gurney
	Create Date:	6/21/2017
	Description:	Inserts an "Admin" or "User" role into the database,
					given a user's Email Address
	Modified:		6/22/2017 - Added call to new proc "dbo.uspCleanupUserRoles"
	Sample Execute:
					EXEC dbo.uspAssignUserToRole 'jogwayi@gmail.com', 'Admin'
*/
CREATE PROC [dbo].[uspAssignUserToRole] @Email NVARCHAR(256), @RoleName NVARCHAR(256)
AS BEGIN
	SET NOCOUNT ON
	EXEC dbo.uspCleanupUserRoles
	DECLARE @UserID NVARCHAR(128), @RoleID NVARCHAR(128)
	
	SELECT @UserID = u.ID
	FROM   dbo.AspNetUsers u
	WHERE  u.Email = @Email

	SELECT @RoleID = r.ID
	FROM   dbo.AspNetRoles r
	WHERE  r.[Name] = @RoleName

	IF NOT EXISTS (   SELECT *
					  FROM   [dbo].[AspNetUserRoles] AS x
					  WHERE  [x].[UserID] = @UserID
							 AND [x].[RoleID] = @RoleID
				  )
		INSERT [dbo].[AspNetUserRoles] (UserID, RoleID)
		VALUES ( @UserID, @RoleID )
END


GO

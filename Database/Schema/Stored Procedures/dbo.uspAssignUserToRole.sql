SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/21/2017
	Description:	Inserts an "Admin" or "User" role into the database,
					given a user's Email Address
	Sample Execute:
					EXEC dbo.uspAssignUserToRole 'jogwayi@gmail.com', 'Admin'
*/
CREATE PROC [dbo].[uspAssignUserToRole] @Email NVARCHAR(256), @RoleName NVARCHAR(256)
AS BEGIN
	SET NOCOUNT ON
	DECLARE @UserID NVARCHAR(128), @RoleID NVARCHAR(128)
	
	SELECT @UserID = u.ID
	FROM   dbo.AspNetUsers u
	WHERE  u.Email = @Email

	SELECT @RoleID = r.ID
	FROM   dbo.AspNetRoles r
	WHERE  r.[Name] = @RoleName

	INSERT dbo.AspNetUserRoles (UserID, RoleID)
	VALUES (@UserID, @RoleID)
END
GO

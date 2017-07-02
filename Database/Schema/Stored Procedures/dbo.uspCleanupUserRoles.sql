SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/22/2017
	Description:	Finds users in any other role than "User", and places them
					into the "User" role as 
	Sample Execute:
					EXEC dbo.uspCleanupUserRoles
*/
CREATE PROC [dbo].[uspCleanupUserRoles]
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @UserRoleID NVARCHAR(128)

	SELECT @UserRoleID = r.ID
	FROM   dbo.AspNetRoles r
	WHERE  r.Name = 'User';

	WITH RolesCTE (UserID) AS
	(
		SELECT r.UserID
		FROM   [dbo].[vwAspNetUserAndRole] r
		WHERE  r.RoleID != @UserRoleID
			   AND NOT EXISTS 
			   (
					SELECT *
					FROM   [dbo].[vwAspNetUserAndRole] ir
					WHERE  ir.UserID = r.UserID
						   AND ir.RoleID = @UserRoleID
			   )
	)
	INSERT dbo.AspNetUserRoles (UserID, RoleID)
	SELECT c.UserID
		 , @UserRoleID
	FROM   RolesCTE c
END
GO

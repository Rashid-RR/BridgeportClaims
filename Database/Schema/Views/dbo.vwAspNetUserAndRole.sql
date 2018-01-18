SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwAspNetUserAndRole]
AS
SELECT u.ID UserID
	 , u.FirstName
     , u.LastName
     , u.UserName
	 , r.ID RoleID
     , RoleName = r.Name
	 , u.[EmailConfirmed]
	 , u.[RegisteredDate]
	 , u.[LockoutEndDateUtc]
	 , u.[LockoutEnabled]
FROM   dbo.AspNetUsers AS u
       LEFT JOIN dbo.AspNetUserRoles AS ur
                 INNER JOIN dbo.AspNetRoles AS r ON r.Id = ur.RoleId ON ur.UserId = u.ID
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwAspNetUsersAndRoles]
AS
SELECT u.FirstName
     , u.LastName
     , u.UserName
     , RoleName = r.Name
FROM   dbo.AspNetUsers AS u
       LEFT JOIN dbo.AspNetUserRoles AS ur
                 INNER JOIN dbo.AspNetRoles AS r ON r.Id = ur.RoleId ON ur.UserId = u.ID
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       9/13/2018
 Description:       Gets all Active Users 
 Example Execute:
					DECLARE @UtcNow DATETIME = GETUTCDATE();
                    EXECUTE [dbo].[uspGetActiveUsers] 'Jordan', 'Gurney', @UtcNow
 =============================================
*/
CREATE PROC [dbo].[uspGetActiveUsers]
(
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@UtcNow DATETIME
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	SELECT  u.ID Id
		   ,u.FirstName
		   ,u.LastName
	FROM    dbo.AspNetUsers u
	WHERE   (
				u.LockoutEndDateUtc IS NULL
				OR  u.LockoutEnabled = 1
					AND (u.LockoutEndDateUtc IS NOT NULL)
					AND u.LockoutEndDateUtc > @UtcNow
			)
			AND
			((u.FirstName IS NOT NULL)
			 AND (u.FirstName <> @FirstName OR  u.FirstName IS NULL)
			 OR (u.LastName IS NOT NULL)
				AND (u.LastName <> @LastName OR u.LastName IS NULL)
			);
END
GO

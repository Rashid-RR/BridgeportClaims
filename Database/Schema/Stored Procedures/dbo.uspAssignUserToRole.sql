SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/21/2017
	Description:	Inserts an "Admin" or "User" role into the database,
					given a user's Email Address
	Modified:		6/22/2017 - Added call to new proc "dbo.uspCleanupUserRoles"
	Sample Execute:
					EXEC dbo.uspAssignUserToRole 'teaton@bridgeportclaims.com', 'Admin'
*/
CREATE PROC [dbo].[uspAssignUserToRole] @Email NVARCHAR(256), @RoleName NVARCHAR(256)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
            
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

			-- Inserting the User role if the role passed in is not User or Client, and they don't have a User role already.
			IF (@RoleName != 'User' AND @RoleName != 'Client')
				BEGIN
					DECLARE @UserRoleID NVARCHAR(128)

					SELECT @UserRoleID = [anr].[ID] FROM [dbo].[AspNetRoles] AS [anr] WHERE  [anr].[Name] = 'User'

					IF NOT EXISTS (   SELECT *
									  FROM   [dbo].[AspNetUserRoles] AS x
									  WHERE  [x].[UserID] = @UserID
											 AND [x].[RoleID] = @UserRoleID
						  )
					INSERT [dbo].[AspNetUserRoles] (UserID, RoleID)
					VALUES ( @UserID, @UserRoleID )
				END
            
        IF (@@TRANCOUNT > 0)
            COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
            ROLLBACK;
                
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

        RAISERROR(N'%s (line %d): %s',    -- Message text w formatting
            @ErrSeverity,        -- Severity
            @ErrState,            -- State
            @ErrProc,            -- First argument (string)
            @ErrLine,            -- Second argument (int)
            @ErrMsg);            -- First argument (string)
    END CATCH
END
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	12/07/2018
	Description:	Adds a user one or more User Roles
	Sample Execute:
					EXEC dbo.uspAddUserToRole
*/
CREATE PROC [dbo].[uspAddUserToRole] @UserID NVARCHAR(128), @RoleID NVARCHAR(128)
AS BEGIN
    SET NOCOUNT ON;
	SET XACT_ABORT ON;
	SET DEADLOCK_PRIORITY HIGH;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;

	DECLARE @AdminRoleID NVARCHAR(128), @IndexerRoleID NVARCHAR(128), @UserRoleID NVARCHAR(128)
	SELECT @AdminRoleID = [r].[ID] FROM [dbo].[AspNetRoles] AS [r] WHERE [r].[Name] = 'Admin'
	SELECT @IndexerRoleID = [r].[ID] FROM [dbo].[AspNetRoles] AS [r] WHERE [r].[Name] = 'Indexer'
	SELECT @UserRoleID = [r].[ID] FROM [dbo].[AspNetRoles] AS [r] WHERE [r].[Name] = 'User'

	DECLARE @InternalUserID NVARCHAR(128)
		  , @InternalRoleID NVARCHAR(128)
	SELECT  @InternalUserID = @UserID
		  , @InternalRoleID = @RoleID
	DECLARE @IsTargetRoleAdmin BIT, @IsTargetRoleIndexer BIT, @IsTargetRoleUser BIT
	
	IF @InternalRoleID = @AdminRoleID
		SET @IsTargetRoleAdmin = 1
	ELSE IF @InternalRoleID = @IndexerRoleID
		SET @IsTargetRoleIndexer = 1
	ELSE IF @InternalRoleID = @UserRoleID
		SET @IsTargetRoleUser = 1
	ELSE
		BEGIN
			RAISERROR(N'An error was encountered. The role passed in was not found.', 16, 1) WITH NOWAIT
			RETURN -1
		END

	IF @IsTargetRoleAdmin = 1
		BEGIN
			SET @IsTargetRoleIndexer = 1
			SET @IsTargetRoleUser = 1
		END

	IF @IsTargetRoleIndexer = 1
		SET @IsTargetRoleUser = 1

	-- Insert into Admin (if necessary)
	IF @IsTargetRoleAdmin = 1 AND
	   NOT EXISTS (SELECT  *
				   FROM    [dbo].[AspNetUserRoles] AS [ir]
				   WHERE   [ir].[UserID] = @InternalUserID
						   AND [ir].[RoleID] = @AdminRoleID)
		INSERT [dbo].[AspNetUserRoles] ([UserID], [RoleID])
		VALUES  (@InternalUserID, @AdminRoleID)

	-- Insert into Indexer (if necessary)
	IF @IsTargetRoleIndexer = 1 AND
	   NOT EXISTS (SELECT  *
				   FROM    [dbo].[AspNetUserRoles] AS [ir]
				   WHERE   [ir].[UserID] = @InternalUserID
						   AND [ir].[RoleID] = @IndexerRoleID)
		INSERT [dbo].[AspNetUserRoles] ([UserID], [RoleID])
		VALUES  (@InternalUserID, @IndexerRoleID)

	-- Insert into User
	IF NOT EXISTS (SELECT  *
				   FROM    [dbo].[AspNetUserRoles] AS [ir]
				   WHERE   [ir].[UserID] = @InternalUserID
						   AND [ir].[RoleID] = @UserRoleID)
		INSERT [dbo].[AspNetUserRoles] ([UserID], [RoleID])
		VALUES  (@InternalUserID, @UserRoleID)
END
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/29/2017
	Description:	CRUD Proc inserting into [dbo].[CheckIndex]
	Sample Execute:
					EXEC [dbo].[uspCheckIndexInsert]
*/
CREATE PROC [dbo].[uspCheckIndexInsert]
    @DocumentID INT,
    @ModifiedByUserID NVARCHAR(128),
    @CheckNumber VARCHAR(100)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @PrntMsg NVARCHAR(100);
	DECLARE @AlreadyExists BIT;
	IF EXISTS (SELECT * FROM [dbo].[CheckIndex] c WHERE c.DocumentID = @DocumentID)
		SET @AlreadyExists = 1;
	ELSE
		SET @AlreadyExists = 0;

	IF (@AlreadyExists = 1)
		BEGIN
			IF (@@TRANCOUNT > 0)
				ROLLBACK;
 			SET @PrntMsg = N'A CheckIndex record already exists.';
			RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
			RETURN -1;
		END

	DECLARE @UtcNow DATETIME2 = SYSUTCDATETIME();
	INSERT [dbo].[CheckIndex] ([DocumentID],[CheckNumber],[ModifiedByUserID],[CreatedOnUTC],[UpdatedOnUTC])
	SELECT @DocumentID, @CheckNumber, @ModifiedByUserID, @UtcNow, @UtcNow;
END
GO

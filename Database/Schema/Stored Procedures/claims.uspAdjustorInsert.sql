SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [claims].[uspAdjustorInsert] 
    @AdjustorName varchar(255),
    @PhoneNumber varchar(30),
    @FaxNumber varchar(30),
    @EmailAddress varchar(155),
    @Extension varchar(10),
    @ModifiedByUserID nvarchar(128)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @AdjustorID INT;
	DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();
	
	INSERT INTO [dbo].[Adjustor] ([AdjustorName], [PhoneNumber], [FaxNumber], [EmailAddress], [Extension], [ModifiedByUserID], [CreatedOnUTC], [UpdatedOnUTC])
	SELECT @AdjustorName, @PhoneNumber, @FaxNumber, @EmailAddress, @Extension, @ModifiedByUserID, @UtcNow, @UtcNow;
	
	SET @AdjustorID = SCOPE_IDENTITY();

	SELECT [AdjustorId] = [a].[AdjustorID], [a].[AdjustorName], [a].[PhoneNumber], [a].[FaxNumber], [a].[EmailAddress], [a].[Extension],
		   [ModifiedBy] = [x].[FirstName] + ' ' + [x].[LastName]
	FROM   [dbo].[Adjustor] AS [a]
		   LEFT JOIN [dbo].[AspNetUsers] AS [x] ON [a].[ModifiedByUserID] = [x].[ID]
	WHERE  [a].[AdjustorID] = @AdjustorID;
END
GO

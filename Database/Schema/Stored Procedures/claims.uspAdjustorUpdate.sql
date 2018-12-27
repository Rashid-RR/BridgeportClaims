SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [claims].[uspAdjustorUpdate]
    @AdjustorID int,
    @AdjustorName varchar(255),
    @PhoneNumber varchar(30),
    @FaxNumber varchar(30),
    @EmailAddress varchar(155),
    @Extension varchar(10),
    @ModifiedByUserID nvarchar(128)
AS 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();

	UPDATE [dbo].[Adjustor]
	SET    [AdjustorName] = @AdjustorName, [PhoneNumber] = @PhoneNumber, [FaxNumber] = @FaxNumber, [EmailAddress] = @EmailAddress,
		   [Extension] = @Extension, [ModifiedByUserID] = @ModifiedByUserID, [UpdatedOnUTC] = @UtcNow
	WHERE  [AdjustorID] = @AdjustorID;

	SELECT [AdjustorId] = [a].[AdjustorID], [a].[AdjustorName], [a].[PhoneNumber], [a].[FaxNumber], [a].[EmailAddress], [a].[Extension],
		   [ModifiedBy] = [x].[FirstName] + ' ' + [x].[LastName]
	FROM   [dbo].[Adjustor] AS [a]
		   LEFT JOIN [dbo].[AspNetUsers] AS [x] ON [a].[ModifiedByUserID] = [x].[ID]
	WHERE  [a].[AdjustorID] = @AdjustorID;
GO

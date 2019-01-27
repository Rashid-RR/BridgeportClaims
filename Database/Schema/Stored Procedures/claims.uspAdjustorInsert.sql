SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [claims].[uspAdjustorInsert]
    @AdjustorName VARCHAR(255)
   ,@Address1 VARCHAR(255)
   ,@Address2 VARCHAR(255)
   ,@City VARCHAR(255)
   ,@StateID INT
   ,@PostalCode VARCHAR(255)
   ,@PhoneNumber VARCHAR(30)
   ,@FaxNumber VARCHAR(30)
   ,@EmailAddress VARCHAR(155)
   ,@Extension VARCHAR(10)
   ,@ModifiedByUserID NVARCHAR(128)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @AdjustorID INT;
    DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();

    INSERT INTO [dbo].[Adjustor] ([AdjustorName],[Address1],[Address2],[City],[StateID],[PostalCode],[PhoneNumber],[FaxNumber],[EmailAddress],[Extension],[ModifiedByUserID],[CreatedOnUTC],[UpdatedOnUTC])
    SELECT  @AdjustorName, @Address1, @Address2, @City, @StateID, @PostalCode, @PhoneNumber, @FaxNumber, @EmailAddress, @Extension, @ModifiedByUserID, @UtcNow, @UtcNow;

    SET @AdjustorID = SCOPE_IDENTITY();

    SELECT   [AdjustorId] = [a].[AdjustorID]
            ,[a].[AdjustorName]
            ,[a].[Address1]
            ,[a].[Address2]
            ,[a].[City]
            ,[us].[StateName]
            ,[a].[PostalCode]
            ,[a].[PhoneNumber]
            ,[a].[FaxNumber]
            ,[a].[EmailAddress]
            ,[a].[Extension]
            ,[ModifiedBy] = [x].[FirstName] + ' ' + [x].[LastName]
    FROM    [dbo].[Adjustor] AS [a]
            LEFT JOIN [dbo].[AspNetUsers] AS [x] ON [a].[ModifiedByUserID] = [x].[ID]
			LEFT JOIN [dbo].[UsState] AS [us] ON [a].[StateID] = [us].[StateID]
    WHERE   [a].[AdjustorID] = @AdjustorID;
END

GO

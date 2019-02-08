SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [claims].[uspAttorneyInsert]
    @AttorneyName varchar(255),
    @Address1 varchar(255),
    @Address2 varchar(255),
    @City varchar(255),
    @StateID int,
    @PostalCode varchar(255),
    @PhoneNumber varchar(30),
    @FaxNumber varchar(30),
	@EmailAddress VARCHAR(155),
    @ModifiedByUserID nvarchar(128)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();
	DECLARE @AttorneyID INT;

	INSERT INTO [dbo].[Attorney] ([AttorneyName], [Address1], [Address2], [City], [StateID], [PostalCode], [PhoneNumber], [FaxNumber], EmailAddress, [ModifiedByUserID], [CreatedOnUTC], [UpdatedOnUTC])
	SELECT @AttorneyName, @Address1, @Address2, @City, @StateID, @PostalCode, @PhoneNumber, @FaxNumber, @EmailAddress, @ModifiedByUserID, @UtcNow, @UtcNow;

	SET @AttorneyID = SCOPE_IDENTITY();

	SELECT  [AttorneyId] = [a].[AttorneyID]
		   ,[a].[AttorneyName]
		   ,[a].[Address1]
		   ,[a].[Address2]
		   ,[a].[City]
		   ,[us].[StateName]
		   ,[a].[PostalCode]
		   ,[a].[PhoneNumber]
		   ,[a].[FaxNumber]
		   ,[a].[EmailAddress]
		   ,[ModifiedBy] = [x].[FirstName] + ' ' + [x].[LastName]
	FROM    [dbo].[Attorney] AS [a]
			LEFT JOIN [dbo].[UsState] AS [us] ON [a].[StateID] = [us].[StateID]
			INNER JOIN [dbo].[AspNetUsers] AS [x] ON [a].[ModifiedByUserID] = [x].[ID]
	WHERE   [a].[AttorneyID] = @AttorneyID;
END
GO

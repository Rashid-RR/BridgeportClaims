SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[uspPayorInsert]
    @GroupName VARCHAR(255)
  , @BillToName VARCHAR(255)
  , @BillToAddress1 VARCHAR(255)
  , @BillToAddress2 VARCHAR(255)
  , @BillToCity VARCHAR(155)
  , @BillToStateID INT
  , @BillToPostalCode VARCHAR(100)
  , @PhoneNumber VARCHAR(30)
  , @AlternatePhoneNumber VARCHAR(30)
  , @FaxNumber VARCHAR(30)
  , @Notes VARCHAR(8000)
  , @Contact VARCHAR(255)
  , @LetterName VARCHAR(255)
  , @ModifiedByUserID NVARCHAR(128)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();
        DECLARE @PayorID INT;
        INSERT INTO [dbo].[Payor]
        (
            [GroupName]
          , [BillToName]
          , [BillToAddress1]
          , [BillToAddress2]
          , [BillToCity]
          , [BillToStateID]
          , [BillToPostalCode]
          , [PhoneNumber]
          , [AlternatePhoneNumber]
          , [FaxNumber]
          , [Notes]
          , [Contact]
          , [LetterName]
          , [CreatedOnUTC]
          , [UpdatedOnUTC]
          , [ModifiedByUserID]
        )
        SELECT @GroupName
             , @BillToName
             , @BillToAddress1
             , @BillToAddress2
             , @BillToCity
             , @BillToStateID
             , @BillToPostalCode
             , @PhoneNumber
             , @AlternatePhoneNumber
             , @FaxNumber
             , @Notes
             , @Contact
             , @LetterName
             , @UtcNow
             , @UtcNow
             , @ModifiedByUserID;
        SET @PayorID = SCOPE_IDENTITY();
        SELECT [p].[PayorID] PayorId
             , [p].[GroupName]
			 , [p].[BillToName]
             , [p].[BillToAddress1]
			 , [p].[BillToAddress2]
			 , [p].[BillToCity]
			 , (SELECT x.[StateName] FROM [dbo].[UsState] AS x WHERE x.[StateID] = @BillToStateID) AS [BillToStateName]
             , [p].[BillToPostalCode]
             , [p].[PhoneNumber]
             , [p].[AlternatePhoneNumber]
             , [p].[FaxNumber]
             , [p].[Notes]
             , [p].[Contact]
             , [p].[LetterName]
             , [x].[FirstName] + ' ' + [x].[LastName] AS [ModifiedBy]
        FROM   [dbo].[Payor] AS [p]
               LEFT JOIN [dbo].[AspNetUsers] AS [x] ON [x].[ID] = [p].[ModifiedByUserID]
        WHERE  [p].[PayorID] = @PayorID;
    END;

GO

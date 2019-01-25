SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[uspPayorUpdate]
    @PayorID INT
  , @GroupName VARCHAR(255)
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
        UPDATE [dbo].[Payor]
        SET    [GroupName] = @GroupName
             , [BillToName] = @BillToName
             , [BillToAddress1] = @BillToAddress1
             , [BillToAddress2] = @BillToAddress2
             , [BillToCity] = @BillToCity
             , [BillToStateID] = @BillToStateID
             , [BillToPostalCode] = @BillToPostalCode
             , [PhoneNumber] = @PhoneNumber
             , [AlternatePhoneNumber] = @AlternatePhoneNumber
             , [FaxNumber] = @FaxNumber
             , [Notes] = @Notes
             , [Contact] = @Contact
             , [LetterName] = @LetterName
             , [UpdatedOnUTC] = @UtcNow
             , [ModifiedByUserID] = @ModifiedByUserID
        WHERE  [PayorID] = @PayorID;
        SELECT [p].[PayorID]
             , [p].[GroupName]
             , [p].[BillToAddress1] + ' ' + [p].[BillToAddress2] + ', ' + [p].[BillToCity] + ', ' + [us].[StateName]
               + '  ' + [p].[BillToPostalCode] AS [BillingAddress]
             , [p].[PhoneNumber]
             , [p].[AlternatePhoneNumber]
             , [p].[FaxNumber]
             , [p].[Notes]
             , [p].[Contact]
             , [p].[LetterName]
             , [x].[FirstName] + ' ' + [x].[LastName] AS [ModifiedBy]
        FROM   [dbo].[Payor] AS [p]
               LEFT JOIN [dbo].[UsState] AS [us] ON [us].[StateID] = [p].[BillToStateID]
               LEFT JOIN [dbo].[AspNetUsers] AS [x] ON [x].[ID] = [p].[ModifiedByUserID]
        WHERE  [p].[PayorID] = @PayorID;
    END;
GO

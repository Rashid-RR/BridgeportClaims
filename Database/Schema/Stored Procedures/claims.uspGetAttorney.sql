SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       2/18/2019
 Description:       Gets a single Attorney by ID 
 Example Execute:
                    EXECUTE [claims].[uspGetAttorney] 258
 =============================================
*/
CREATE PROC [claims].[uspGetAttorney] @AttorneyID INT
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
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
            LEFT JOIN [dbo].[AspNetUsers] AS [x] ON [a].[ModifiedByUserID] = [x].[ID]
            LEFT JOIN [dbo].[UsState] AS [us] ON [a].[StateID] = [us].[StateID]
    WHERE   [a].[AttorneyID] = @AttorneyID;
END
GO

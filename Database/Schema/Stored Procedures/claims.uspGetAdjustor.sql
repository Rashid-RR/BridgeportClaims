SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       2/15/2019
 Description:       Gets the Adjustor for the Link on the Claims blade. 
 Example Execute:	
                    EXECUTE [claims].[uspGetAdjustor] 11
 =============================================
*/
CREATE PROC [claims].[uspGetAdjustor] @AdjustorID INT
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SELECT  [AdjustorId] = [a].[AdjustorID]
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

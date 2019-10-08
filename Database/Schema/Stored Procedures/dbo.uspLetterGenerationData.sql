SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		3/7/2018
 Description:		Pulls all of the data necessary for populating the 3 letters available on the Claims page.
 Example Execute:
					DECLARE @UserID NVARCHAR(128) = [util].[udfGetRandomAspNetUserID]()
					EXECUTE [dbo].[uspLetterGenerationData] 775, @UserID, 9966
 =============================================
*/
CREATE PROC [dbo].[uspLetterGenerationData]
(
    @ClaimID INT
   ,@UserID NVARCHAR(128)
   ,@PrescriptionID INT = NULL
)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        DECLARE @Today DATE = CONVERT(DATE, [dtme].[udfGetLocalDate]());

        DECLARE @FirstName VARCHAR(100)
               ,@LastName VARCHAR(100)
               ,@Extension VARCHAR(30);
        SELECT @FirstName = [u].[FirstName]
              ,@LastName = [u].[LastName]
              ,@Extension = [u].[Extension]
        FROM [dbo].[AspNetUsers] AS [u]
        WHERE [u].[ID] = @UserID;

        SELECT @Today AS [TodaysDate]
              ,[p].[FirstName]
              ,[p].[LastName]
              ,[p].[Address1]
              ,[p].[Address2]
              ,[p].[City]
              ,[us].[StateCode]
              ,[p].[PostalCode]
              ,[pay].[LetterName]
              ,[pay].[BillToName]
              ,@FirstName AS [UserFirstName]
              ,@LastName AS [UserLastName]
              ,(
                   SELECT [p].[PharmacyName]
                   FROM [dbo].[vwPrescriptionPharmacy] AS [p] WITH (NOEXPAND)
                   WHERE [p].[PrescriptionID] = @PrescriptionID
               ) AS [PharmacyName]
              ,@Extension AS [Extension]
              ,[a].[AttorneyName]
              ,[a].[Address1] AS [AttorneyAddress1]
              ,[a].[Address2] AS [AttorneyAddress2]
              ,[a].[City] AS [AttorneyCity]
              ,NULLIF([usa].[StateCode], 'NA') AS [AttorneyStateCode]
              ,[a].[PostalCode] AS [AttorneyPostalCode]
              ,[c].[ClaimNumber]
        FROM [dbo].[Claim] AS [c]
             LEFT JOIN [dbo].[Attorney] AS [a]
                       LEFT JOIN [dbo].[UsState] AS [usa] ON [usa].[StateID] = [a].[StateID] ON [a].[AttorneyID] = [c].[AttorneyID]
             INNER JOIN [dbo].[Payor] AS [pay] ON [pay].[PayorID] = [c].[PayorID]
             INNER JOIN [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
             LEFT JOIN [dbo].[UsState] AS [us] ON [us].[StateID] = [p].[StateID]
        WHERE [c].[ClaimID] = @ClaimID;
    END;
GO

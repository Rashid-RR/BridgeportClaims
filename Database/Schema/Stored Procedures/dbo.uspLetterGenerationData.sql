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
					DECLARE @UserID NVARCHAR(128)
					SELECT TOP 1 @UserID = u.[ID] FROM [dbo].[AspNetUsers] AS [u] WHERE u.Extension IS NOT NULL ORDER BY NEWID()
					EXECUTE [dbo].[uspLetterGenerationData] 775, @UserID, 9966
 =============================================
*/
CREATE PROC [dbo].[uspLetterGenerationData]
(
	@ClaimID INT,
	@UserID NVARCHAR(128),
	@PrescriptionID INT = NULL
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @Today DATE = CONVERT(DATE, [dtme].[udfGetLocalDate]());

	DECLARE @FirstName VARCHAR(100), @LastName VARCHAR(100), @Extension VARCHAR(30)
	SELECT  @FirstName = u.[FirstName]
			, @LastName  = u.[LastName]
			, @Extension = u.[Extension]
	FROM    [dbo].[AspNetUsers] AS [u]
	WHERE   [u].[ID] = @UserID;

	SELECT          TodaysDate = @Today
					, p.[FirstName]
					, p.[LastName]
					, p.[Address1]
					, p.[Address2]
					, p.[City]
					, us.[StateCode]
					, p.[PostalCode]
					, [pay].[LetterName]
					, [pay].[BillToName]
					, @FirstName UserFirstName
					, @LastName UserLastName
					,(SELECT [p].[PharmacyName]
					FROM   [dbo].[vwPrescriptionPharmacy] AS [p] WITH (NOEXPAND)
					WHERE  [p].[PrescriptionID] = @PrescriptionID) [PharmacyName]
					,@Extension Extension
	FROM            [dbo].[Claim]   AS [c]
		INNER JOIN  [dbo].[Payor]   AS [pay] ON [pay].[PayorID] = [c].[PayorID]
		INNER JOIN  [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
		LEFT JOIN   [dbo].[UsState] AS [us] ON [us].[StateID] = [p].[StateID]
	WHERE           [c].[ClaimID] = @ClaimID;
END
GO

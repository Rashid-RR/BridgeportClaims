SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		11/11/2018
 Description:		Pulls all of the data necessary for populating the Dr Note Request Letter.
 Example Execute:
					DECLARE @UserID NVARCHAR(128) = util.[udfGetRandomAspNetUserID]()
					DECLARE @PrescriptionIds [dbo].[udtID]; INSERT @PrescriptionIds (ID) VALUES (1),(2),(3),(4),(5);
					EXECUTE [dbo].[uspDrNoteLetterGenerationData] 775, @UserID, 9966, @PrescriptionIds;
 =============================================
*/
CREATE PROC [dbo].[uspDrNoteLetterGenerationData]
(
	@ClaimID INT,
	@UserID NVARCHAR(128),
	@FirstPrescriptionID INT,
	@PrescriptionIds [dbo].[udtID] READONLY
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @Today DATE = CONVERT(DATE, [dtme].[udfGetLocalDate]());

	DECLARE @FirstName VARCHAR(100), @LastName VARCHAR(100), @Extension VARCHAR(30);
	SELECT    @FirstName = u.[FirstName]
			, @LastName  = u.[LastName]
			, @Extension = u.[Extension]
	FROM    [dbo].[AspNetUsers] AS [u]
	WHERE   [u].[ID] = @UserID;

	SELECT            TodaysDate = @Today
					, [pre].[Prescriber] PrescriberName
					, [pb].[Addr1]
					, [pb].[Addr2]
					, [pb].[City]e
					, [us].[StateCode]
					, [pb].[Zip] [PostalCode]
					, [p].[FirstName]
					, [p].[LastName]
					, [p].[DateOfBirth]
					, [c].[ClaimNumber]
					, [pay].[LetterName]
					, CASE WHEN (SELECT COUNT(*) FROM @PrescriptionIds) > 1 THEN 's' ELSE '' END [Plurality]
					, @FirstName UserFirstName
					, @LastName UserLastName
					, @Extension Extension
					,(SELECT [p].[PharmacyName]
					  FROM   [dbo].[vwPrescriptionPharmacy] AS [p] WITH (NOEXPAND)
					  WHERE  [p].[PrescriptionID] = @FirstPrescriptionID) [PharmacyName]
	FROM            [dbo].[Claim]   AS [c]
		INNER JOIN  [dbo].[Payor]   AS [pay] ON [pay].[PayorID] = [c].[PayorID]
		INNER JOIN  [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
		INNER JOIN  [dbo].[Prescription] AS [pre] ON [pre].[ClaimID] = [c].[ClaimID]
		LEFT JOIN	[dbo].[Prescriber] AS [pb] ON [pb].[PrescriberNPI] = [pre].[PrescriberNPI]
		LEFT JOIN	[dbo].[UsState] AS [us] ON [pb].[StateID] = [us].[StateID]
	WHERE           [c].[ClaimID] = @ClaimID
					AND [pre].[PrescriptionID] = @FirstPrescriptionID;

	SELECT [p].[LabelName], p.[DateFilled] FROM @PrescriptionIds AS [pi] INNER JOIN [dbo].[Prescription] AS [p] ON [p].[PrescriptionID] = [pi].[ID];
END

GO

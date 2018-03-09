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
					SELECT TOP 1 @UserID = u.[ID] FROM [dbo].[AspNetUsers] AS [u] ORDER BY NEWID()
					EXECUTE [dbo].[uspLetterGenerationData] 775, @UserID, 9966
 =============================================
*/
CREATE PROC [dbo].[uspLetterGenerationData]
(
	@ClaimID INT,
	@UserID NVARCHAR(128),
	@PrescriptionID INT
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
		DECLARE @Today DATE = CONVERT(DATE, [dtme].[udfGetLocalDate]()), @One INT = 1;

		DECLARE @FirstName VARCHAR(100), @LastName VARCHAR(100)
		SELECT  @FirstName = u.[FirstName]
			  , @LastName  = u.[LastName]
		FROM    [dbo].[AspNetUsers] AS [u]
		WHERE   [u].[ID] = @UserID;

		SELECT          TodaysDate = FORMAT(@Today, 'MM/dd/yyyy')
					  , p.[FirstName]
					  , p.[LastName]
					  , p.[Address1]
					  , p.[Address2]
					  , p.[City]
					  , us.[StateCode]
					  , p.[PostalCode]
					  , [pay].[GroupName]
					  , @FirstName UserFirstName
					  , @LastName UserLastName
					  ,(SELECT [p].[PharmacyName]
						FROM   [dbo].[vwPrescriptionPharmacy] AS [p] WITH (NOEXPAND)
						WHERE  [p].[PrescriptionID] = @PrescriptionID) [PharmacyName]
		FROM            [dbo].[Claim]   AS [c]
			INNER JOIN  [dbo].[Payor]   AS [pay] ON [pay].[PayorID] = [c].[PayorID]
			INNER JOIN  [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
			LEFT JOIN   [dbo].[UsState] AS [us] ON [us].[StateID] = [p].[StateID]
		WHERE           [c].[ClaimID] = @ClaimID

		IF (@@TRANCOUNT > 0)
			COMMIT;
    END TRY
    BEGIN CATCH     
		IF (@@TRANCOUNT > 0)
			ROLLBACK;
				
		DECLARE @ErrSeverity INT = ERROR_SEVERITY()
			, @ErrState INT = ERROR_STATE()
			, @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
			, @ErrLine INT = ERROR_LINE()
			, @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

		RAISERROR(N'%s (line %d): %s',	-- Message text w formatting
			@ErrSeverity,		-- Severity
			@ErrState,			-- State
			@ErrProc,			-- First argument (string)
			@ErrLine,			-- Second argument (int)
			@ErrMsg);			-- First argument (string)
    END CATCH
END
GO

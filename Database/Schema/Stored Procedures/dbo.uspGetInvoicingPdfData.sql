SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       4/6/2019
 Description:       Gets the data needed to populate an invoice PDF.
 Example Execute:
					DECLARE @ID NVARCHAR(128) = [util].[udfGetRandomAspNetUserID]();
                    EXECUTE [dbo].[uspGetInvoicingPdfData] @ID;
 =============================================
*/
CREATE PROCEDURE [dbo].[uspGetInvoicingPdfData]
(
	@GeneratedByUserID NVARCHAR(128)
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	DECLARE @TodaysLocalDate DATE = CAST([dtme].[udfGetLocalDate]() AS DATE);
	DECLARE @Msg NVARCHAR(4000);
	BEGIN TRY
		BEGIN TRANSACTION;
		-- Testing
		/*
			DROP TABLE IF EXISTS #ReturnTable;
			DROP TABLE IF EXISTS #Ranking;
			DROP TABLE IF EXISTS #GroupedClaim;
			DROP TABLE IF EXISTS #DistinctReturnTable;
		*/
		DECLARE @ImportTypeEnvision INT = [etl].[udfGetImportTypeByCode]('ENVISION');
		DECLARE @InvoiceNumber BIGINT = (NEXT VALUE FOR dbo.[seqInvoice]);
		DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]()

		CREATE TABLE #DistinctReturnTable (
			[ClaimId] [int] NOT NULL,
			[BillToName] [varchar](255) NOT NULL,
			[BillToAddress1] [varchar](255) NULL,
			[BillToAddress2] [varchar](255) NULL,
			[BillToCity] [varchar](155) NULL,
			[BillToStateCode] [char](2) NULL,
			[BillToPostalCode] [varchar](100) NULL,
			[InvoiceNumber] [bigint] NULL,
			[InvoiceDate] [nvarchar](4000) NULL,
			[ClaimNumber] [varchar](255) NOT NULL,
			[PatientLastName] [varchar](155) NOT NULL,
			[PatientFirstName] [varchar](155) NOT NULL,
			[PatientAddress1] [varchar](255) NULL,
			[PatientAddress2] [varchar](255) NULL,
			[PatientCity] [varchar](155) NULL,
			[PatientStateCode] [char](2) NULL,
			[PatientPostalCode] [varchar](100) NULL,
			[PatientPhoneNumber] [varchar](30) NULL,
			[DateOfBirthDay] [varchar](2) NULL,
			[DateOfBirthMonth] [varchar](2) NULL,
			[DateOfBirthYear] [varchar](2) NULL,
			[IsMale] [bit] NULL,
			[IsFemale] [bit] NULL,
			[DateOfInjuryDay] [varchar](2) NULL,
			[DateOfInjuryMonth] [varchar](2) NULL,
			[DateOfInjuryYear] [varchar](2) NULL);
		CREATE TABLE #ReturnTable (
			[ClaimId] [int] NOT NULL,
			[BillToName] [varchar](255) NOT NULL,
			[BillToAddress1] [varchar](255) NULL,
			[BillToAddress2] [varchar](255) NULL,
			[BillToCity] [varchar](155) NULL,
			[BillToStateCode] [char](2) NULL,
			[BillToPostalCode] [varchar](100) NULL,
			[InvoiceNumber] [bigint] NULL,
			[InvoiceDate] [nvarchar](4000) NULL,
			[ClaimNumber] [varchar](255) NOT NULL,
			[PatientLastName] [varchar](155) NOT NULL,
			[PatientFirstName] [varchar](155) NOT NULL,
			[PatientAddress1] [varchar](255) NULL,
			[PatientAddress2] [varchar](255) NULL,
			[PatientCity] [varchar](155) NULL,
			[PatientStateCode] [char](2) NULL,
			[PatientPostalCode] [varchar](100) NULL,
			[PatientPhoneNumber] [varchar](30) NULL,
			[DateOfBirthDay] [varchar](2) NULL,
			[DateOfBirthMonth] [varchar](2) NULL,
			[DateOfBirthYear] [varchar](2) NULL,
			[IsMale] [bit] NULL,
			[IsFemale] [bit] NULL,
			[DateOfInjuryDay] [varchar](2) NULL,
			[DateOfInjuryMonth] [varchar](2) NULL,
			[DateOfInjuryYear] [varchar](2) NULL,
			[PrescriptionId] [int] NOT NULL PRIMARY KEY CLUSTERED,
			[Prescriber] [varchar](100) NULL,
			[PrescriberNpi] [varchar](12) NULL,
			[DateFilledDay] [varchar](2) NULL,
			[DateFilledMonth] [varchar](2) NULL,
			[DateFilledYear] [varchar](2) NULL,
			[Ndc] [varchar](11) NOT NULL,
			[LabelName] [varchar](25) NULL,
			[RxNumber] [varchar](100) NOT NULL,
			[BilledAmount] [money] NOT NULL,
			[BilledAmountDollars] [int] NULL,
			[BilledAmountCents] [varchar](4) NULL,
			[Quantity] [float] NOT NULL,
			[PharmacyName] [varchar](60) NULL,
			[Address1] [varchar](55) NULL,
			[Address2] [varchar](55) NULL,
			[City] [varchar](35) NULL,
			[PharmacyState] [char](2) NOT NULL,
			[PostalCode] [varchar](11) NULL,
			[FederalTin] [varchar](15) NULL,
			[Npi] [varchar](10) NULL,
			[Nabp] [varchar](7) NULL
		);
		CREATE TABLE #Ranking (RowID INT IDENTITY NOT NULL PRIMARY KEY CLUSTERED,
			[ClaimRank] [bigint] NOT NULL,
			[ClaimID] [int] NOT NULL,
			[PrescriberNpiRank] [bigint] NOT NULL,
			[PharmacyNpiRank] [bigint] NOT NULL,
			[PrescriberNpi] [varchar](12) NULL,
			[PharmacyNpi] [varchar](10) NULL,
			[PrescriptionID] [int] NOT NULL
		);
		INSERT INTO [#Ranking]
		(
			[ClaimRank]
		   ,[ClaimID]
		   ,[PrescriberNpiRank]
		   ,[PharmacyNpiRank]
		   ,[PrescriberNpi]
		   ,[PharmacyNpi]
		   ,[PrescriptionID]
		)
		SELECT DENSE_RANK() OVER (ORDER BY [c].[ClaimID]) AS [ClaimRank]
			  ,[c].[ClaimID]
			  ,DENSE_RANK() OVER (ORDER BY [c].[ClaimID], [p].[PrescriberNPI]) AS [PrescriberNpiRank]
			  ,DENSE_RANK() OVER (ORDER BY [c].[ClaimID], [p].[PrescriberNPI], [ph].[NPI]) AS [PharmacyNpiRank]
			  ,[p].[PrescriberNPI] AS [PrescriberNpi]
			  ,[ph].[NPI] AS [PharmacyNpi]
			  ,[p].[PrescriptionID]
		FROM [dbo].[Prescription] AS [p]
			 INNER JOIN [dbo].[Pharmacy] AS [ph] ON [ph].[NABP] = [p].[PharmacyNABP]
			 INNER JOIN [dbo].[Claim] AS [c] ON [c].[ClaimID] = [p].[ClaimID]
		WHERE 1 = 1
			AND [p].[ImportTypeID] = @ImportTypeEnvision
			AND NOT EXISTS (SELECT * FROM [dbo].[InvoiceDocument] AS [id] WHERE [id].[PrescriptionID] = [p].[PrescriptionID]);

		-- Determine the first Claim that we want to work with

		CREATE TABLE #GroupedClaim (PharmacyNpiRank BIGINT NOT NULL, Cnt INT NOT NULL);
		INSERT INTO [#GroupedClaim]
		(
			[PharmacyNpiRank]
			,[Cnt]
		)
		SELECT [r].[PharmacyNpiRank], COUNT(*) Cnt FROM [#Ranking] AS [r]
		WHERE [r].[ClaimRank] = 1
		GROUP BY [r].[PharmacyNpiRank];

		DECLARE @PharmacyNpiRank INT, @One INT = 1, @Six INT = 6;
		SELECT TOP (@One) @PharmacyNpiRank = g.[PharmacyNpiRank] FROM [#GroupedClaim] g ORDER BY g.[Cnt] DESC;
		DECLARE @Prescriptions TABLE (PrescriptionID INT NOT NULL PRIMARY KEY CLUSTERED);
		INSERT INTO @Prescriptions
		(
			[PrescriptionID]
		)
		SELECT TOP (@Six) [r].[PrescriptionID] FROM [#Ranking] AS [r] WHERE [r].[PharmacyNpiRank] = @PharmacyNpiRank;

		INSERT INTO [#ReturnTable]
		(
			[ClaimId]
		   ,[BillToName]
		   ,[BillToAddress1]
		   ,[BillToAddress2]
		   ,[BillToCity]
		   ,[BillToStateCode]
		   ,[BillToPostalCode]
		   ,[InvoiceNumber]
		   ,[InvoiceDate]
		   ,[ClaimNumber]
		   ,[PatientLastName]
		   ,[PatientFirstName]
		   ,[PatientAddress1]
		   ,[PatientAddress2]
		   ,[PatientCity]
		   ,[PatientStateCode]
		   ,[PatientPostalCode]
		   ,[PatientPhoneNumber]
		   ,[DateOfBirthDay]
		   ,[DateOfBirthMonth]
		   ,[DateOfBirthYear]
		   ,[IsMale]
		   ,[IsFemale]
		   ,[DateOfInjuryDay]
		   ,[DateOfInjuryMonth]
		   ,[DateOfInjuryYear]
		   ,[PrescriptionId]
		   ,[Prescriber]
		   ,[PrescriberNpi]
		   ,[DateFilledDay]
		   ,[DateFilledMonth]
		   ,[DateFilledYear]
		   ,[Ndc]
		   ,[LabelName]
		   ,[RxNumber]
		   ,[BilledAmount]
		   ,[BilledAmountDollars]
		   ,[BilledAmountCents]
		   ,[Quantity]
		   ,[PharmacyName]
		   ,[Address1]
		   ,[Address2]
		   ,[City]
		   ,[PharmacyState]
		   ,[PostalCode]
		   ,[FederalTin]
		   ,[Npi]
		   ,[Nabp]
		)
		SELECT c1.[ClaimID] AS ClaimId,
				p2.BillToName,
				p2.BillToAddress1,
				p2.BillToAddress2,
				p2.BillToCity,
				p2s.StateCode BillToStateCode,
				p2.BillToPostalCode,
				@InvoiceNumber AS InvoiceNumber, -- i1.InvoiceNumber ,
				InvoiceDate = FORMAT(@TodaysLocalDate, 'MM/dd/yyyy'),
				c1.ClaimNumber,
				p3.LastName AS PatientLastName,
				p3.FirstName AS PatientFirstName,
				p3.Address1 AS PatientAddress1,
				p3.Address2 AS PatientAddress2,
				p3.City AS PatientCity,
				s1.StateCode AS PatientStateCode,
				p3.PostalCode AS PatientPostalCode,
				p3.PhoneNumber AS PatientPhoneNumber,
				DateOfBirthDay = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(DAY, p3.DateOfBirth)), 2),
				DateOfBirthMonth = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(MONTH, p3.DateOfBirth)), 2),
				DateOfBirthYear = RIGHT(CONVERT(VARCHAR(4), DATEPART(YEAR, p3.DateOfBirth)), 2),
				IsMale = CAST(CASE WHEN g.GenderCode = 'M' THEN 1 ELSE 0 END AS BIT),
				IsFemale = CAST(CASE WHEN g.GenderCode = 'F' THEN 1 ELSE 0 END AS BIT),
				DateOfInjuryDay = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(DAY, c1.DateOfInjury)), 2),
				DateOfInjuryMonth = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(MONTH, c1.DateOfInjury)), 2),
				DateOfInjuryYear = RIGHT(CONVERT(VARCHAR(4), DATEPART(YEAR, c1.DateOfInjury)), 2),
				[p1].[PrescriptionID] AS PrescriptionId,
				p1.Prescriber,
				p1.PrescriberNPI AS PrescriberNpi,
				DateFilledDay = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(DAY, p1.DateFilled)), 2),
				DateFilledMonth = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(MONTH, p1.DateFilled)), 2),
				DateFilledYear = RIGHT(CONVERT(VARCHAR(4), DATEPART(YEAR, p1.DateFilled)), 2),
				p1.NDC AS Ndc,
				p1.LabelName,
				p1.RxNumber,
				p1.BilledAmount,
				TRY_CAST(ISNULL(FLOOR(p1.BilledAmount), 0) AS INT) AS BilledAmountDollars,
				TRY_CAST(ISNULL(p1.BilledAmount % 1, '0.00') AS VARCHAR(4)) AS [BilledAmountCents],
				p1.Quantity,
				p4.PharmacyName,
				p4.Address1,
				p4.Address2,
				p4.City,
				PharmacyState = phars.StateCode,
				p4.PostalCode,
				p4.FederalTIN AS FederalTin,
				p4.NPI AS Npi,
				CASE WHEN TRY_CONVERT(INT, p4.NABP) < 0 THEN NULL ELSE p4.[NABP] END AS Nabp
		FROM dbo.Prescription AS p1
			INNER JOIN @Prescriptions AS [p] ON [p].[PrescriptionID] = [p1].[PrescriptionID]
			INNER JOIN dbo.Pharmacy AS p4 ON p4.NABP = p1.PharmacyNABP
			INNER JOIN dbo.UsState AS phars ON phars.StateID = p4.StateID
			INNER JOIN dbo.Claim AS c1 ON c1.ClaimID = p1.ClaimID
			INNER JOIN dbo.Payor AS p2 ON p2.PayorID = c1.PayorID
			INNER JOIN dbo.Patient AS p3 ON p3.PatientID = c1.PatientID
			INNER JOIN dbo.Gender AS g ON g.GenderID = p3.GenderID
			LEFT JOIN dbo.Invoice AS i1 ON i1.InvoiceID = p1.InvoiceID
			LEFT JOIN dbo.UsState AS s1 ON s1.StateID = p3.StateID
			LEFT JOIN dbo.UsState AS p2s ON p2.BillToStateID = p2s.StateID;

		-- QA Checks
		INSERT INTO [#DistinctReturnTable]
		(
			[ClaimId]
		   ,[BillToName]
		   ,[BillToAddress1]
		   ,[BillToAddress2]
		   ,[BillToCity]
		   ,[BillToStateCode]
		   ,[BillToPostalCode]
		   ,[InvoiceNumber]
		   ,[InvoiceDate]
		   ,[ClaimNumber]
		   ,[PatientLastName]
		   ,[PatientFirstName]
		   ,[PatientAddress1]
		   ,[PatientAddress2]
		   ,[PatientCity]
		   ,[PatientStateCode]
		   ,[PatientPostalCode]
		   ,[PatientPhoneNumber]
		   ,[DateOfBirthDay]
		   ,[DateOfBirthMonth]
		   ,[DateOfBirthYear]
		   ,[IsMale]
		   ,[IsFemale]
		   ,[DateOfInjuryDay]
		   ,[DateOfInjuryMonth]
		   ,[DateOfInjuryYear]
		)
		SELECT DISTINCT [r].[ClaimId]
				,[r].[BillToName]
				,[r].[BillToAddress1]
				,[r].[BillToAddress2]
				,[r].[BillToCity]
				,[r].[BillToStateCode]
				,[r].[BillToPostalCode]
				,[r].[InvoiceNumber]
				,[r].[InvoiceDate]
				,[r].[ClaimNumber]
				,[r].[PatientLastName]
				,[r].[PatientFirstName]
				,[r].[PatientAddress1]
				,[r].[PatientAddress2]
				,[r].[PatientCity]
				,[r].[PatientStateCode]
				,[r].[PatientPostalCode]
				,[r].[PatientPhoneNumber]
				,[r].[DateOfBirthDay]
				,[r].[DateOfBirthMonth]
				,[r].[DateOfBirthYear]
				,[r].[IsMale]
				,[r].[IsFemale]
				,[r].[DateOfInjuryDay]
				,[r].[DateOfInjuryMonth]
				,[r].[DateOfInjuryYear]
		FROM [#ReturnTable] AS [r]

		IF (SELECT COUNT(*) FROM [#DistinctReturnTable]) > 1
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				SELECT @Msg = N'Error, the grouping fields had a non-distinct value.';
				RAISERROR(@Msg, 16, 1) WITH NOWAIT;
			END

		INSERT INTO [dbo].[InvoiceDocument]
		(
			[InvoiceNumber]
		   ,[PrescriptionID]
		   ,[ClaimID]
		   ,[GeneratedByUserID]
		   ,[InvoiceDate]
		   ,[CreatedOnUTC]
		   ,[UpdatedOnUTC]
		)
		SELECT @InvoiceNumber
			  ,[r].[PrescriptionId]
			  ,[r].[ClaimId]
			  ,@GeneratedByUserID
			  ,@TodaysLocalDate
			  ,@UtcNow
			  ,@UtcNow
		FROM [#ReturnTable] AS [r];

		-- New Logic, make sure we're getting this right.
		DECLARE @InvoiceID INT;
		INSERT INTO [dbo].[Invoice]
		(
		    [InvoiceNumber]
		   ,[InvoiceDate]
		   ,[Amount]
		)
		VALUES
		(
			CONVERT(VARCHAR(100), @InvoiceNumber),
			@TodaysLocalDate,
			(SELECT SUM(rt.[BilledAmount]) FROM [#ReturnTable] AS [rt])
		);
		SET @InvoiceID = SCOPE_IDENTITY();

		-- Now, circle back around to the scripts to update them with the Invoice ID that we just generated.
		UPDATE [p]
		SET [p].[InvoiceID] = @InvoiceID
		FROM [dbo].[Prescription] AS [p]
			 INNER JOIN [#ReturnTable] AS [r] ON [r].[PrescriptionId] = [p].[PrescriptionID];

		-- End new logic.

		SELECT [r].[ClaimId]
			  ,[r].[BillToName]
			  ,[r].[BillToAddress1]
			  ,[r].[BillToAddress2]
			  ,[r].[BillToCity]
			  ,[r].[BillToStateCode]
			  ,[r].[BillToPostalCode]
			  ,[r].[InvoiceNumber]
			  ,[r].[InvoiceDate]
			  ,[r].[ClaimNumber]
			  ,[r].[PatientLastName]
			  ,[r].[PatientFirstName]
			  ,[r].[PatientAddress1]
			  ,[r].[PatientAddress2]
			  ,[r].[PatientCity]
			  ,[r].[PatientStateCode]
			  ,[r].[PatientPostalCode]
			  ,[r].[PatientPhoneNumber]
			  ,[r].[DateOfBirthDay]
			  ,[r].[DateOfBirthMonth]
			  ,[r].[DateOfBirthYear]
			  ,[r].[IsMale]
			  ,[r].[IsFemale]
			  ,[r].[DateOfInjuryDay]
			  ,[r].[DateOfInjuryMonth]
			  ,[r].[DateOfInjuryYear]
			  ,[r].[PrescriptionId]
			  ,[r].[Prescriber]
			  ,[r].[PrescriberNpi]
			  ,[r].[DateFilledDay]
			  ,[r].[DateFilledMonth]
			  ,[r].[DateFilledYear]
			  ,[r].[Ndc]
			  ,[r].[LabelName]
			  ,[r].[RxNumber]
			  ,[r].[BilledAmount]
			  ,[r].[BilledAmountDollars]
			  ,[r].[BilledAmountCents]
			  ,[r].[Quantity]
			  ,[r].[PharmacyName]
			  ,[r].[Address1]
			  ,[r].[Address2]
			  ,[r].[City]
			  ,[r].[PharmacyState]
			  ,[r].[PostalCode]
			  ,[r].[FederalTin]
			  ,[r].[Npi]
			  ,[r].[Nabp]
		FROM [#ReturnTable] AS [r];
	IF (@@TRANCOUNT > 0)
		COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
			ROLLBACK;	
		DECLARE @ErrLine INT = ERROR_LINE()
              , @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
		SET @Msg = FORMATMESSAGE(N'An error occurred: %s Line Number: %u', @ErrMsg, @ErrLine);
		INSERT INTO [util].[NLog]
		(
			[SiteName]
		   ,[Logged]
		   ,[Level]
		   ,[Message]
		   ,[Logger]
		   ,[ServerName]
		   ,[Port]
		   ,[Url]
		   ,[ServerAddress]
		   ,[RemoteAddress]
		   ,[Callsite]
		   ,[Exception]
		)
		VALUES
		(   N'' -- SiteName - nvarchar(200)
		   ,@TodaysLocalDate -- Logged - datetime2(7)
		   ,'Error' -- Level - varchar(5)
		   ,@Msg -- Message - nvarchar(max)
		   ,N'[dbo].[uspGetInvoicingPdfData]' -- Logger - nvarchar(300)
		   ,N'' -- ServerName - nvarchar(200)
		   ,N'' -- Port - nvarchar(100)
		   ,N'' -- Url - nvarchar(2000)
		   ,N'' -- ServerAddress - nvarchar(100)
		   ,N'' -- RemoteAddress - nvarchar(100)
		   ,N'' -- Callsite - nvarchar(300)
		   ,N'' -- Exception - nvarchar(max)
			);
		THROW 50000, @Msg, 0;
    END CATCH;
END
GO

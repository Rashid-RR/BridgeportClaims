SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/6/2019
 Description:       Imports Envision files into the database.
 Example Execute:
                    EXECUTE [etl].[uspImportEnvision]
 =============================================
*/
CREATE PROCEDURE [etl].[uspImportEnvision] @Base [dbo].[udtEnvision] READONLY
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	SET DEADLOCK_PRIORITY HIGH;
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	BEGIN TRY
        BEGIN TRAN;

		DECLARE @UtcNow DATETIME2 = dtme.udfGetDate();

		MERGE etl.EnvisionStaging AS tgt USING
		(
			SELECT CarrierID,
				GroupID,
				LocationCode,
				MemberID,
				PersonCode,
				LastName,
				FirstName,
				RelCode,
				DOB,
				Gender,
				SSN,
				HICN,
				Subgroup,
				FillDate,
				WrittenDate,
				ProcessDate,
				ProcessTime,
				PharmacyNPI,
				PharmacyName,
				SubmittedPrescriberID,
				AltPrescriberID,
				RxNumber,
				QTY,
				DS,
				FillNumber,
				TranType,
				DAW,
				Compound,
				OtherCoverageCode,
				RxOrigin,
				MPANumber,
				MarkScriptAs,
				AuthNumber,
				ReversedAuth,
				NDC,
				GPI,
				DrugName,
				MONY,
				DEAClass,
				Tier,
				IngredCost,
				DispFee,
				SalesTax,
				VaccineAdminFee,
				MemberCostShareCopay,
				GroupBillAmount,
				DeductibleAmount,
				MemberOOP,
				BenefitAmount,
				SDCIND1,
				SDCVALUE1,
				SDCIND2,
				SDCVALUE2,
				SDCIND3,
				SDCVALUE3,
				SDCIND4,
				SDCVALUE4,
				SDCIND5,
				SDCVALUE5
			FROM @Base
		) AS src ON 1 = 0
		WHEN NOT MATCHED THEN
		INSERT
		( CarrierID, GroupID, LocationCode,	MemberID, PersonCode, LastName,	FirstName, RelCode, DOB, Gender, SSN, HICN,	Subgroup, FillDate, WrittenDate, ProcessDate, ProcessTime,
			PharmacyNPI, PharmacyName, SubmittedPrescriberID, AltPrescriberID, RxNumber, QTY, DS, FillNumber, TranType,	DAW, Compound, OtherCoverageCode, RxOrigin, MPANumber,
			MarkScriptAs, AuthNumber, ReversedAuth,	NDC, GPI, DrugName, MONY, DEAClass,	Tier, IngredCost, DispFee, SalesTax, VaccineAdminFee, MemberCostShareCopay,
			GroupBillAmount, DeductibleAmount, MemberOOP, BenefitAmount, SDCIND1, SDCVALUE1, SDCIND2, SDCVALUE2, SDCIND3, SDCVALUE3, SDCIND4, SDCVALUE4, SDCIND5, SDCVALUE5
		)
		VALUES (src.CarrierID, src.GroupID,src.LocationCode,src.MemberID,RIGHT(src.PersonCode, 2),src.LastName,src.FirstName,src.RelCode,src.DOB,src.Gender,src.SSN,src.HICN,src.Subgroup,src.FillDate,
				src.WrittenDate,src.ProcessDate,src.ProcessTime,src.PharmacyNPI,src.PharmacyName,src.SubmittedPrescriberID,src.AltPrescriberID,src.RxNumber,src.QTY,src.DS,src.FillNumber,
				src.TranType,src.DAW,src.Compound,src.OtherCoverageCode,src.RxOrigin,src.MPANumber,src.MarkScriptAs,src.AuthNumber,src.ReversedAuth,src.NDC,src.GPI,src.DrugName,src.MONY,
				src.DEAClass,src.Tier,src.IngredCost,src.DispFee,src.SalesTax,src.VaccineAdminFee,src.MemberCostShareCopay,src.GroupBillAmount,src.DeductibleAmount,src.MemberOOP,
				src.BenefitAmount,src.SDCIND1,src.SDCVALUE1,src.SDCIND2,src.SDCVALUE2,src.SDCIND3,src.SDCVALUE3,src.SDCIND4,src.SDCVALUE4,src.SDCIND5,src.SDCVALUE5);

		CREATE TABLE #EnvisionETLPatient (RowID INT IDENTITY NOT NULL PRIMARY KEY CLUSTERED, ActionTaken VARCHAR(50) NOT NULL, TargetPatientID INT NULL, TargetLastName VARCHAR(155) NULL,
										TargetFirstName VARCHAR(155) NULL,	TargetGenderID INT NULL, TargetDateOfBirth DATE NULL, SourcePatientID INT NOT NULL, SourceLastName VARCHAR(155) NOT NULL,
										SourceFirstName VARCHAR(155) NOT NULL, SourceGenderID INT NOT NULL, SourceDateOfBirth DATE NULL);
		CREATE TABLE #EnvisionETLClaim (RowID INT IDENTITY NOT NULL PRIMARY KEY CLUSTERED, ActionTaken VARCHAR(50) NOT NULL, TargetClaimID INT NULL, TargetJurisdictionStateID INT NULL, TargetClaimNumber VARCHAR(255) NULL,
										TargetPersonCode CHAR(2) NULL, TargetIsFirstParty BIT NULL, TargetRelationCode TINYINT NULL, TargetPatientID INT NULL, TargetIsMaxBalance BIT NULL,
										TargetPayorID INT NULL, SourceClaimID INT NOT NULL, SourceJurisdictionStateID INT NULL, SourceClaimNumber VARCHAR(255) NOT NULL, SourcePersonCode CHAR(2) NULL,
										SourceIsFirstParty BIT NOT NULL, SourceRelationCode TINYINT NULL, SourcePatientID INT NOT NULL, SourceIsMaxBalance BIT NOT NULL, SourcePayorID INT NOT NULL);
		CREATE TABLE #EnvisionETLPharmacy (RowID INT IDENTITY NOT NULL PRIMARY KEY CLUSTERED, ActionTaken VARCHAR(50) NOT NULL, TargetNABP VARCHAR(7) NULL, TargetPharmacyName VARCHAR(60) NULL,
										TargetNPI VARCHAR(10) NULL, TargetStateID INT NULL, SourceNABP VARCHAR(7) NOT NULL, SourcePharmacyName VARCHAR(60) NOT NULL, SourceNPI VARCHAR(10) NULL,
										SourceStateID INT NOT NULL);

		-- Here comes the fun....
		WITH PatientsCTE AS
		(
			SELECT DISTINCT	e.LastName,
					e.FirstName,
					TRY_CONVERT(DATE, e.DOB) DOB,
					g.GenderID,
					@UtcNow Created,
					@UtcNow Updated
			FROM	etl.EnvisionStaging AS e
					LEFT JOIN dbo.Gender AS g ON g.GenderCode = e.Gender
			WHERE	e.IsImported = 0
		)
		MERGE INTO dbo.Patient AS tgt USING PatientsCTE AS src ON ISNULL(src.DOB, '') = ISNULL(tgt.DateOfBirth, '') AND src.FirstName = tgt.FirstName AND src.LastName = tgt.LastName
		WHEN MATCHED
			THEN UPDATE SET tgt.UpdatedOnUTC = @UtcNow
		WHEN NOT MATCHED BY TARGET THEN
			INSERT (LastName, FirstName, DateOfBirth, GenderID, CreatedOnUTC, UpdatedOnUTC)
			VALUES (src.LastName, src.FirstName, src.DOB, src.GenderID, src.Created, src.Updated)
		OUTPUT  $action, 
				Deleted.PatientID AS TargetPatientID, 
				Deleted.LastName AS TargetLastName, 
				Deleted.FirstName AS TargetFirstName,
				Deleted.GenderID AS TargetGenderID,
				Deleted.DateOfBirth AS TargetDateOfBirth,
				Inserted.PatientID AS SourcePatientID, 
				Inserted.LastName AS SourceLastName, 
				Inserted.FirstName AS SourceFirstName,
				Inserted.GenderID AS SourceGenderID,
				Inserted.DateOfBirth AS SourceDateOfBirth
		INTO #EnvisionETLPatient (ActionTaken, TargetPatientID, TargetLastName, TargetFirstName, TargetGenderID, TargetDateOfBirth, SourcePatientID, SourceLastName, SourceFirstName, SourceGenderID, SourceDateOfBirth);

		-- Update etl.EnvisionStaging with the PatientID.
		UPDATE	es SET es.PatientID = e.SourcePatientID
		FROM	etl.EnvisionStaging AS es
				INNER JOIN #EnvisionETLPatient e ON es.LastName = e.SourceLastName
		WHERE	es.FirstName = e.SourceFirstName
				AND TRY_CONVERT(DATE, es.DOB) = e.SourceDateOfBirth;

		MERGE dbo.Claim AS tgt USING
		(
			SELECT DISTINCT	us.StateID JurisdictionStateID,
					es.MemberID,
					es.PersonCode,
					1 IsFirstParty,
					TRY_CONVERT(TINYINT, es.RelCode) RelCode,
					es.PatientID,
					0 AS IsMaxBalance,
					-1 AS PayorID, -- "Unknown" Payor
					@UtcNow Created,
					@UtcNow Updated
			FROM	etl.EnvisionStaging AS es
					LEFT JOIN dbo.UsState AS us ON us.StateName = es.GroupID
			WHERE	es.IsImported = 0
		) AS src ON src.MemberID = tgt.ClaimNumber AND src.PersonCode = tgt.PersonCode
		WHEN MATCHED
			THEN UPDATE SET tgt.UpdatedOnUTC = @UtcNow
		WHEN NOT MATCHED BY TARGET THEN
			INSERT (
						JurisdictionStateID,
						ClaimNumber,
						PersonCode,
						IsFirstParty,
						RelationCode,
						PatientID,
						IsMaxBalance,
						PayorID,
						CreatedOnUTC,
						UpdatedOnUTC
					)
			VALUES (src.JurisdictionStateID, src.MemberID, src.PersonCode, src.IsFirstParty, src.RelCode, src.PatientID, src.IsMaxBalance,
					src.PayorID, src.Created, src.Updated)
		OUTPUT  $action,
				Deleted.ClaimID AS TargetClaimID,
				Deleted.JurisdictionStateID AS TargetJurisdictionStateID,
				Deleted.ClaimNumber AS TargetClaimNumber,
				Deleted.PersonCode AS TargetPersonCode,
				Deleted.IsFirstParty AS TargetIsFirstParty,
				Deleted.RelationCode AS TargetRelationCode,
				Deleted.PatientID AS TargetPatientID,
				Deleted.IsMaxBalance AS TargetIsMaxBalance,
				Deleted.PayorID AS TargetPayorID,
				Inserted.ClaimID AS SourceClaimID,
				Inserted.JurisdictionStateID AS SourceJurisdictionStateID,
				Inserted.ClaimNumber AS SourceClaimNumber,
				Inserted.PersonCode AS SourcePersonCode,
				Inserted.IsFirstParty AS SourceIsFirstParty,
				Inserted.RelationCode AS SourceRelationCode,
				Inserted.PatientID AS SourcePatientID,
				Inserted.IsMaxBalance AS SourceIsMaxBalance,
				Inserted.PayorID AS SourcePayorID
		INTO #EnvisionETLClaim (ActionTaken, TargetClaimID, TargetJurisdictionStateID, TargetClaimNumber, TargetPersonCode, TargetIsFirstParty, TargetRelationCode, TargetPatientID, TargetIsMaxBalance, TargetPayorID, SourceClaimID, SourceJurisdictionStateID, SourceClaimNumber, SourcePersonCode, SourceIsFirstParty, SourceRelationCode, SourcePatientID, SourceIsMaxBalance, SourcePayorID);

		-- Update etl.EnvisionStaging with the ClaimID.
		UPDATE	es SET es.ClaimID = e.SourceClaimID
		FROM	etl.EnvisionStaging AS es
				INNER JOIN #EnvisionETLClaim e ON e.SourceClaimNumber = es.MemberID
		WHERE	e.SourcePersonCode = es.PersonCode;

		MERGE dbo.Pharmacy AS tgt USING
		(
			SELECT  e.PharmacyNPI,
					e.PharmacyName,
					-1 StateID,
					@UtcNow Created,
					@UtcNow Updated,
					TRY_CONVERT(VARCHAR(7), MIN(e.RowID) * -1) AS PharmacyNABP
			FROM etl.EnvisionStaging AS e
			WHERE e.IsImported = 0
			GROUP BY e.PharmacyNPI, e.PharmacyName
		) AS src ON src.PharmacyNPI = tgt.NPI
		WHEN MATCHED
			THEN UPDATE SET tgt.UpdatedOnUTC = @UtcNow
		WHEN NOT MATCHED BY TARGET THEN
			INSERT (
					NABP,
					PharmacyName,
					NPI,
					StateID,
					CreatedOnUTC,
					UpdatedOnUTC
					)
			VALUES (src.PharmacyNABP,src.PharmacyName,src.PharmacyNPI,src.StateID,src.Created,src.Updated)
		OUTPUT  $action,
				Deleted.NABP AS TargetNABP,
				Deleted.PharmacyName AS TargetPharmacyName,
				Deleted.NPI AS TargetNPI,
				Deleted.StateID AS TargetStateID,
				Inserted.NABP AS SourceNABP,
				Inserted.PharmacyName AS SourcePharmacyName,
				Inserted.NPI AS SourceNPI,
				Inserted.StateID AS SourceStateID
		INTO #EnvisionETLPharmacy (ActionTaken, TargetNABP, TargetPharmacyName, TargetNPI, TargetStateID, SourceNABP, SourcePharmacyName, SourceNPI, SourceStateID);

		UPDATE es SET es.PharmacyNABP = e.SourceNABP FROM etl.EnvisionStaging AS es INNER JOIN #EnvisionETLPharmacy AS e ON e.SourceNPI = es.PharmacyNPI;

		DECLARE @Scripts TABLE (PrescriptionID INT NOT NULL PRIMARY KEY CLUSTERED, RowID INT NOT NULL);
		
		DECLARE @ImportTypeID INT = [etl].[udfGetImportTypeByCode]('ENVISION');

		MERGE dbo.Prescription AS tgt USING
        (
			SELECT e.ClaimID,
			   TRY_CONVERT(DATE, e.FillDate) AS FillDate
			   ,TRY_CONVERT(DATE, e.ProcessDate) AS ProcessDate
			   ,e.SubmittedPrescriberID
			   ,e.AltPrescriberID
			   ,e.RxNumber
			   ,e.QTY
			   ,e.DS
			   ,e.FillNumber
			   ,e.DAW
			   ,e.Compound
			   ,COALESCE(NULLIF(e.AuthNumber, ''), NULLIF(e.ReversedAuth, '')) AS ETLRowID
			   ,e.NDC
			   ,e.GPI
			   ,e.DrugName
			   ,e.MONY
			   ,CASE WHEN e.MONY = 'Y' THEN 'Y' ELSE 'N' END AS Generic
			   ,e.IngredCost
			   ,e.DispFee
			   ,e.SalesTax
			   ,e.GroupBillAmount PayableAmount
			   ,0 BilledAmount
			   ,e.TranType TransactionType
			   ,'' TranID
			   ,@ImportTypeID ImportTypeID
			   ,@UtcNow Created
			   ,@UtcNow Updated
			   ,e.PharmacyNABP
			   ,e.RowID
		FROM etl.EnvisionStaging AS e
		WHERE e.IsImported = 0
		) AS src ON 1 = 0
		WHEN NOT MATCHED THEN
		INSERT
		( ClaimID, DateFilled, DateSubmitted, DEA, PrescriberNPI, RxNumber,	Quantity, DaySupply, RefillNumber, DAW,	Compound, ETLRowID, NDC, GPI, LabelName, MONY,
			Generic, BillIngrCost, BillDispFee,	BilledTax, PayableAmount, BilledAmount,	TransactionType, TranID, ImportTypeID, CreatedOnUTC, UpdatedOnUTC, PharmacyNABP
		)
		VALUES (src.ClaimID, src.FillDate, src.ProcessDate, src.AltPrescriberID, src.SubmittedPrescriberID, src.RxNumber, src.QTY, src.DS, src.FillNumber, src.DAW,
				src.Compound, src.ETLRowID, src.NDC, src.GPI, src.DrugName, src.MONY, src.Generic, src.IngredCost, src.DispFee, src.SalesTax, src.PayableAmount,
				src.BilledAmount, src.TransactionType, src.TranID, src.ImportTypeID, src.Created, src.Updated, src.PharmacyNABP)
		OUTPUT Inserted.PrescriptionID, src.RowID INTO @Scripts (PrescriptionID, RowID);

		UPDATE es SET es.PrescriptionID = s.PrescriptionID FROM etl.EnvisionStaging AS es INNER JOIN @Scripts AS s ON s.RowID = es.RowID;

		UPDATE e SET e.IsImported = 1 FROM etl.EnvisionStaging AS e WHERE e.IsImported = 0;

		-- Generate Notifications
		DECLARE @TodayLocal DATE = CONVERT(DATE, [dtme].[udfGetLocalDate]());
		DECLARE @NotificationTypeID INT = [dbo].[udfGetNotificationTypeIDFromCode]('ENVISIONINFO');
		INSERT [dbo].[Notification]
		(
			[MessageText]
		   ,[GeneratedDate]
		   ,[NotificationTypeID]
		   ,[PrescriptionID]
		)
		SELECT 'A new Envision Claim #: ' + [c].[ClaimNumber]
			   + ' has been imported. Claimant: ' + [pat].[LastName] + ', ' + [pat].[FirstName] + '. Rx Date: ' + FORMAT([p].[DateFilled], 'MM/dd/yyyy') + '. Rx #: ' + [p].[RxNumber] + '. Label: ' + ISNULL([p].[LabelName], '')
			   + ' needs a Billed Amount' + CASE WHEN [pay].[PayorID] = -1 THEN ' and a Carrier.' ELSE '' END
			  ,@TodayLocal
			  ,@NotificationTypeID
			  ,[p].[PrescriptionID]
		FROM [dbo].[Prescription] AS [p]
			 INNER JOIN [dbo].[Claim] AS [c] ON [c].[ClaimID] = [p].[ClaimID]
			 INNER JOIN [dbo].[Payor] AS [pay] ON [pay].[PayorID] = [c].[PayorID]
			 INNER JOIN [dbo].[Patient] AS [pat] ON [pat].[PatientID] = [c].[PatientID]
		WHERE [p].[ImportTypeID] = @ImportTypeID
			 AND NOT EXISTS (SELECT * FROM [dbo].[Notification] AS [n] WHERE [n].[PrescriptionID] = [p].[PrescriptionID]);

		IF EXISTS
        (
			SELECT [n].[MessageText]
				  ,COUNT(*) [Cnt]
			FROM [dbo].[Notification] AS [n]
			GROUP BY [n].[MessageText]
			HAVING COUNT(*) > 1
		)
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				RAISERROR(N'Error. The generation of new notifications produced duplicates.', 16, 1) WITH NOWAIT;
				RETURN -1;
			END

	IF (@@TRANCOUNT > 0)
		COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
			ROLLBACK;	
		DECLARE @ErrLine INT = ERROR_LINE()
              , @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
		DECLARE @Msg NVARCHAR(2000) = FORMATMESSAGE(N'An error occurred: %s Line Number: %u', @ErrMsg, @ErrLine);
		INSERT INTO util.NLog
		(
		    SiteName,
		    Logged,
		    Level,
		    Message,
		    Logger,
		    ServerName,
		    Port,
		    Url,
		    ServerAddress,
		    RemoteAddress,
		    Callsite,
		    Exception
		)
		VALUES
		(   N'',           -- SiteName - nvarchar(200)
		    SYSDATETIME(), -- Logged - datetime2(7)
		    'Error',            -- Level - varchar(5)
		    @Msg,           -- Message - nvarchar(max)
		    N'[etl].[uspImportEnvision]',           -- Logger - nvarchar(300)
		    N'',           -- ServerName - nvarchar(200)
		    N'',           -- Port - nvarchar(100)
		    N'',           -- Url - nvarchar(2000)
		    N'',           -- ServerAddress - nvarchar(100)
		    N'',           -- RemoteAddress - nvarchar(100)
		    N'',           -- Callsite - nvarchar(300)
		    N''            -- Exception - nvarchar(max)
		    );
		THROW 50000, @Msg, 0;
    END CATCH;
END
GO

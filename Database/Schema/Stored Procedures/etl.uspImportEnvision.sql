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
		DECLARE @ImportTypeID INT = [etl].[udfGetImportTypeByCode]('ENVISION');
		DECLARE @EnvisionETL TABLE (RowID INT NOT NULL PRIMARY KEY CLUSTERED);

		INSERT INTO etl.EnvisionStaging
		(
			CarrierID,
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
		) OUTPUT Inserted.RowID INTO @EnvisionETL
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
		FROM @Base;

		DECLARE @EnvisionETLPatient TABLE (RowID INT NOT NULL PRIMARY KEY CLUSTERED, PatientID INT NOT NULL);

		-- Here comes the fun....
		MERGE INTO dbo.Patient USING
		(
			SELECT e.LastName,
				   e.FirstName,
				   TRY_CONVERT(DATE, e.DOB) DOB,
				   g.GenderID,
				   @UtcNow Created,
				   @UtcNow Updated,
				   e.RowID
			FROM etl.EnvisionStaging AS e
				 INNER JOIN @EnvisionETL AS etl ON etl.RowID = e.RowID
				 LEFT JOIN dbo.Gender AS g ON g.GenderCode = e.Gender
		) AS src ON 1 = 0 -- Force a False so an INSERT will happen
		WHEN NOT MATCHED THEN
			INSERT (LastName, FirstName, DateOfBirth, GenderID, CreatedOnUTC, UpdatedOnUTC)
			VALUES (src.LastName, src.FirstName, src.DOB, src.GenderID, src.Created, src.Updated)
		OUTPUT src.RowID, Inserted.PatientID INTO @EnvisionETLPatient (RowID, PatientID);

		DECLARE @EnvisionETLClaim TABLE (RowID INT NOT NULL PRIMARY KEY CLUSTERED, ClaimID INT NOT NULL);
		SELECT * FROM dbo.Payor AS p
		MERGE dbo.Claim USING
		(
			SELECT us.StateID JurisdictionStateID,
				   es.MemberID,
				   es.PersonCode,
				   1 IsFirstParty,
				   TRY_CONVERT(TINYINT, es.RelCode) RelCode,
				   p.PatientID,
				   0 AS IsMaxBalance,
				   -1 AS PayorID, -- Create a "Unknown" Payor record of -1.
				   @UtcNow Created,
				   @UtcNow Updated,
				   es.RowID
			FROM etl.EnvisionStaging AS es
				 INNER JOIN @EnvisionETL AS etl ON etl.RowID = es.RowID
				 INNER JOIN @EnvisionETLPatient p ON p.RowID = es.RowID
				 LEFT JOIN dbo.UsState AS us ON us.StateName = es.GroupID
		) AS src ON 1 = 0 -- Force a False so that an INSERT will happen.
		WHEN NOT MATCHED THEN
			INSERT (
						JurisdictionStateID,
						ClaimNumber,
						PersonCode,
						IsFirstParty,
						RelationCode,
						PatientID,
						IsMaxBalance,
						PayorID, -- Don't have Payor
						CreatedOnUTC,
						UpdatedOnUTC
					)
			VALUES (src.JurisdictionStateID, src.MemberID, src.PersonCode, src.IsFirstParty, src.RelCode, src.PatientID, src.IsMaxBalance,
					src.PayorID, src.Created, src.Updated)
		OUTPUT src.RowID, Inserted.ClaimID INTO @EnvisionETLClaim (RowID, ClaimID);

		INSERT INTO dbo.Prescription
		(
			ClaimID,
			DateFilled,
			DateSubmitted,
			-- Enter the NABP for this Pharmacy
			-- This should go into the Pharmacy Table, which DOES have this fields already.
			-- Column18 (Pharmacy NPI) - we don't have a field for this yet.  Can we add a new field to the Prescription table called PharmacyNPI (varchar(10), null) to store this info?
			-- Column19 (Pharmacy Name) - will you add another field to the Prescription table called PharmacyName to store anything submitted in this field.  (varchar(60), null)
			-- Import to the Pharmcy table and Prescription table.
			DEA,
			PrescriberNPI,
			RxNumber,	  
			Quantity,
			DaySupply,
			RefillNumber,
			DAW,
			Compound,
			ETLRowID,
			NDC,
			GPI,
			LabelName,
			MONY,
			Generic,
			BillIngrCost,
			BillDispFee,
			BilledTax,
			PayableAmount,
			BilledAmount,
			TransactionType,
			TranID,
			ImportTypeID,
			CreatedOnUTC,
			UpdatedOnUTC,
			PharmacyNABP -- NULLABLE
		)
		SELECT c.ClaimID,
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
			   ,COALESCE(NULLIF(e.AuthNumber, ''), NULLIF(e.ReversedAuth, ''))
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
			   ,@UtcNow
			   ,@UtcNow
			   ,NULL
		FROM etl.EnvisionStaging AS e
			 INNER JOIN @EnvisionETL AS etl ON etl.RowID = e.RowID
			 INNER JOIN @EnvisionETLClaim AS c ON c.RowID = e.RowID

		UPDATE e SET e.IsImported = 1 FROM etl.EnvisionStaging AS e INNER JOIN @EnvisionETL etl ON etl.RowID = e.RowID;

	IF (@@TRANCOUNT > 0)
		COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
			ROLLBACK;	
		DECLARE @ErrLine INT = ERROR_LINE()
              , @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
		DECLARE @Msg NVARCHAR(2000) = FORMATMESSAGE(N'An error occurred: %s Line Number: %u', @ErrMsg, @ErrLine);
		THROW 50000, @Msg, 0;
    END CATCH
	
END
GO

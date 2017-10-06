SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	10/3/2017
	Description:	Adds system constraints in order to generate test data.
	Sample Execute:
					EXEC etl.uspAddConstraintsToStagedLakerFile
*/
CREATE PROC [etl].[uspAddConstraintsToStagedLakerFile]
AS BEGIN
	SET NOCOUNT ON;
	ALTER TABLE etl.StagedLakerFile ALTER COLUMN PayorID INT NOT NULL;
	ALTER TABLE etl.StagedLakerFile WITH CHECK ADD CONSTRAINT fkfkfkfkfk FOREIGN KEY (PayorID) REFERENCES dbo.Payor (PayorID)
	ALTER TABLE etl.StagedLakerFile ALTER COLUMN ClaimID INT NOT NULL;
	ALTER TABLE etl.StagedLakerFile WITH CHECK ADD CONSTRAINT fkdf FOREIGN KEY (ClaimID) REFERENCES dbo.Claim (ClaimID)
	ALTER TABLE etl.StagedLakerFile WITH CHECK ADD CONSTRAINT shoe FOREIGN KEY (AdjustorID) REFERENCES dbo.Adjustor (AdjustorID)
	ALTER TABLE etl.StagedLakerFile ALTER COLUMN PrescriptionID INT NOT NULL;
	ALTER TABLE etl.StagedLakerFile WITH CHECK ADD CONSTRAINT sfooo FOREIGN KEY (PrescriptionID) REFERENCES dbo.Prescription (PrescriptionID)
	ALTER TABLE etl.StagedLakerFile ALTER COLUMN PatientID INT NOT NULL;
	ALTER TABLE etl.StagedLakerFile WITH CHECK ADD CONSTRAINT kdjfksjkd FOREIGN KEY (PatientID) REFERENCES dbo.Patient (PatientID)
	ALTER TABLE etl.StagedLakerFile WITH CHECK ADD CONSTRAINT dfjidsjijdiji FOREIGN KEY (AcctPayableID) REFERENCES dbo.AcctPayable (AcctPayableID)
	ALTER TABLE etl.StagedLakerFile WITH CHECK ADD CONSTRAINT dss FOREIGN KEY (InvoiceID) REFERENCES dbo.Invoice (InvoiceID)
	ALTER TABLE etl.StagedLakerFile WITH CHECK ADD CONSTRAINT fdjijijiji FOREIGN KEY (NABP) REFERENCES dbo.Pharmacy
	EXEC util.uspRenameForeignKeys
END
GO

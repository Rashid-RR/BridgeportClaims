SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/11/2017
	Description:	Adds the necessary columns for ETL.
	Sample Execute:
					EXEC [etl].[uspAddStagedLakerFileETLColumns]
*/
CREATE PROC [etl].[uspAddStagedLakerFileETLColumns]
AS BEGIN
	SET NOCOUNT ON;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;
	SET DEADLOCK_PRIORITY HIGH;
	BEGIN TRY;
		BEGIN TRANSACTION;
		DECLARE @Obj INTEGER = OBJECT_ID(N'etl.StagedLakerFile', N'U')
		IF NOT EXISTS
		(
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'PayorID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] ADD [PayorID] INTEGER NULL
		IF NOT EXISTS
	    (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'AdjustorID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] ADD [AdjustorID] INTEGER NULL
		IF NOT EXISTS
	    (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'PatientID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] ADD [PatientID] INTEGER NULL
		IF NOT EXISTS
	    (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'InvoiceID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] ADD [InvoiceID] INTEGER NULL
		IF NOT EXISTS
	    (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'ClaimID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] ADD [ClaimID] INTEGER NULL
		IF NOT EXISTS
	    (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'PrescriptionID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] ADD [PrescriptionID] INTEGER NULL
		IF NOT EXISTS
	    (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'PharmacyID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] ADD [PharmacyID] INTEGER NULL
		IF NOT EXISTS
		(
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'StageID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] ADD [StageID] INTEGER IDENTITY
		-- Payor
		IF NOT EXISTS
        (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'StageID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.Payor', N'U')
		)
			ALTER TABLE dbo.[Payor] ADD StageID INTEGER NULL
		-- Patient
		IF NOT EXISTS
        (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'StageID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.Patient', N'U')
		)
			ALTER TABLE dbo.[Patient] ADD StageID INTEGER NULL
		-- Claim
		IF NOT EXISTS
        (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'StageID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.Claim', N'U')
		)
			ALTER TABLE dbo.[Claim] ADD StageID INTEGER NULL
		-- Invoice
		IF NOT EXISTS
        (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'StageID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.Invoice', N'U')
		)
			ALTER TABLE dbo.[Invoice] ADD StageID INTEGER NULL
		-- Pharmacy
		IF NOT EXISTS
        (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'StageID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.Pharmacy', N'U')
		)
			ALTER TABLE dbo.[Pharmacy] ADD StageID INTEGER NULL
		IF (@@TRANCOUNT > 0)
			COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END

GO

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
		/****************************************************************************
		First, we're going to add all of the columns from the import tables to the staging table.
		****************************************************************************/
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
			WHERE [c].[name] = 'AcctPayableID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] ADD [AcctPayableID] INTEGER NULL
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
			WHERE [c].[name] = 'NABP'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] ADD NABP VARCHAR(7) NULL
		/****************************************************************************
		Now, we're going to move on to the import tables themselves, and permanently add Columns for the Import
		****************************************************************************/
		-- Payor
		IF NOT EXISTS
        (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'ETLRowID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.Payor', N'U')
		)
			ALTER TABLE dbo.[Payor] ADD ETLRowID VARCHAR(50) NULL
		-- Adjustor
		IF NOT EXISTS
        (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'ETLRowID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.Adjustor', N'U')
		)
			ALTER TABLE dbo.[Adjustor] ADD ETLRowID VARCHAR(50) NULL
		-- Patient
		IF NOT EXISTS
        (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'ETLRowID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.Patient', N'U')
		)
			ALTER TABLE dbo.[Patient] ADD ETLRowID VARCHAR(50) NULL
		-- Claim
		IF NOT EXISTS
        (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'ETLRowID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.Claim', N'U')
		)
			ALTER TABLE dbo.[Claim] ADD ETLRowID VARCHAR(50) NULL
		-- Invoice
		IF NOT EXISTS
        (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'ETLRowID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.Invoice', N'U')
		)
			ALTER TABLE dbo.[Invoice] ADD ETLRowID VARCHAR(50) NULL
		-- Pharmacy
		IF NOT EXISTS
        (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'ETLRowID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.Pharmacy', N'U')
		)
			ALTER TABLE dbo.[Pharmacy] ADD ETLRowID VARCHAR(50) NULL
		-- Prescription
		IF NOT EXISTS
        (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'ETLRowID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.Prescription', N'U')
		)
			ALTER TABLE dbo.[Prescription] ADD ETLRowID VARCHAR(50) NULL
		-- Payment
		IF NOT EXISTS
        (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'ETLRowID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.AcctPayable', N'U')
		)
			ALTER TABLE dbo.[AcctPayable] ADD ETLRowID VARCHAR(50) NULL
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

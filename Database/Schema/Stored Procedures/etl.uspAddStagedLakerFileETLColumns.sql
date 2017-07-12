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
			WHERE [c].[name] = 'StageID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] ADD [StageID] INTEGER IDENTITY
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

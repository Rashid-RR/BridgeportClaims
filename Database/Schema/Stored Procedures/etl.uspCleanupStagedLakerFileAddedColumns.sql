SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/11/2017
	Description:	Removes all of the columns necessary to setup the "StagedLakerFile" table for ETL.
	Sample Execute:
					EXEC [etl].[uspCleanupStagedLakerFileAddedColumns]
*/
CREATE PROC [etl].[uspCleanupStagedLakerFileAddedColumns]
AS BEGIN
	SET NOCOUNT ON;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;
	SET DEADLOCK_PRIORITY HIGH;
	BEGIN TRY
		BEGIN TRANSACTION;
		DECLARE @Obj INT = OBJECT_ID(N'etl.StagedLakerFile', N'U')
		IF EXISTS
		(
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'PayorID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] DROP COLUMN [PayorID]
		IF EXISTS
	    (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'AdjustorID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] DROP COLUMN [AdjustorID]
		IF EXISTS
	    (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'PatientID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] DROP COLUMN [PatientID]
		IF EXISTS
	    (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'InvoiceID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] DROP COLUMN [InvoiceID]
		IF EXISTS
	    (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'ClaimID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] DROP COLUMN [ClaimID]
		IF EXISTS
	    (
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'PrescriptionID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] DROP COLUMN [PrescriptionID]
		IF EXISTS
		(
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'StageID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] DROP COLUMN [StageID]
		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END
GO

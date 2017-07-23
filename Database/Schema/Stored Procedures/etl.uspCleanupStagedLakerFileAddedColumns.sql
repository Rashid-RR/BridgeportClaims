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
		DECLARE @PrntMsg NVARCHAR(1000)
				,@TableName SYSNAME
				,@ColumnName SYSNAME
				,@NewLine CHAR(1) = CHAR(10) + CHAR(13)
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
			WHERE [c].[name] = 'PharmacyID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] DROP COLUMN PharmacyID
		IF EXISTS
		(
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'StageID'
				  AND [t].[object_id] = OBJECT_ID(N'dbo.Payor', N'U')
		)
			ALTER TABLE dbo.[Payor] DROP COLUMN StageID
		IF EXISTS
		(
			SELECT * FROM [sys].[columns] AS [c]
			INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
			WHERE [c].[name] = 'StageID'
				  AND [t].[object_id] = @Obj
		)
			ALTER TABLE [etl].[StagedLakerFile] DROP COLUMN [StageID]
		-- Final QA check, ensure that no ETL columns (mainly StageID) are inside any table any longer.
		CREATE TABLE #QA (TableName SYSNAME NOT NULL, ColumnName SYSNAME NOT NULL)
		INSERT [#QA] (  [TableName],[ColumnName])
		SELECT TableName = [t].[name]
			 , ColumnName = [c].[name]
		FROM   [sys].[columns] AS [c]
			   INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
		WHERE  [c].[name] = 'StageID'
		IF EXISTS
        (
			SELECT * FROM [#QA] AS [q]
		)
			BEGIN
				SET @PrntMsg = 'Error. The following table(s) still have a column named "StageID" in them. ';
				DECLARE C CURSOR LOCAL FAST_FORWARD FOR
				SELECT TableName, [ColumnName]
				FROM [#QA] AS [q]
				OPEN [C]
				FETCH NEXT FROM [C] INTO @TableName, @ColumnName
				WHILE @@FETCH_STATUS = 0
					BEGIN
						SET @PrntMsg += 'Table Name: ' + @TableName + '. Column Name: ' + @ColumnName + @NewLine
						RAISERROR(@PrntMsg, 10, 1) WITH NOWAIT
						FETCH NEXT FROM [C] INTO @TableName, @ColumnName
					END
				CLOSE [C]
				DEALLOCATE [C]
			END
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

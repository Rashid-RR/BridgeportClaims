SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/29/2017
	Description:	Proc that takes care of the index defrag and update stats, as well as replaces the
					last, live, online copy of the database with a new backup.
	Sample Execute:
					EXEC util.uspDefragIndexesAndBackupDatabase
*/
CREATE PROCEDURE [util].[uspDefragIndexesAndBackupDatabase]
AS BEGIN
	DECLARE @DBNAME SYSNAME = DB_NAME()
	EXEC [util].[uspIndexOptimize] @Databases = @DBNAME
	    , @FragmentationLow = NULL
	    , @FragmentationMedium = 'INDEX_REORGANIZE,INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE'
	    , @FragmentationHigh = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE'
	    , @FragmentationLevel1 = 5
	    , @FragmentationLevel2 = 30
	    , @LogToTable = 'N'
	    , @UpdateStatistics = 'ALL'
	    , @Indexes = 'ALL_INDEXES'
	DBCC CHECKDB
	DECLARE @SQLStatement NVARCHAR(500)

	IF @@VERSION NOT LIKE '%Azure%' OR (DB_NAME() != 'BridgeportClaims')
		RETURN

	---------------------------------------------------------------
	---------------------- Hard-coded Value -----------------------
	SET @SQLStatement = N'DROP DATABASE [BridgeportClaims20171127]'
	---------------------------------------------------------------
	EXEC [sys].[sp_executesql] @SQLStatement
	DECLARE @Now DATETIME2 = [dtme].[udfGetLocalDateTime](SYSUTCDATETIME())
			, @PrntMsg VARCHAR(1000)
			, @CurrentDB SYSNAME = DB_NAME()
	DECLARE @NewDB SYSNAME = @CurrentDB + CONVERT(VARCHAR(20), @Now, 112)
	SET @PrntMsg = 'Backing up the ' + QUOTENAME(@CurrentDB) + ' database at: ' +
		 FORMAT(@Now, 'M/d/yyyy h:mm tt') + ' to the ' + QUOTENAME(@NewDB) + ' database.'
	RAISERROR(@PrntMsg, 10, 1) WITH NOWAIT
	SET @SQLStatement = N'CREATE DATABASE ' + QUOTENAME(@NewDB) + ' AS COPY OF ' + QUOTENAME(@CurrentDB)
	EXECUTE [sys].[sp_executesql] @SQLStatement
END 
GO

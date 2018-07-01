SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       6/25/2018
 Description:       Performs an cursor that performs an script generation, and subsequent
					execution of those scripts, which were created to REBUILD those indexes, due to 
					the high disk fragmentation that is being reported by the DMV's in the system catalog.
					Also performs an automatic IndexOptimize routine which does something similiar, also 
					updates nearly all statistics, and finally performs a DBCC CHECKDB for an integrity check.
 Example Execute:
                    EXECUTE [util].[uspMaintenance]
 =============================================
*/
CREATE PROCEDURE [util].[uspMaintenance]
(
	@PageCountLimitation INT = 0
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
            
			CREATE TABLE #IndexRebuildScript
			(
				TableName sysname NOT NULL
			   ,IndexName sysname NOT NULL
			   ,IndexType NVARCHAR(60) NOT NULL
			   ,avg_fragmentation_in_percent FLOAT NULL
			   ,Script NVARCHAR(4000) NOT NULL
			);

			INSERT #IndexRebuildScript (TableName, IndexName, IndexType, avg_fragmentation_in_percent, Script)
			SELECT  [TableName] = OBJECT_NAME(ind.object_id)
				   ,[IndexName] = ind.name
				   ,[IndexType] = indexstats.index_type_desc
				   ,indexstats.avg_fragmentation_in_percent
				   ,[Script] = 'ALTER INDEX ' + QUOTENAME(ind.name) + ' ON ' + QUOTENAME(s.name) + '.'
							   + QUOTENAME(OBJECT_NAME(ind.object_id)) + ' REBUILD;'
			FROM    sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, NULL) AS [indexstats]
					INNER JOIN sys.indexes AS [ind] ON ind.object_id = indexstats.object_id
													   AND ind.index_id = indexstats.index_id
					INNER JOIN sys.objects AS [o] ON o.object_id = ind.object_id
					INNER JOIN sys.schemas AS [s] ON o.schema_id = s.schema_id
			WHERE   indexstats.page_count >= @PageCountLimitation
					AND indexstats.index_type_desc != 'HEAP'
					AND indexstats.avg_fragmentation_in_percent > 0
			ORDER BY indexstats.avg_fragmentation_in_percent DESC;

			DECLARE @RebuildIndexScript NVARCHAR(4000) 

			DECLARE RebuildIdx CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
			SELECT i.Script FROM #IndexRebuildScript AS i

			OPEN RebuildIdx

			FETCH NEXT FROM RebuildIdx INTO @RebuildIndexScript

			WHILE @@FETCH_STATUS = 0
			BEGIN
				EXEC sys.sp_executesql @RebuildIndexScript;

				FETCH NEXT FROM RebuildIdx INTO @RebuildIndexScript
			END

			CLOSE RebuildIdx;
			DEALLOCATE RebuildIdx;

			DECLARE @DBNAME SYSNAME = DB_NAME()
			EXEC [util].[uspIndexOptimize] @Databases = @DBNAME
				, @FragmentationLow = NULL
				, @FragmentationMedium = 'INDEX_REORGANIZE,INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE'
				, @FragmentationHigh = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE'
				, @FragmentationLevel1 = 2
				, @FragmentationLevel2 = 20
				, @LogToTable = 'N'
				, @UpdateStatistics = 'ALL'
				, @Indexes = 'ALL_INDEXES'
			DBCC CHECKDB;
            
        IF (@@TRANCOUNT > 0)
            COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
            ROLLBACK;
                
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

		RAISERROR(N'%s (line %d): %s', @ErrSeverity, @ErrState, @ErrProc, @ErrLine, @ErrMsg);
        
    END CATCH
END

GO
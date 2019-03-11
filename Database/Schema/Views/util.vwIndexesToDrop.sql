SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [util].[vwIndexesToDrop]
AS
SELECT 
   DB_NAME() AS [database_name], 
   DB_ID() AS database_id,
   OBJECT_SCHEMA_NAME(i.[object_id]) AS [schema_name], 
   OBJECT_NAME(i.[object_id]) AS [object_name], 
   iu.[object_id],
   i.[name], 
   i.index_id, 
   i.[type_desc],
   i.is_primary_key,
   i.is_unique,
   i.is_unique_constraint,
   iu.user_seeks, 
   iu.user_scans, 
   iu.user_lookups, 
   iu.user_updates,
   iu.user_seeks + iu.user_scans + iu.user_lookups AS total_uses,
   CASE WHEN (iu.user_seeks + iu.user_scans + iu.user_lookups) > 0
        THEN iu.user_updates/( iu.user_seeks + iu.user_scans + iu.user_lookups )
        ELSE iu.user_updates END AS update_to_use_ratio,
N'DROP INDEX ' + QUOTENAME(I.NAME) + ' ON ' + QUOTENAME(OBJECT_SCHEMA_NAME(i.[object_id])) + '.' + QUOTENAME(OBJECT_NAME(i.[object_id])) + ';' DropScript
FROM sys.dm_db_index_usage_stats iu
RIGHT JOIN sys.indexes i ON iu.index_id = i.index_id AND iu.[object_id] = i.[object_id]
WHERE 
   OBJECTPROPERTY(iu.[object_id], 'IsUserTable') = 1
   AND iu.database_id = DB_ID()
   AND [i].[is_primary_key] = 0
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[vwMissingNonclusteredIndexDataCompression]
AS
SELECT [t].[name] AS [Table], [i].[name] AS [Index],  
    [p].[partition_number] AS [Partition],
    [p].[data_compression_desc] AS [Compression],
	'ALTER INDEX ' + QUOTENAME([i].[name]) + ' ON ' + QUOTENAME(s.[name]) + '.' + QUOTENAME([t].[name]) +
	' REBUILD WITH (FILLFACTOR = 90, DATA_COMPRESSION = PAGE)' Script
FROM [sys].[partitions] AS [p]
INNER JOIN sys.tables AS [t] ON [t].[object_id] = [p].[object_id]
INNER JOIN sys.[schemas] AS [s] ON [s].[schema_id] = [t].[schema_id]
INNER JOIN sys.indexes AS [i] ON [i].[object_id] = [p].[object_id] AND [i].[index_id] = [p].[index_id]
WHERE [p].[index_id] > 1
AND [p].[data_compression_desc] != 'PAGE'
GO

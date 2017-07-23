SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[vwMissingClusteredIndexDataCompression]
AS
	SELECT [Table] = [t].[name]
		 , [Partition] = [p].[partition_number]
		 , [Compression] = [p].[data_compression_desc]
	FROM   [sys].[partitions] AS [p]
		   INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [p].[object_id]
	WHERE  [p].[index_id] IN ( 0, 1 ) -- Only Clustered and Heaps
		   AND NOT 1 = CASE WHEN [p].[index_id] = 1
						 AND [p].[data_compression_desc] = 'ROW' THEN 1
					WHEN [p].[index_id] = 0
						 AND [p].[data_compression_desc] = 'NONE' THEN 1
					ELSE 0
			   END
GO

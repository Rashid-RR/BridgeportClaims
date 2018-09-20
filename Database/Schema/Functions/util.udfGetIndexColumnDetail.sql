SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
   =============================================
   Author:            Jordan Gurney
   Create date:       9/18/2018
   Description:       Gets the Sort columns and Included columns in index(es).
   Example Execute:
                      SELECT * FROM [util].[udfGetIndexColumnDetail]('dbo', 'Document');
   =============================================
*/
CREATE FUNCTION [util].[udfGetIndexColumnDetail]
(
    @FilterSchemaName VARCHAR(500) = NULL,
    @FilterTableName VARCHAR(500) = NULL
)
RETURNS @IndexDetail TABLE
(
    [SchemaName] VARCHAR(500)
   ,[TableName] VARCHAR(500)
   ,[IndexName] VARCHAR(500)
   ,[ColumnNames] VARCHAR(4000)
   ,[IncludeColumns] VARCHAR(4000)
   ,[IndexType] VARCHAR(100)
)
AS
    BEGIN
        DECLARE @IndexDetailWindow TABLE
        (
            [SchemaName] VARCHAR(500)
           ,[ObjectID] INT
           ,[TableName] VARCHAR(500)
           ,[IndexID] INT
           ,[IndexName] VARCHAR(500)
           ,[ColumnID] INT
           ,[column_index_id] INT
           ,[ColumnNames] VARCHAR(4000)
           ,[IncludeColumns] VARCHAR(4000)
           ,[NumberOfColumns] INT
           ,[IndexType] VARCHAR(100)
           ,[LastColRecord] INT
        );
        WITH [CTE_Indexes] ([SchemaName], [ObjectID], [TableName], [IndexID], [IndexName], [ColumnID]
                           ,[column_index_id], [ColumnNames], [IncludeColumns], [NumberOfColumns], [IndexType]) AS
        (
            SELECT  [s].[name]
                   ,[t].[object_id]
                   ,[t].[name]
                   ,[i].[index_id]
                   ,[i].[name]
                   ,[c].[column_id]
                   ,[ic].[index_column_id]
                   ,CASE [ic].[is_included_column] WHEN 0 THEN CAST([c].[name] AS VARCHAR(4000))ELSE '' END
                   ,CASE [ic].[is_included_column] WHEN 1 THEN CAST([c].[name] AS VARCHAR(4000))ELSE '' END
                   ,1
                   ,[i].[type_desc]
            FROM    [sys].[schemas] AS [s]
                    JOIN [sys].[tables] AS [t] ON [s].[schema_id] = [t].[schema_id]
                    JOIN [sys].[indexes] AS [i] ON [i].[object_id] = [t].[object_id]
                    JOIN [sys].[index_columns] AS [ic] ON [ic].[index_id] = [i].[index_id]
                                                          AND  [ic].[object_id] = [i].[object_id]
                    JOIN [sys].[columns] AS [c] ON [c].[column_id] = [ic].[column_id]
                                                   AND [c].[object_id] = [ic].[object_id]
                                                   AND [ic].[index_column_id] = 1
            WHERE   [s].[name] = COALESCE(@FilterSchemaName, [s].[name])
                    AND [t].[name] = COALESCE(@FilterTableName, [t].[name])
            UNION ALL
            SELECT  [s].[name]
                   ,[t].[object_id]
                   ,[t].[name]
                   ,[i].[index_id]
                   ,[i].[name]
                   ,[c].[column_id]
                   ,[ic].[index_column_id]
                   ,CASE [ic].[is_included_column] WHEN 0 THEN
                                                       CAST([cte].[ColumnNames] + ', ' + [c].[name] AS VARCHAR(4000))
                                                   ELSE
                                                       [cte].[ColumnNames]
                    END
                   ,CASE WHEN [ic].[is_included_column] = 1
                              AND   [cte].[IncludeColumns] != '' THEN
                             CAST([cte].[IncludeColumns] + ', ' + [c].[name] AS VARCHAR(4000))
                         WHEN [ic].[is_included_column] = 1
                              AND   [cte].[IncludeColumns] = '' THEN
                             CAST([c].[name] AS VARCHAR(4000))
                         ELSE
                             ''
                    END
                   ,[cte].[NumberOfColumns] + 1
                   ,[i].[type_desc]
            FROM    [sys].[schemas] AS [s]
                    INNER JOIN [sys].[tables] AS [t] ON [s].[schema_id] = [t].[schema_id]
                    INNER JOIN [sys].[indexes] AS [i] ON [i].[object_id] = [t].[object_id]
                    INNER JOIN [sys].[index_columns] AS [ic] ON [ic].[index_id] = [i].[index_id] AND  [ic].[object_id] = [i].[object_id]
                    INNER JOIN [sys].[columns] AS [c] ON [c].[column_id] = [ic].[column_id] AND [c].[object_id] = [ic].[object_id]
                    INNER JOIN [CTE_Indexes] AS [cte] ON [cte].[column_index_id] + 1 = [ic].[index_column_id] AND [cte].[IndexID] = [i].[index_id] AND [cte].[ObjectID] = [ic].[object_id]
        )
        INSERT INTO @IndexDetailWindow
        SELECT  [cte].[SchemaName]
               ,[cte].[ObjectID]
               ,[cte].[TableName]
               ,[cte].[IndexID]
               ,[cte].[IndexName]
               ,[cte].[ColumnID]
               ,[cte].[column_index_id]
               ,[cte].[ColumnNames]
               ,[cte].[IncludeColumns]
               ,[cte].[NumberOfColumns]
               ,[cte].[IndexType]
               ,[LastRecord] = RANK() OVER (PARTITION BY [cte].[ObjectID]
                                                        ,[cte].[IndexID]
                                            ORDER BY [cte].[NumberOfColumns] DESC
                                           )
        FROM    [CTE_Indexes] AS [cte];

        INSERT      @IndexDetail ([SchemaName], [TableName], [IndexName], [ColumnNames],
                    [IncludeColumns], [IndexType])
        SELECT      [t].[SchemaName]
                   ,[t].[TableName]
                   ,[t].[IndexName]
                   ,[t].[ColumnNames]
                   ,[t].[IncludeColumns]
                   ,[t].[IndexType]
        FROM        @IndexDetailWindow AS [t]
        WHERE       [t].[LastColRecord] = 1;
        RETURN;
    END
GO

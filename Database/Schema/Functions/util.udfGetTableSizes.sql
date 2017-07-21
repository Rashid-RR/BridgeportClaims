SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:				Jordan Gurney
	Date:				5/8/2017
	Description:		TVF that returns the row count, and data 
						sizes for all tables in the database
	Example Execute:
						SELECT * FROM dbo.udfGetTableSizes()
*/
CREATE FUNCTION [util].[udfGetTableSizes]()
RETURNS @TableSizes TABLE
	(
	 TableName NVARCHAR(150) NOT NULL
	,NumberOfRows BIGINT NOT NULL
	,ReservedSpace VARCHAR(150) NOT NULL
	,DataInKB VARCHAR(150) NOT NULL
	,DataInMB VARCHAR(150) NOT NULL
	,IndexSize VARCHAR(150) NULL
	,UnusedSpace VARCHAR(150) NULL
	)
AS
BEGIN
    DECLARE @PageSize FLOAT
	SELECT @PageSize = 8.000000;

    WITH SpaceUsedCTE AS
	( 
		SELECT   OBJECT_NAME(i.object_id) AS [name]
                ,p.rows
                ,CONVERT(VARCHAR(50) ,@PageSize * SUM(a.total_pages))
                + ' KB' AS [reserved]
                ,CONVERT(VARCHAR(50) ,@PageSize
                * SUM(CASE WHEN a.type <> 1 THEN a.used_pages
                            WHEN p.index_id < 2 THEN a.data_pages
                            ELSE 0
                        END)) AS [data]
                ,CONVERT(VARCHAR(50) ,@PageSize * SUM(a.used_pages
                                                    - CASE
                                                    WHEN a.type <> 1
                                                    THEN a.used_pages
                                                    WHEN p.index_id < 2
                                                    THEN a.data_pages
                                                    ELSE 0
                                                    END)) + ' KB' AS [index_size]
                ,CONVERT(VARCHAR(50) ,@PageSize * SUM(a.total_pages
                                                    - a.used_pages))
                + ' KB' AS [unused]
        FROM     sys.indexes AS i
                INNER JOIN sys.partitions AS p ON p.object_id = i.object_id
                                                    AND p.index_id = i.index_id
                INNER JOIN sys.allocation_units AS a ON a.container_id = p.partition_id
                INNER JOIN sys.tables t ON i.object_id = t.object_id
        WHERE    1 = 1
                AND i.type <= 1
                AND a.type = 1
                AND t.type = 'U'
                AND t.is_ms_shipped = 0
        GROUP BY i.object_id
                ,p.rows
	)
    INSERT  @TableSizes
            (TableName
            ,NumberOfRows
			,ReservedSpace
            ,DataInKB
            ,DataInMB
            ,IndexSize
            ,UnusedSpace
            )
    SELECT  s.name TableName
            ,s.rows NumberOfRows
            ,s.reserved ReservedSpace
            ,s.data + ' KB' DataInKB
            ,CAST(CONVERT(DECIMAL(38 ,2) ,ROUND(CONVERT(INT ,s.data) / 1024.00 ,2)) AS VARCHAR(50)) + ' MB' DataInMB
            ,s.index_size IndexSize
            ,s.unused UnusedSpace
    FROM    SpaceUsedCTE s
	ORDER BY s.[rows] DESC

    RETURN

END

GO

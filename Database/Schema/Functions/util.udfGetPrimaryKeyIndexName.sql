SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

/*
	Author:				Jordan Gurney
	Date:				7/30/2015
	Description:		Used to retrieve the index name of the primary key for a table.
						This is useful when used to rename PK's from system generated names
						to actual names.
	Example Execute:
						SELECT dbo.udf_get_primary_key_index_name('dbo.IncentiveTypes')
*/
CREATE FUNCTION [util].[udfGetPrimaryKeyIndexName]
(
    @TableName nvarchar(1000) = null
)
RETURNS nvarchar(1000)
AS
BEGIN
	DECLARE @index_name nvarchar(1000)

    SELECT @index_name = i.name
	FROM sys.tables AS tbl
	INNER JOIN sys.indexes AS i ON (i.index_id > 0 and i.is_hypothetical = 0) AND (i.object_id=tbl.object_id)
	WHERE (i.is_unique=1 and i.is_disabled=0) and (tbl.name=PARSENAME(@TableName, 1))

    RETURN @index_name
END
GO

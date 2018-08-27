SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:				Jordan Gurney
	Date:				7/13/2015
	Description:		This Function Returns the Primary Key Column name for a table.
	Example Execute:
						SELECT dbo.udfGetPrimaryKeyColumnName('[dbo].[PCGFGLyphs]') PkName
*/
CREATE FUNCTION [util].[udfGetPrimaryKeyColumnName] ( @TableName sysname )
RETURNS NVARCHAR(100)
AS
BEGIN
    IF @TableName LIKE '%[%' OR @TableName LIKE '%]%'
        SET @TableName = REPLACE(REPLACE(@TableName, ']', ''), '[', '')
    DECLARE @RetVal NVARCHAR(100)

    SELECT  @RetVal = COALESCE(@RetVal + ',', '') + i.COLUMN_NAME
    FROM    INFORMATION_SCHEMA.KEY_COLUMN_USAGE i
    WHERE   OBJECTPROPERTY(OBJECT_ID(i.TABLE_SCHEMA + '.' + i.CONSTRAINT_NAME), 'IsPrimaryKey') = 1
            AND i.TABLE_NAME = PARSENAME(@TableName, 1)
            AND i.TABLE_SCHEMA = PARSENAME(@TableName, 2);
    RETURN @RetVal;
END
GO

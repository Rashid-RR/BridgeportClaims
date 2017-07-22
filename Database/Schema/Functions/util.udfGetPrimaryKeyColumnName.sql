SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

/*
	Author:				Jordan Gurney
	Date:				7/29/2015
	Description:		This Proc Does
	Example Execute:
						SELECT dbo.udf_get_primary_key_column_name('dbo.Accounts')
*/
CREATE FUNCTION [util].[udfGetPrimaryKeyColumnName] ( @TableName sysname )
RETURNS NVARCHAR(100)
AS
	/*
		Author:				Jordan Gurney
		Date:				7/13/2015
		Description:		This Function Returns the Primary Key Column name for a table.
		Example Execute:
							SELECT dbo.udf_get_primary_key_column_name('dbo.Accounts') PkName
	*/
    BEGIN

        DECLARE @RetVal NVARCHAR(100)
        SELECT  @RetVal = i.COLUMN_NAME
        FROM    INFORMATION_SCHEMA.KEY_COLUMN_USAGE i
        WHERE   OBJECTPROPERTY(OBJECT_ID(i.CONSTRAINT_NAME), 'IsPrimaryKey') = 1
                AND i.TABLE_NAME = PARSENAME(@TableName, 1)
        RETURN @RetVal
    END


GO

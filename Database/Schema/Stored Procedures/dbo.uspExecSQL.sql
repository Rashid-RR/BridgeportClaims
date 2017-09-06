SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	9/5/2017
	Description:	Wrapper proc to Execute or print dynamic SQL
	Sample Execute:
					EXEC dbo.uspExecSQL 'DROP TABLE dbo.T', 1
*/
CREATE PROC [dbo].[uspExecSQL] @SQLStatement NVARCHAR(4000), @DebugOnly BIT = 0
AS BEGIN
	SET NOCOUNT ON;
	IF @DebugOnly = 1
		PRINT @SQLStatement
	ELSE
		EXEC sys.sp_executesql @SQLStatement
END
GO

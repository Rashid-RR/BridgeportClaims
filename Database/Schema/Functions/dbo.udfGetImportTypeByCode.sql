SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[udfGetImportTypeByCode](@Code VARCHAR(50))
RETURNS TINYINT
AS BEGIN
	RETURN  (
				SELECT  it.ImportTypeID
				FROM    etl.ImportType AS it
				WHERE	it.TypeCode = @Code
			);
END
GO

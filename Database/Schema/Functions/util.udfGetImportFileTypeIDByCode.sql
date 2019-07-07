SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [util].[udfGetImportFileTypeIDByCode](@Code VARCHAR(30))
RETURNS INT
AS BEGIN
	RETURN  (
				SELECT  X.ImportFileTypeID
				FROM    util.ImportFileType AS x
				WHERE	x.Code = @Code
			);
END
GO

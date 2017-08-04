SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/3/2017
	Description:	Select the DTO fields from the ImportFile table.
	Sample Execute:
					EXEC dbo.uspGetImportFile
*/
CREATE PROC [dbo].[uspGetImportFile]
AS BEGIN
	SET NOCOUNT ON;
	SELECT [if].[ImportFileID]
     , [if].[FileName]
     , [if].[FileExtension]
     , [dtme].[udfGetLocalDateTime]([if].[CreatedOnUTC]) CreatedOn
      FROM [util].[ImportFile] AS [if]
END
GO

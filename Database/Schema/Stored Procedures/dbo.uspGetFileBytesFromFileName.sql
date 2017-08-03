SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/1/2017
	Description:	Gets the byte array for C# of the File Bytes in the database.
	Sample Execute:
					EXEC dbo.uspGetFileBytesFromFileName '2017-07-20_10h54_23.jpg'
*/
CREATE PROC [dbo].[uspGetFileBytesFromFileName] @FileName NVARCHAR(255)
AS BEGIN
	SET NOCOUNT ON;
	SELECT [i].[FileBytes]
	FROM [util].[ImportFile] AS [i]
	WHERE [i].[FileName] = @FileName
END
GO

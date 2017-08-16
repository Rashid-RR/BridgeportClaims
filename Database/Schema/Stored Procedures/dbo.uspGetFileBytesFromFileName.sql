SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/1/2017
	Description:	Gets the byte array for C# of the File Bytes in the database.
	Sample Execute:
					EXEC dbo.uspGetFileBytesFromFileName '06-17Payments.xlsx'
*/
CREATE PROC [dbo].[uspGetFileBytesFromFileName] @FileName NVARCHAR(255)
AS BEGIN
	BEGIN TRY
	    SET NOCOUNT ON
		SELECT [i].[FileBytes]
		FROM [util].[ImportFile] AS [i]
		WHERE [i].[FileName] = @FileName
			AND [i].[Processed] = 0
	END TRY
	BEGIN CATCH
	    THROW;
	END CATCH
END
GO

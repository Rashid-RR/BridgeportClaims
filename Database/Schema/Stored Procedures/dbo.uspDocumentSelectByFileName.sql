SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	12/20/2017
	Description:	Proc to return a Document object by its File Name.
	Sample Execute:
					EXEC [dbo].[uspDocumentSelectByFileName] 'csp201711245306.pdf'
*/
CREATE PROC [dbo].[uspDocumentSelectByFileName]
    @FileName VARCHAR(1000),
	@FileTypeID TINYINT
AS 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	SELECT  [FileName]
		  , [Extension]
		  , [FileSize]
		  , [CreationTimeLocal]
		  , [LastAccessTimeLocal]
		  , [LastWriteTimeLocal]
		  , [DirectoryName]
		  , [FullFilePath]
		  , [FileUrl]
		  , [ByteCount]
		  , [FileTypeID]
	FROM    [dbo].[Document]
	WHERE   [FileName] = @FileName
			AND [FileTypeID] = @FileTypeID
GO

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
    @FullFileName NVARCHAR(4000),
	@FileTypeID TINYINT
AS BEGIN
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
	WHERE   [FullFilePath] = @FullFileName
			AND [FileTypeID] = @FileTypeID
END

GO
GRANT EXECUTE ON  [dbo].[uspDocumentSelectByFileName] TO [bridgeportClaimsWindowsServiceUser]
GO

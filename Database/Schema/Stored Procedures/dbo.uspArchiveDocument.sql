SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	01/16/2018
	Description:	Simple update to Archive Document
	Sample Execute:
					EXEC [dbo].[uspArchiveDocument] 1
*/
CREATE PROC [dbo].[uspArchiveDocument] @DocumentID INT
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    UPDATE  [dbo].[Document]
    SET     [Archived] = 1, [UpdatedOnUTC] = SYSUTCDATETIME()
    WHERE   [DocumentID] = @DocumentID
END
GO

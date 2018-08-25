SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[uspArchivedDuplicateClaimInsert]
    @ExcludeClaimID int,
    @ExcludedByUserID nvarchar(128)
AS 
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @UtcNow DATETIME2 = dtme.udfGetDate();
    INSERT INTO [dbo].[ArchivedDuplicateClaim] ([ExcludeClaimID], [ExcludedByUserID], [CreatedOnUTC], [UpdatedOnUTC])
    SELECT @ExcludeClaimID, @ExcludedByUserID, @UtcNow, @UtcNow;
GO

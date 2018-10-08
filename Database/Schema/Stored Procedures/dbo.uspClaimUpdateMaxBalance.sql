SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       10/7/2018
 Description:       Updates the MaxBalance column
 Example Execute:
                    EXECUTE [dbo].[uspClaimUpdateMaxBalance] 25, 1, N'0c1547ab-3c21-4647-ac93-9861bdcc0963'
 =============================================
*/
CREATE   PROC [dbo].[uspClaimUpdateMaxBalance]
    @ClaimID INT, @IsMaxBalance BIT, @ModifiedByUserID NVARCHAR(128)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        IF (@ClaimID IS NULL)
            BEGIN
                RAISERROR(N'Error. You may not pass in a null Claim ID.', 16, 1) WITH NOWAIT;
                RETURN -1;
            END
        IF (@IsMaxBalance IS NULL) 
			SET @IsMaxBalance = 0;
        DECLARE @UtcNow DATETIME2 = dtme.udfGetDate();
        UPDATE  dbo.Claim
        SET		ModifiedByUserID = @ModifiedByUserID, UpdatedOnUTC = @UtcNow, IsMaxBalance = @IsMaxBalance
        WHERE   ClaimID = @ClaimID
    END
GO

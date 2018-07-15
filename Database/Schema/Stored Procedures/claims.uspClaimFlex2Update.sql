SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/14/2018
 Description:       Updates a Claim with a Claim Flex 2 Value 
 Example Execute:
                    EXECUTE [claims].[uspClaimFlex2Update]
 =============================================
*/
CREATE PROC [claims].[uspClaimFlex2Update]
(
	@ClaimID INT,
	@ClaimFlex2ID INT,
	@ModifiedByUserID NVARCHAR(128),
	@Operation VARCHAR(10) OUTPUT
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
            
        SELECT  @Operation = CASE WHEN c.ClaimFlex2ID IS NULL THEN 'Add' ELSE 'Update' END
		FROM    dbo.Claim AS c
		WHERE   c.ClaimID = @ClaimID;

		UPDATE  dbo.Claim
		SET		ClaimFlex2ID = @ClaimFlex2ID, ModifiedByUserID = @ModifiedByUserID, UpdatedOnUTC = SYSUTCDATETIME()
		WHERE   ClaimID = @ClaimID;
            
        IF (@@TRANCOUNT > 0)
            COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
            ROLLBACK;
                
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

        RAISERROR(N'%s (line %d): %s',    -- Message text w formatting
            @ErrSeverity,        -- Severity
            @ErrState,            -- State
            @ErrProc,            -- First argument (string)
            @ErrLine,            -- Second argument (int)
            @ErrMsg);            -- First argument (string)
    END CATCH
END
GO

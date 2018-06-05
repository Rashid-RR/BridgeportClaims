SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       5/26/2018
 Description:       Dedupes and merges the Claim data from one claim into another.
 Example Execute:   DECLARE @I INT
                    EXECUTE @I = [dbo].[uspMergeDuplicateClaims] 2392, 508,'3006686098',366,'12/10/2018', 898, 45, 5,'01'
                    SELECT @I
 =============================================
*/
CREATE PROC [dbo].[uspMergeDuplicateClaims]
(
    @ClaimID INT,
    @DuplicateClaimID INT,
    @ClaimNumber VARCHAR(255) = '{NULL}',
    @PatientID INT = -1,
    @DateOfInjury DATE = '1/1/1901',
    @AdjustorID INT = -1,
    @PayorID INT = -1,
    @ClaimFlex2ID INT = -1,
    @PersonCode CHAR(2) = '{}'
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY;
        BEGIN TRAN;

            DECLARE @RowCount INT, @PrntMsg NVARCHAR(500);

            UPDATE  [c] SET [c].[ClaimNumber] = CASE WHEN @ClaimNumber != '{NULL}' THEN @ClaimNumber ELSE [c].[ClaimNumber] END,
                            [c].[PatientID] = CASE WHEN @PatientID != -1 THEN @PatientID ELSE [c].[PatientID] END,
                            [c].[DateOfInjury] = CASE WHEN @DateOfInjury != '1/1/1901' THEN @DateOfInjury ELSE [c].[DateOfInjury] END,
                            [c].[AdjustorID] = CASE WHEN @AdjustorID != -1 THEN @AdjustorID ELSE [c].[AdjustorID] END,
                            [c].[PayorID] = CASE WHEN @PayorID != -1 THEN @PayorID ELSE [c].[PayorID] END,
                            [c].[ClaimFlex2ID] = CASE WHEN @ClaimFlex2ID != -1 THEN @ClaimFlex2ID ELSE [c].[ClaimFlex2ID] END,
                            [c].[PersonCode] = CASE WHEN @PersonCode != '{}' THEN @PersonCode ELSE [c].[PersonCode] END
            FROM    [dbo].[Claim] AS [c]
            WHERE   [c].[ClaimID] = @ClaimID;

            SET @RowCount = @@ROWCOUNT;

            IF (@RowCount != 1)
                BEGIN
                    IF (@@TRANCOUNT > 0)
                        ROLLBACK;
                    SET @PrntMsg = N'Error, the updated row count was ' + CONVERT(NVARCHAR, @RowCount) + ', not 1.';
                    RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
                    RETURN -1;
                END

            EXEC dbo.[uspDeDupeTable] @TableName = 'dbo.Claim', @IDToRemove = @DuplicateClaimID
                                     ,@IDToKeep = @ClaimID, @DebugOnly = 0;

        IF (@@TRANCOUNT > 0)
            COMMIT;
    END TRY
    BEGIN CATCH;
        IF (@@TRANCOUNT > 0)
            ROLLBACK;
                
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

        RAISERROR(N'%s (line %d): %s',
            @ErrSeverity,
            @ErrState,
            @ErrProc,
            @ErrLine,
            @ErrMsg);
    END CATCH
END

GO

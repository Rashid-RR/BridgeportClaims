SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       8/23/2018
 Description:       Fixes Ajustors that make it into the Sytem from the Laker file import system that have Adjustor Name
 Example Execute:
                    EXECUTE [dbo].[uspRemoveBlankAdjustors]
 =============================================
*/
CREATE PROC [dbo].[uspRemoveBlankAdjustors]
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        
        -- Check to see if this routine is even needed.
        IF NOT EXISTS (SELECT * FROM dbo.Adjustor AS a WHERE RTRIM(LTRIM(ISNULL(a.AdjustorName, ''))) = '')
            BEGIN
                IF (@@ROWCOUNT > 0)
                    ROLLBACK TRANSACTION;
                RAISERROR(N'Error, all Adjustor Names have valid values.', 16, 1) WITH NOWAIT;
                RETURN -1;
            END;

            DECLARE @AdjustorFix TABLE (AdjustorID [INT] NOT NULL PRIMARY KEY);
            INSERT  @AdjustorFix (AdjustorID)
            SELECT  a.AdjustorID
            FROM    dbo.Adjustor AS a
            WHERE   RTRIM(LTRIM(ISNULL(a.AdjustorName, ''))) = '';

            DECLARE @AdjustorID INT;

            DECLARE AdjustorCrsr CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
            SELECT a.AdjustorID FROM @AdjustorFix AS a

            OPEN AdjustorCrsr;
            FETCH NEXT FROM AdjustorCrsr INTO @AdjustorID;

            WHILE (@@FETCH_STATUS = 0)
                BEGIN
                    UPDATE dbo.Claim SET AdjustorID = NULL WHERE AdjustorID = @AdjustorID;
                    FETCH NEXT FROM AdjustorCrsr INTO @AdjustorID;
                END

            CLOSE AdjustorCrsr;
            DEALLOCATE AdjustorCrsr;

        -- Cleanup
        DELETE a FROM dbo.Adjustor AS a WHERE RTRIM(LTRIM(ISNULL(a.AdjustorName, ''))) = '';

        IF (@@TRANCOUNT > 0)
            BEGIN
                COMMIT;
            END
    END TRY
    BEGIN CATCH
        IF (@@TRANCOUNT > 0)
            BEGIN
                ROLLBACK;
            END;

        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(4000) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();

        RAISERROR(N'%s (line %d): %s',
            @ErrSeverity,
            @ErrState,@ErrProc,@ErrLine,@ErrMsg);
    END CATCH
END
GO

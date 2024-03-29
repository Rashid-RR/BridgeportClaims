SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
 Author:            Jordan Gurney
 Create Date:       9/17/2017
 Description:       Dissolve return prescriptions into origin prescriptions.
 Sample Execute:
                    EXEC dbo.uspDissolveReturnPrescriptions
*/
CREATE PROC [dbo].[uspDissolveReversedPrescriptions]
AS
    BEGIN
        SET NOCOUNT ON;
        BEGIN TRY
            BEGIN TRAN;
            DECLARE @RecordCount INT
                   ,@PrntMsg NVARCHAR(500)
                   ,@Predicate CHAR(2) = '%R'
                   ,@UtcNow DATETIME2 = SYSUTCDATETIME()
                   ,@TotalRows INT
                   ,@rID INT
                   ,@One INT = 1
                   ,@pID INT;

            -- Testing
            /*
              IF OBJECT_ID('tempdb..#Returns') IS NOT NULL
               DROP TABLE #Returns
            */

            CREATE TABLE [#Returns]
            (
                [rID] INT NOT NULL
               ,[pID] INT NOT NULL
               ,[BilledAmount] MONEY NOT NULL
               ,[Quantity] FLOAT NOT NULL
               ,[DaySupply] FLOAT NOT NULL
               ,[Usual] DECIMAL(18, 0) NOT NULL
               ,[PayableAmount] MONEY NOT NULL
               ,[BillIngrCost] FLOAT NULL
               ,[BillDispFee] FLOAT NULL
               ,[PayIngrCost] FLOAT NULL
               ,[PayDispFee] FLOAT NULL
               ,[DateSubmitted] DATETIME2 NOT NULL
               ,PRIMARY KEY CLUSTERED ([rID],[pID])
            )
            INSERT  [#Returns] ([rID], [pID], [BilledAmount], [Quantity], [DaySupply], [Usual], [PayableAmount]
                               ,[BillIngrCost], [BillDispFee], [PayIngrCost], [PayDispFee], [DateSubmitted])
            SELECT  [p].[PrescriptionID]
                   ,[nr].[PrescriptionID]
                   ,[c].[BilledAmount]
                   ,[c].[Quantity]
                   ,[c].[DaySupply]
                   ,[c].[Usual]
                   ,[c].[PayableAmount]
                   ,[c].[BillIngrCost]
                   ,[c].[BillDispFee]
                   ,[c].[PayIngrCost]
                   ,[c].[PayDispFee]
                   ,[c].[DateSubmitted]
            FROM
                    (
                        SELECT  [iip].[BilledAmount]
                               ,[iip].[Quantity]
                               ,[iip].[DaySupply]
                               ,[iip].[Usual]
                               ,[iip].[PayableAmount]
                               ,[iip].[BillIngrCost]
                               ,[iip].[BillDispFee]
                               ,[iip].[PayIngrCost]
                               ,[iip].[PayDispFee]
                               ,[iip].[PrescriptionID]
                               ,[iip].[DateSubmitted]
                        FROM    [dbo].[Prescription] AS [iip]
                                INNER JOIN [etl].[StagedLakerFile] AS [s] ON [s].[PrescriptionID] = [iip].[PrescriptionID]
                        WHERE   [s].[RowID] LIKE @Predicate
                    ) AS [c]
                    INNER JOIN [dbo].[Prescription] AS [p] ON [p].[PrescriptionID] = [c].[PrescriptionID]
                    CROSS APPLY
                    (
                        SELECT      TOP (@One)
                                    [nonr].[PrescriptionID]
                        FROM        [dbo].[Prescription] AS [nonr]
                        WHERE       [nonr].[DateFilled] = [p].[DateFilled]
                                    AND [nonr].[NDC] = [p].[NDC]
                                    AND [nonr].[RxNumber] = [p].[RxNumber]
                                    AND [p].[PharmacyNABP] = [nonr].[PharmacyNABP]
                                    AND [nonr].[LabelName] = [p].[LabelName]
                                    AND [nonr].[DateFilled] = [p].[DateFilled]
                                    AND (([p].[BilledAmount] + [nonr].[BilledAmount]) = 0)
                                    AND (([p].[PayIngrCost] + [nonr].[PayIngrCost]) = 0)
                                    AND ISNULL([p].[PrescriberNPI], '') = ISNULL([nonr].[PrescriberNPI], '')
                        ORDER BY    [nonr].[DateSubmitted] ASC
                    ) AS [nr];
            SET @TotalRows = @@ROWCOUNT;
            SET @PrntMsg = N'Updating ' + CONVERT(NVARCHAR, @TotalRows) + N' rows.';
            RAISERROR(@PrntMsg, 1, 1) WITH NOWAIT;

            -- Ok, first, verify that we've scooped up every single return prescription
            SELECT  @RecordCount = COUNT(*)
            FROM
                    (
                        SELECT  [p].[PrescriptionID]
                        FROM    [etl].[StagedLakerFile] AS [s]
                                INNER JOIN [dbo].[Prescription] AS [p] ON [p].[PrescriptionID] = [s].[PrescriptionID]
                        WHERE   [s].[RowID] LIKE @Predicate
                    ) AS [c]
                    LEFT JOIN [#Returns] AS [r] ON [c].[PrescriptionID] = [r].[rID]
            WHERE   [r].[rID] IS NULL;

            IF (@RecordCount > 0)
                BEGIN
                    IF (@@TRANCOUNT > 0)
                        ROLLBACK;
                    SET @PrntMsg = N'Error. Not all Return Prescriptions were caught. There are '
                                   + CONVERT(NVARCHAR, @RecordCount) + N' records that were not caught.';
                    RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
                    RETURN -1;
                END

            DECLARE @Reversed TABLE
            (
                [PrescriptionID] INT NOT NULL PRIMARY KEY CLUSTERED
               ,[iBilledAmount] MONEY NOT NULL
               ,[iPayableAmount] MONEY NOT NULL
               ,[dBilledAmount] MONEY NOT NULL
               ,[dPayableAmount] MONEY NOT NULL
            )

            -- We have now sufficiently passed the QA checks. Let's get down to business.
            UPDATE  [rp]
            SET     [rp].[BilledAmount] = [rp].[BilledAmount] + [r].[BilledAmount]
                   ,[rp].[Quantity] = [rp].[Quantity] + [r].[Quantity]
                   ,[rp].[DaySupply] = [rp].[DaySupply] + [r].[DaySupply]
                   ,[rp].[Usual] = [rp].[Usual] + [r].[Usual]
                   ,[rp].[PayableAmount] = [rp].[PayableAmount] + [r].[PayableAmount]
                   ,[rp].[BillIngrCost] = [rp].[BillIngrCost] + [r].[BillIngrCost]
                   ,[rp].[BillDispFee] = [rp].[BillDispFee] + [r].[BillDispFee]
                   ,[rp].[PayIngrCost] = [rp].[PayIngrCost] + [r].[PayIngrCost]
                   ,[rp].[PayDispFee] = [rp].[PayDispFee] + [r].[PayDispFee]
                   ,[rp].[ReversedDate] = [r].[DateSubmitted]
                   ,[rp].[UpdatedOnUTC] = @UtcNow
            OUTPUT [Inserted].[PrescriptionID]
                  ,[Inserted].[BilledAmount]
                  ,[Inserted].[PayableAmount]
                  ,[Deleted].[BilledAmount]
                  ,[Deleted].[PayableAmount]
            INTO @Reversed
            (
                [PrescriptionID]
               ,[iBilledAmount]
               ,[iPayableAmount]
               ,[dBilledAmount]
               ,[dPayableAmount]
            )
            FROM    [dbo].[Prescription] AS [rp]
                    INNER JOIN [#Returns] AS [r] ON [r].[pID] = [rp].[PrescriptionID];
            SET @RecordCount = @@ROWCOUNT;

            IF (@RecordCount > @TotalRows)
                BEGIN
                    IF (@@TRANCOUNT > 0)
                        ROLLBACK;
                    SET @PrntMsg = N'Error. The updated row count of ' + CONVERT(NVARCHAR, @RecordCount)
                                   + N' is greater than the total row count of ' + CONVERT(NVARCHAR, @TotalRows) + N'.';
                    RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
                    RETURN -1;
                END

            -- Roll through every record, dissolving the return script into the original
            DECLARE [DissolvingCursr] CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
            SELECT  [r].[rID]
                   ,[r].[pID]
            FROM    [#Returns] AS [r];
            OPEN [DissolvingCursr];

            FETCH NEXT FROM [DissolvingCursr]
            INTO @rID
                ,@pID;

            WHILE @@FETCH_STATUS = 0
                BEGIN
                    EXEC [dbo].[uspDeDupeTable] @TableName = 'dbo.Prescription'
                                               ,@IDToRemove = @rID
                                               ,@IDToKeep = @pID;
                    FETCH NEXT FROM [DissolvingCursr]
                    INTO @rID
                        ,@pID;
                END

            CLOSE [DissolvingCursr];
            DEALLOCATE [DissolvingCursr];

            SET @UtcNow = SYSUTCDATETIME();

            -- Update the Original Amounts
            UPDATE  [p]
            SET     [p].[BilledAmountOriginal] = CASE WHEN [r].[iBilledAmount] != [r].[dBilledAmount] THEN
                                                          [r].[dBilledAmount]
                                                      ELSE
                                                          [p].[BilledAmountOriginal]
                                                 END
                   ,[p].[PayableAmountOriginal] = CASE WHEN [r].[iPayableAmount] != [r].[dPayableAmount] THEN
                                                           [r].[dPayableAmount]
                                                       ELSE
                                                           [p].[PayableAmountOriginal]
                                                  END
                   ,[p].[UpdatedOnUTC] = @UtcNow
            FROM    [dbo].[Prescription] AS [p]
                    INNER JOIN @Reversed AS [r] ON [r].[PrescriptionID] = [p].[PrescriptionID]
            WHERE   [r].[iPayableAmount] != [r].[dPayableAmount]
                    OR  [r].[iBilledAmount] != [r].[dBilledAmount];

            IF (@@TRANCOUNT > 0)
                BEGIN
                    COMMIT;
                END
            IF (@@TRANCOUNT > 0)
                RAISERROR(N'Transaction count is greater than zero when exiting routine.', 16, 1) WITH NOWAIT;
        END TRY
        BEGIN CATCH
            IF (@@TRANCOUNT > 0)
                ROLLBACK;

            DECLARE @ErrSeverity INT = ERROR_SEVERITY()
                   ,@ErrState INT = ERROR_STATE()
                   ,@ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
                   ,@ErrLine INT = ERROR_LINE()
                   ,@ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

            RAISERROR(   N'%s (line %d): %s' -- Message text w formatting
                        ,@ErrSeverity -- Severity
                        ,@ErrState -- State
                        ,@ErrProc -- First argument (string)
                        ,@ErrLine -- Second argument (int)
                        ,@ErrMsg
                     ); -- First argument (string)
        END CATCH
    END
GO

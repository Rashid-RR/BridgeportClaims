SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/10/2019
 Description:       Dismisses the new Envision notification to update the BilledAmount
					and possibly the Payor ID (if necessary).
 Example Execute:
                    EXECUTE [dbo].[uspDismissEnvisionNotification]
 =============================================
*/
CREATE PROCEDURE [dbo].[uspDismissEnvisionNotification]
(
	@PrescriptionID INT,
	@BilledAmount MONEY,
	@ModifiedByUserID NVARCHAR(128),
	@PayorID INT = NULL
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	DECLARE @UtcNow DATETIME2 = dtme.udfGetDate();
	DECLARE @ImportTypeID INT = dbo.udfGetImportTypeByCode('ENVISION');
	DECLARE @NotificationID INT;
	DECLARE @TodayLocal DATE = CONVERT(DATE, [dtme].[udfGetLocalDate]());
	IF @ImportTypeID IS NULL
		BEGIN
			RAISERROR(N'Error, could not populate the Import Type ID variable.', 16, 1) WITH NOWAIT;
			RETURN -1;
		END

	BEGIN TRY
        BEGIN TRAN;
		UPDATE p SET p.BilledAmount = @BilledAmount, p.ModifiedByUserID = @ModifiedByUserID, p.UpdatedOnUTC = @UtcNow
		FROM dbo.Prescription AS p
		WHERE p.ImportTypeID = @ImportTypeID
			  AND p.PrescriptionID = @PrescriptionID;

		IF (@PayorID IS NOT NULL AND @PayorID > 0)
			BEGIN
				UPDATE c SET c.PayorID = @PayorID, c.ModifiedByUserID = @ModifiedByUserID, c.UpdatedOnUTC = @UtcNow
				FROM dbo.Prescription AS p INNER JOIN dbo.Claim AS c ON c.ClaimID = p.ClaimID
				WHERE p.ImportTypeID = @ImportTypeID
					  AND p.PrescriptionID = @PrescriptionID;
			END

		-- Dismiss Notification
		SELECT @NotificationID = [n].[NotificationID]
		FROM [dbo].[Prescription] AS [p]
			 INNER JOIN [dbo].[Notification] AS [n] ON [n].[PrescriptionID] = [p].[PrescriptionID]
		WHERE [p].[PrescriptionID] = @PrescriptionID;

		UPDATE [n]
		SET [n].[IsDismissed] = 1
		   ,[n].[DismissedByUserID] = @ModifiedByUserID
		   ,[n].[DismissedDate] = @TodayLocal
		FROM [dbo].[Notification] AS [n]
		WHERE [n].[NotificationID] = @NotificationID;

		IF (@@TRANCOUNT > 0)
			COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
			ROLLBACK;	
		DECLARE @ErrLine INT = ERROR_LINE()
              , @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
		DECLARE @Msg NVARCHAR(2000) = FORMATMESSAGE(N'An error occurred: %s Line Number: %u', @ErrMsg, @ErrLine);
		THROW 50000, @Msg, 0;
    END CATCH
END
GO

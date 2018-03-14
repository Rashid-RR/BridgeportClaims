SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		3/14/2018
 Description:		Runs twice, once at the beginning of the data import, and once at the end,
					picking up events that occurred in between.
 Example Execute:
					EXECUTE [dbo].[uspGenerateNotifications] 0
 =============================================
*/
CREATE PROC [dbo].[uspGenerateNotifications] @IsBeforeDataImport BIT
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
		DECLARE @UtcNow DATETIME2 = SYSDATETIME();
		DECLARE @RealNow DATE = CONVERT(DATE, [dtme].[udfGetLocalDate]())
		
		-- Payor Letter Name Notifications.	
		DECLARE @NotificationTypeID INT, @MaxPayorID INT;
		SET @NotificationTypeID = dbo.udfGetNotificationTypeIDFromCode('PAYORLETTER');

		IF (@IsBeforeDataImport = 1) -- Stage any data needed to generating notifications after the data import.
			BEGIN
				SELECT @MaxPayorID = MAX([p].[PayorID]) FROM [dbo].[Payor] AS [p];

				UPDATE  [dbo].[NotificationConfig]
				SET     [NotificationValue] = @MaxPayorID,	
						[EffectiveDate] = @RealNow,
						[UpdatedOnUTC] = @UtcNow
				WHERE   [NotificationTypeID] = @NotificationTypeID
			END
		ELSE -- else we're running after the data import.
			BEGIN
				DECLARE @MessageText VARCHAR(4000);

				SELECT  @MaxPayorID = CONVERT(INT, [nc].[NotificationValue])
				FROM    [dbo].[NotificationConfig] AS [nc]
				WHERE   [nc].[NotificationTypeID] = @NotificationTypeID

				INSERT INTO [dbo].[Notification]
				(   [MessageText]
				  , [GeneratedDate]
				  , [NotificationTypeID]
				  , [CreatedOnUTC]
				  , [UpdatedOnUTC])
				SELECT  'A new Payor (ID ' + CONVERT(VARCHAR, [p].[PayorID]) + ') has been imported into the system as of ' + FORMAT(@RealNow, 'M/d/yyyy') +
						'. The "BillToName" is "' + [p].[BillToName] + '". This will be used for the "LetterName" unless you''d like to edit it here.'
					  , @RealNow
					  , @NotificationTypeID
					  , @UtcNow
					  , @UtcNow
				FROM    [dbo].[Payor] AS [p]
				WHERE   [p].[PayorID] > @MaxPayorID
			END
			
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

		RAISERROR(N'%s (line %d): %s',	-- Message text w formatting
			@ErrSeverity,		-- Severity
			@ErrState,			-- State
			@ErrProc,			-- First argument (string)
			@ErrLine,			-- Second argument (int)
			@ErrMsg);			-- First argument (string)
    END CATCH
END
GO

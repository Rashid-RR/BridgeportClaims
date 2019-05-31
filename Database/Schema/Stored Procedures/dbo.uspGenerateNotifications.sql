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
		DECLARE @NotificationTypeID INT, @MaxPayorID INT, @MaxReferralID INT;
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

				SELECT  @MaxPayorID = CONVERT(INT, [nc].[NotificationValue])
				FROM    [dbo].[NotificationConfig] AS [nc]
				WHERE   [nc].[NotificationTypeID] = @NotificationTypeID

				INSERT INTO [dbo].[Notification]
				(   [MessageText]
				  , [GeneratedDate]
				  , [NotificationTypeID]
				  , [CreatedOnUTC]
				  , [UpdatedOnUTC])
				SELECT  'A new Payor (ID ' + CONVERT(VARCHAR(100), [p].[PayorID]) + ') has been imported into the system as of ' + FORMAT(@RealNow, 'M/d/yyyy') +
						'. The "BillToName" is "' + [p].[BillToName] + '". This will be used for the "LetterName" unless you''d like to edit it here.'
					  , @RealNow
					  , @NotificationTypeID
					  , @UtcNow
					  , @UtcNow
				FROM    [dbo].[Payor] AS [p]
				WHERE   [p].[PayorID] > @MaxPayorID

				-- Notifying of any new client referrals.
				SET @NotificationTypeID = dbo.udfGetNotificationTypeIDFromCode('NEWREFERRAL');
				SELECT  @MaxReferralID = CONVERT(INT, [nc].[NotificationValue])
				FROM    [dbo].[NotificationConfig] AS [nc]
				WHERE   [nc].[NotificationTypeID] = @NotificationTypeID

				INSERT INTO [dbo].[Notification]
				(   [MessageText]
				  , [GeneratedDate]
				  , [NotificationTypeID]
				  , [CreatedOnUTC]
				  , [UpdatedOnUTC])
				SELECT 'A new client referral (ID ' + CONVERT(VARCHAR(100), r.ReferralID) + ') has been entered into the system as of ' + FORMAT(@RealNow, 'M/d/yyyy') +
				'. Patient name: ' + ISNULL(r.FirstName, '') + ' ' + ISNULL(r.LastName, '')
				, @RealNow
				, @NotificationTypeID
				, @UtcNow
				, @UtcNow
				FROM client.Referral AS r
				WHERE r.ReferralID > @MaxReferralID;
				IF (@@ROWCOUNT > 0)
					BEGIN
						SELECT @MaxReferralID = MAX(r.ReferralID) FROM client.Referral AS r;

						UPDATE  [dbo].[NotificationConfig]
						SET     [NotificationValue] = @MaxReferralID,	
								[EffectiveDate] = @RealNow,
								[UpdatedOnUTC] = @UtcNow
						WHERE   [NotificationTypeID] = @NotificationTypeID;
					END
			END
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

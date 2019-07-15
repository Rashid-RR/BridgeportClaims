SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/11/2019
 Description:       Gets all Notifications not dismissed.
 Example Execute:
                    EXECUTE [dbo].[uspGetNotifications]
 =============================================
*/
CREATE PROCEDURE [dbo].[uspGetNotifications]
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        SELECT [n].[NotificationID] AS [NotificationId]
              ,[n].[MessageText]
              ,[n].[GeneratedDate]
              ,[nt].[TypeName] AS [NotificationType]
              ,[n].[PrescriptionID] AS [PrescriptionId]
              ,CASE WHEN [pay].[PayorID] IS NOT NULL AND [pay].[PayorID] = -1 THEN 1 ELSE 0 END AS [NeedsCarrier]
              ,[c].[ClaimID] AS [ClaimId]
        FROM [dbo].[Notification] AS [n]
             INNER JOIN [dbo].[NotificationType] AS [nt] ON [nt].[NotificationTypeID] = [n].[NotificationTypeID]
             LEFT JOIN [dbo].[Prescription] AS [p]
                       INNER JOIN [dbo].[Claim] AS [c] ON [c].[ClaimID] = [p].[ClaimID]
                       INNER JOIN [dbo].[Payor] AS [pay] ON [pay].[PayorID] = [c].[PayorID] ON [p].[PrescriptionID] = [n].[PrescriptionID]
        WHERE [n].[IsDismissed] = 0
        ORDER BY [n].[UpdatedOnUTC] DESC;
    END;
GO

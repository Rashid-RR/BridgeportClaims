SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[udfGetNotificationTypeIDFromCode](@Code VARCHAR(50))
RETURNS INTEGER
AS BEGIN
	RETURN
		(
			SELECT  [nt].[NotificationTypeID]
			FROM    [dbo].[NotificationType] AS [nt]
			WHERE   [nt].[Code] = @Code
		)
END
GO

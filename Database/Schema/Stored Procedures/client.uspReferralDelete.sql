SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [client].[uspReferralDelete] 
    @ReferralID int
AS 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DELETE
	FROM   [client].[Referral]
	WHERE  [ReferralID] = @ReferralID;
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [util].[udfGetClaimNumberByClaimID](@ClaimID INT)
RETURNS VARCHAR(255)
AS BEGIN
	RETURN
		(
			SELECT  [c].[ClaimNumber]
			FROM    [dbo].[Claim] AS [c]
			WHERE   [c].[ClaimID] = @ClaimID
		)
	END
GO

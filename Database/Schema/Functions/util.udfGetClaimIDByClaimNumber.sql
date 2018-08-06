SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [util].[udfGetClaimIDByClaimNumber](@ClaimNumber VARCHAR(255))
RETURNS VARCHAR(255)
AS BEGIN
	RETURN
		(
			SELECT  [c].[ClaimID]
			FROM    [dbo].[Claim] AS [c]
			WHERE   [c].[ClaimID] = @ClaimNumber
		)
	END
GO

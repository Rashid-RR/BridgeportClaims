SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/30/2017
	Description:	Returns the Check #, Check Amount, and Amount Remaining
	Sample Execute:
					DECLARE @Base [dbo].[udtClaimID] INSERT @Base (ClaimID) 
					VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9)
					EXEC dbo.uspGetAmountRemaining @Base, '99999'
*/
CREATE PROC [dbo].[uspGetAmountRemaining]
(
	@ClaimIDs [dbo].[udtClaimID] READONLY,
	@CheckNumber VARCHAR(50),
	@AmountRemaining MONEY OUTPUT
)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @SplitList VARCHAR (8000)
	SELECT	@SplitList = COALESCE(@SplitList + ',' , '') + CAST(p.ClaimID AS VARCHAR)
	FROM	@ClaimIDs AS p

	CREATE TABLE #PossibleClaimIDs (ClaimID INT) -- Needs to match at least one of these.
	INSERT	#PossibleClaimIDs ( ClaimID )
	SELECT	Split.[value] 
	FROM	STRING_SPLIT(@SplitList, ',') AS Split

	SELECT	@AmountRemaining = SUM(p.AmountPaid)
	FROM	dbo.vwPayment AS p
	WHERE	p.CheckNumber = @CheckNumber
			AND p.ClaimID IN (SELECT c.ClaimID FROM #PossibleClaimIDs AS c)
END

GO

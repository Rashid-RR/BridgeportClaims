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
	@CheckNumber VARCHAR(50)
)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @SplitList VARCHAR (8000)
	SELECT	@SplitList = COALESCE(@SplitList + ',' , '') + CAST(p.ClaimID AS VARCHAR)
	FROM	@ClaimIDs AS p

	SELECT	@CheckNumber CheckNumber, @CheckNumber CheckNumber, SUM(p.AmountPaid) AmountRemaining
	FROM	dbo.vwPayment AS p
			INNER JOIN STRING_SPLIT(@SplitList, ',') AS Split
				ON Split.[value] = p.PaymentID
	WHERE	p.PaymentID IS NOT NULL
			AND	p.CheckNumber = @CheckNumber
END
GO

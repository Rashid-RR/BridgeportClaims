SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/30/2017
	Description:	Returns the Check #, Check Amount, and Amount Remaining
	Sample Execute:
					DECLARE @AmountRemaining MONEY
					DECLARE @Base [dbo].[udtID] INSERT @Base (ID)
					SELECT DISTINCT TOP 50 VP.ClaimID FROM dbo.vwPayment AS vp WHERE vp.CheckNumber = '190620377'
					EXEC dbo.uspGetAmountRemaining @Base, '190620377', @AmountRemaining OUTPUT
					SELECT @AmountRemaining
*/
CREATE PROC [dbo].[uspGetAmountRemaining]
(
    @ClaimIDs [dbo].[udtID] READONLY,
    @CheckNumber VARCHAR(50),
    @AmountRemaining MONEY OUTPUT
)
AS BEGIN
    SET NOCOUNT ON;
    SELECT  @AmountRemaining = SUM(p.AmountPaid)
    FROM    dbo.vwPayment AS p
    WHERE   p.CheckNumber = @CheckNumber
            AND EXISTS (SELECT c.ID FROM @ClaimIDs AS c WHERE c.ID = p.ClaimID);
END

GO

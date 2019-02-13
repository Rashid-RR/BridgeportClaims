SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       2/13/2019
 Description:       Removes the header the keeps track of the tree path.
 Example Execute:
                    EXECUTE [dbo].[uspDecisionTreeHeaderDelete]
 =============================================
*/
CREATE PROC [dbo].[uspDecisionTreeHeaderDelete]
(
	@SessionID UNIQUEIDENTIFIER,
	@ClaimID INTEGER
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DELETE [x]
	FROM    [dbo].[DecisionTreeUserPathHeader] AS [x]
	WHERE   [x].[SessionID] = @SessionID
			AND [x].[ClaimID] = @ClaimID;
END
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[uspDecisionTreeUserPathDelete]
    @TreeID INT
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DELETE
	FROM   [dbo].[DecisionTreeUserPath]
	WHERE  [DecisionTreeUserPathID] = @TreeID;
END
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       11/10/2018
 Description:       Gets the CollectionAssignment data for a specific User ID.
 Example Execute:
					DECLARE @ID NVARCHAR(128) = util.udfGetRandomAspNetUserID()
                    EXECUTE [dbo].[uspGetCollectionAssignmentData] @ID
 =============================================
*/
CREATE PROC [dbo].[uspGetCollectionAssignmentData] @UserID NVARCHAR(128)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	DECLARE @Left TABLE (PayorId INT NOT NULL PRIMARY KEY, Carrier VARCHAR(255) NOT NULL);
	INSERT  @Left ([PayorId], [Carrier])
	SELECT  [PayorId] = [ca].[PayorID], [Carrier] = [p].[GroupName]
	FROM    [dbo].[CollectionAssignment] AS [ca]
			INNER JOIN [dbo].[Payor] AS [p] ON [ca].[PayorID] = [p].[PayorID]
	WHERE   [ca].[UserID] = @UserID;

	SELECT [l].[PayorId], [l].[Carrier] FROM @Left AS [l]

	SELECT  [p].[PayorID], [p].[GroupName]
	FROM    [dbo].[Payor] AS [p]
			LEFT JOIN @Left AS [l] ON [p].[PayorID] = [l].[PayorId]
	WHERE   [l].[PayorId] IS NULL;
END
GO

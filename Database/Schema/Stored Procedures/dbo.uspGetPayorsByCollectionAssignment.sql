SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       11/15/2018
 Description:       Gets all the Payors filtered by the user's collection assignment.
 Example Execute:
					DECLARE @UserID NVARCHAR(128);
					SELECT @UserID = [util].[udfGetRandomAspNetUserID]();
                    EXECUTE [dbo].[uspGetPayorsByCollectionAssignment] @UserID;
 =============================================
*/
CREATE PROC [dbo].[uspGetPayorsByCollectionAssignment] @UserID NVARCHAR(128)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	SELECT  [PayorId] = [p].[PayorID], [Carrier] = [p].[GroupName]
	FROM    [dbo].[Payor] AS [p]
			INNER JOIN [dbo].[CollectionAssignment] AS [ca] ON [p].[PayorID] = [ca].[PayorID]
	WHERE   [ca].[UserID] = @UserID;
END
GO

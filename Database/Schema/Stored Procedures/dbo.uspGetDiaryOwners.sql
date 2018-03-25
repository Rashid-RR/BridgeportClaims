SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	03/24/2018
	Description:	Proc that returns the Owners available for the given diaries
	Sample Execute:
					EXEC [dbo].[uspGetDiaryOwners]
*/
CREATE   PROC [dbo].[uspGetDiaryOwners]
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SELECT          DISTINCT
                    UserId  = u.[ID]
                  , [Owner] = u.FirstName + ' ' + u.LastName
    FROM            dbo.Diary       AS d
        INNER JOIN  dbo.AspNetUsers AS u ON u.ID = d.AssignedToUserID
	WHERE			u.[FirstName] != 'Jordan' OR u.[LastName] != 'Gurney'
END
GO

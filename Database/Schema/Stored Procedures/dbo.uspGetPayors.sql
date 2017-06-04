SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	5/26/2017
	Description:	Returns a pagination of Payors
	Sample Execute:
					EXEC dbo.uspGetPayors 1, 10
*/
CREATE PROC [dbo].[uspGetPayors] @PageNumber INT = 1, @PageSize INT = 10
AS BEGIN
    SET NOCOUNT ON
    SELECT PayorId = p.PayorID
         , p.BillToName
         , p.BillToAddress1
         , p.BillToAddress2
         , p.BillToCity
         , [State] = us.StateName
         , p.BillToPostalCode
         , p.PhoneNumber
         , p.AlternatePhoneNumber
         , p.FaxNumber
         , p.Notes
         , p.Contact
         , p.CreatedOn
         , p.UpdatedOn
    FROM dbo.Payor p LEFT JOIN dbo.UsState us ON us.StateID = p.BillToStateID
    ORDER BY p.PayorID
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

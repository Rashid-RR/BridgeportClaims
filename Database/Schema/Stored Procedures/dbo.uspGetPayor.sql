SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	5/26/2017
	Description:	Returns a pagination of Payors
	Sample Execute:
					EXEC dbo.uspGetPayor 34
*/
CREATE PROC [dbo].[uspGetPayor] @PayorID INT
AS BEGIN
    SET NOCOUNT ON;
	SET XACT_ABORT ON;
    SELECT PayorId = p.PayorID
		 , p.[GroupName]
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
		 , p.[LetterName]
         , CreatedOn = p.CreatedOnUTC
         , UpdatedOn = p.UpdatedOnUTC
    FROM dbo.Payor p LEFT JOIN dbo.UsState us ON us.StateID = p.BillToStateID
	WHERE p.[PayorID] = @PayorID;
END
GO

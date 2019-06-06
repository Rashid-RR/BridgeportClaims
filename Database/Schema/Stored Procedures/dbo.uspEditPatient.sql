SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/5/2019
	Description:	Edits from the Patient Address Edit report.
	Sample Execute:
					EXEC dbo.uspEditPatient
*/
CREATE PROCEDURE [dbo].[uspEditPatient] (
    @PatientID int
   ,@ModifiedByUserID nvarchar(128)
   ,@LastName varchar(155) = '{NULL}'
   ,@FirstName varchar(155) = '{NULL}'
   ,@Address1 varchar(255) = '{NULL}'
   ,@Address2 varchar(255) = '{NULL}'
   ,@City varchar(155) = '{NULL}'
   ,@PostalCode varchar(100) = '{NULL}'
   ,@StateName varchar(64) = '{NULL}'
   ,@PhoneNumber varchar(30) = '{NULL}'
   ,@EmailAddress varchar(155) = '{NULL}'
)
AS
    BEGIN
        DECLARE @UtcNow datetime2 = dtme.udfGetDate();
		DECLARE @StateID int;
		IF (@StateName = '{NULL}')
			SET @StateID = -1
		ELSE
			SELECT @StateID = US.StateID FROM dbo.UsState AS US WHERE US.StateName = @StateName;

        UPDATE P
        SET    P.ModifiedByUserID = @ModifiedByUserID
              ,P.UpdatedOnUTC = @UtcNow
              ,P.LastName = CASE WHEN @LastName = '{NULL}' THEN P.LastName ELSE @LastName END
			  ,P.FirstName = CASE WHEN @FirstName = '{NULL}' THEN P.FirstName ELSE @FirstName END
			  ,P.Address1 = CASE WHEN @Address1 = '{NULL}' THEN P.Address1 ELSE @Address1 END
			  ,P.Address2 = CASE WHEN @Address2 = '{NULL}' THEN P.Address2 ELSE @Address2 END
			  ,P.City = CASE WHEN @City = '{NULL}' THEN P.City ELSE @City END
			  ,P.PostalCode = CASE WHEN @PostalCode = '{NULL}' THEN P.PostalCode ELSE @PostalCode END
			  ,P.StateID = CASE WHEN @StateID = -1 THEN P.StateID ELSE @StateID END
			  ,P.PhoneNumber = CASE WHEN @PhoneNumber = '{NULL}' THEN P.PhoneNumber ELSE @PhoneNumber END
			  ,P.EmailAddress = CASE WHEN @EmailAddress = '{NULL}' THEN P.EmailAddress ELSE @EmailAddress END
        FROM   dbo.Patient AS P
        WHERE  P.PatientID = @PatientID;
    END;
GO

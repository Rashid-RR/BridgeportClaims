SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/5/2019
	Description:	Report proc to populate the Patient Address Edit report.
	Sample Execute:
					EXEC rpt.uspPatientAddressEdit
*/
CREATE PROCEDURE [rpt].[uspPatientAddressEdit]
AS BEGIN
	SELECT	 p.PatientID AS PatientId
			,p.[LastName]
			,p.[FirstName]
			,p.[Address1]
			,p.[Address2]
			,p.[City]
			,p.[PostalCode]
			,p.[StateID] AS StateId
			,st.StateName
			,p.[PhoneNumber]
			,p.[EmailAddress]
	FROM [dbo].[Patient] AS p LEFT JOIN dbo.UsState AS st ON st.StateID = p.StateID;
END
GO

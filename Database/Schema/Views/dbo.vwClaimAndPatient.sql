SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

/*
	Author:			Jordan Gurney
	Create Date:	7/18/2017
	Description:	Provides an abraction for getting Claim and Patient Info for the UI
	Sample Execute:
					
*/
CREATE VIEW [dbo].[vwClaimAndPatient]
AS
SELECT [Name] = [p].[FirstName] + ' ' + [p].[LastName]
     , Address1 = [p].[Address1]
     , Address2 = [p].[Address2]
     , Adjustor = [a].[AdjustorName]
     , AdjustorFaxNumber = [a].[FaxNumber]
     , AdjustorPhoneNumber = [a].[PhoneNumber]
     , Carrier = [pa].[BillToName]
     , City = [p].[City]
     , StateAbbreviation = [uss].[StateCode]
     , PostalCode = [p].[PostalCode]
     , Flex2 = 'PIP' -- TODO: remove hard-coded
     , Gender = [g].[GenderName]
     , DateOfBirth = [p].[DateOfBirth]
     , EligibilityTermDate = [c].[TermDate]
     , PatientPhoneNumber = [p].[PhoneNumber]
     , DateEntered = [c].[DateOfInjury]
     , ClaimNumber = [c].[ClaimNumber]
FROM   [dbo].[Claim] AS [c]
       INNER JOIN [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
       LEFT JOIN [dbo].[Gender] AS [g] ON [g].[GenderID] = [p].[GenderID]
       LEFT JOIN [dbo].[Adjustor] AS [a] ON [a].[AdjustorID] = [c].[AdjustorID]
       LEFT JOIN [dbo].[Payor] AS [pa] ON [pa].[PayorID] = [c].[PayorID]
       LEFT JOIN [dbo].[UsState] AS uss ON [p].[StateID] = [uss].[StateID]
GO

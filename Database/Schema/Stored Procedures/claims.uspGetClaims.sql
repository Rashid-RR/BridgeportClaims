SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/13/2018
 Description:       Gets all Claims data for the Claims page.
 Example Execute:
                    EXEC [claims].[uspGetClaims] 775
 =============================================
*/
CREATE PROC [claims].[uspGetClaims] (@ClaimID INT)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        SELECT  ClaimId = c.ClaimID
               ,Name = p.FirstName + ' ' + p.LastName
			   ,c.IsMaxBalance
			   ,c.IsAttorneyManaged
               ,ClaimNumber = c.ClaimNumber
               ,DateOfBirth = p.DateOfBirth
               ,DateOfInjury = c.DateOfInjury
               ,Gender = g.GenderName
               ,Carrier = pay.GroupName
               ,Adjustor = a.AdjustorName
			   ,Attorney = att.[AttorneyName]
               ,EligibilityTermDate = c.TermDate
               ,Flex2 = cl.Flex2
               ,Address1 = p.Address1
               ,Address2 = p.Address2
               ,City = p.City
               ,StateAbbreviation = s.StateCode
               ,PostalCode = p.PostalCode
               ,PatientPhoneNumber = p.PhoneNumber
               ,AdjustorId = c.AdjustorID
			   ,AttorneyId = att.[AttorneyID]
               ,PayorId = c.PayorID
               ,StateId = p.StateID
               ,PatientGenderId = p.GenderID
               ,ClaimFlex2Id = c.ClaimFlex2ID
        FROM    dbo.Patient AS p
                INNER JOIN dbo.Claim AS c ON c.PatientID = p.PatientID
                LEFT JOIN dbo.UsState AS s ON p.StateID = s.StateID
                LEFT JOIN dbo.Gender AS g ON p.GenderID = g.GenderID
                LEFT JOIN dbo.Adjustor AS a ON c.AdjustorID = a.AdjustorID
				LEFT JOIN [dbo].[Attorney] AS [att] ON [c].[AttorneyID] = [att].[AttorneyID]
                LEFT JOIN dbo.Payor AS pay ON c.PayorID = pay.PayorID
                LEFT JOIN dbo.ClaimFlex2 AS cl ON c.ClaimFlex2ID = cl.ClaimFlex2ID
        WHERE   c.ClaimID = @ClaimID;

		SELECT	ClaimFlex2ID ClaimFlex2Id, Flex2
		FROM	dbo.ClaimFlex2;

		SELECT  NoteType = cnt.TypeName, cn.NoteText
		FROM    dbo.ClaimNote AS [cn]
				LEFT JOIN dbo.ClaimNoteType AS [cnt] ON cnt.ClaimNoteTypeID = cn.ClaimNoteTypeID
		WHERE   cn.ClaimID = @ClaimID;
    END
GO

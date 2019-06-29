SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       6/28/2019
 Description:       Searches for Customer / Patient by Patient First Name (Global Search Bar)
 Example Execute:
                    EXECUTE [claims].[uspClaimSearchByClaimNumber] '0309'
 =============================================
*/
CREATE PROCEDURE [claims].[uspClaimSearchByClaimNumber]
(
	@SearchTerm VARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    DECLARE @WildCard CHAR(1) = '%';
    SELECT c.ClaimID AS ClaimId,
           c.ClaimNumber AS ClaimNumber,
           p.LastName AS LastName,
           p.FirstName AS FirstName
    FROM dbo.Claim AS c INNER JOIN dbo.Patient AS p ON p.PatientID = c.PatientID
    WHERE c.ClaimNumber LIKE CONCAT(@SearchTerm, @WildCard);
END;
GO

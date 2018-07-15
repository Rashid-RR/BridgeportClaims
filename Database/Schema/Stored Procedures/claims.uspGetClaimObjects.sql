SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/14/2018
 Description:        
 Example Execute:
                    EXECUTE [claims].[uspGetClaimObjects] 775
 =============================================
*/
CREATE PROC [claims].[uspGetClaimObjects] (@ClaimID INT)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;

        SELECT  DocumentTypeId = dt.DocumentTypeID, dt.TypeName
        FROM    dbo.DocumentType AS [dt];

        SELECT  [Date] = p.DatePosted
               ,p.CheckNumber
               ,p2.RxNumber
               ,RxDate = CAST(p2.DateFilled AS DATE)
               ,CheckAmount = p.AmountPaid
        FROM    dbo.PrescriptionPayment AS [p]
                INNER JOIN dbo.Prescription AS [p2] ON p2.PrescriptionID = p.PrescriptionID
        WHERE   p2.ClaimID = @ClaimID;

		SELECT ps.PrescriptionStatusID PrescriptionStatusId, ps.StatusName FROM dbo.PrescriptionStatus AS ps;

		SELECT GenderId = [g].[GenderID], [g].[GenderName] FROM [dbo].[Gender] AS [g];

		SELECT StateId = us.StateID, us.StateName FROM dbo.UsState AS us;
    END
GO

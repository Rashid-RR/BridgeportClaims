SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       10/28/2018
 Description:       Gets the payment details of an indexed check document.
 Example Execute:
                    EXECUTE [dbo].[uspGetIndexedCheckDetails] 89738
 =============================================
*/
CREATE PROC [dbo].[uspGetIndexedCheckDetails] @DocumentID INT
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SELECT  [pp].[PrescriptionPaymentID] PrescriptionPaymentId
            ,[ci].[CheckNumber]
            ,[pp].[AmountPaid]
            ,[pp].[DatePosted]
            ,[IndexedBy] = [u].[FirstName] + ' ' + [u].[LastName]
            ,[p].[RxNumber]
            ,[RxDate] = [p].[DateFilled]
    FROM    [dbo].[Document] AS [d]
            INNER JOIN [dbo].[CheckIndex] AS [ci] ON [d].[DocumentID] = [ci].[DocumentID]
            INNER JOIN [dbo].[AspNetUsers] AS [u] ON [ci].[ModifiedByUserID] = [u].[ID]
            INNER JOIN [dbo].[PrescriptionPayment] AS [pp] ON [d].[DocumentID] = [pp].[DocumentID]
            INNER JOIN [dbo].[Prescription] AS [p] ON [pp].[PrescriptionID] = [p].[PrescriptionID]
    WHERE   [d].[DocumentID] = @DocumentID;
END
GO

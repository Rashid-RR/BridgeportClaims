SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       11/14/2018
 Description:       Gets the data for the Collections Bonus report.
 Example Execute:
                    EXECUTE [rpt].[uspCollectionsBonus] '09642ba1-ad23-41b6-9369-41e466a1e4ed', 11, 2018;
 =============================================
*/
CREATE PROC [rpt].[uspCollectionsBonus]
(
	@UserID NVARCHAR(128),
	@Month INT,
	@Year INT
)
AS BEGIN
	DECLARE @StartDate DATE = '11/1/2018';
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	-- Testing...
	-- SET @UserID = '09642ba1-ad23-41b6-9369-41e466a1e4ed';
    WITH [CollectionsCTE]
    AS
    (
        SELECT   [c].[ClaimID]
				,[PatientName] = [pat].[FirstName] + ' ' + [pat].[LastName]
                ,[p].[NoteAuthorID]
                ,[pay].[AmountPaid]
                ,[pay].[DatePosted]
                ,[LastNoteCreated] = DENSE_RANK() OVER (PARTITION BY [p].[PrescriptionID] ORDER BY [p].[NoteCreatedOn] DESC)
        FROM    [dbo].[vwPrescriptionNote] AS [p] WITH (NOEXPAND)
                INNER JOIN [dbo].[Prescription] AS [pre] ON [p].[PrescriptionID] = [pre].[PrescriptionID]
                INNER JOIN [dbo].[PrescriptionPayment] AS [pay] ON [pre].[PrescriptionID] = [pay].[PrescriptionID]
                INNER JOIN [dbo].[Invoice] AS [i] ON [pre].[InvoiceID] = [i].[InvoiceID]
                INNER JOIN [dbo].[Claim] AS [c] ON [p].[ClaimID] = [c].[ClaimID]
                INNER JOIN [dbo].[Patient] AS [pat] ON [c].[PatientID] = [pat].[PatientID]
        WHERE   1 = 1
				AND DATEDIFF(DAY, [i].[InvoiceDate], [pay].[DatePosted]) > 45
                AND [pay].[DatePosted] >= @StartDate
    )
    SELECT  [c].[ClaimID] AS [ClaimId],
			[c].[PatientName],
			[c].[DatePosted],
			[c].[AmountPaid],
			ISNULL(ROUND([c].[AmountPaid] * 0.015, 2), 0.0) BonusAmount
    FROM    [CollectionsCTE] [c]
    WHERE   [c].[LastNoteCreated] = 1
			AND [c].[NoteAuthorID] = @UserID
			AND MONTH([c].[DatePosted]) = @Month
			AND YEAR([c].[DatePosted]) = @Year;
    END
GO

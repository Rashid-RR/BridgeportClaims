SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/16/2018
 Description:       Gets the Skipped Payments that have been Archived. 
 Example Execute:
					DECLARE @Carriers [dbo].[udtPayorID], @TotalRowCount INT
                    EXECUTE [rpt].[uspGetArchivedSkippedPayments] @Carriers, 1, 5000, @TotalRowCount OUTPUT
					SELECT @TotalRowCount;
 =============================================
*/
CREATE PROC [rpt].[uspGetArchivedSkippedPayments]
(
	@Carriers [dbo].[udtPayorID] READONLY,
	@PageNumber INT,
	@PageSize INT,
	@TotalRowCount INT OUTPUT
)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;

		CREATE TABLE #Carriers (PayorID INT NOT NULL PRIMARY KEY);

		IF NOT EXISTS (SELECT * FROM @Carriers)
			BEGIN
				INSERT #Carriers (PayorID) SELECT p.PayorID FROM dbo.Payor AS p
			END
		ELSE
			BEGIN
				INSERT #Carriers (PayorID) SELECT PayorID FROM @Carriers
			END

		CREATE TABLE #Results (
				RowId INT NOT NULL PRIMARY KEY,
				PrescriptionId INT NOT NULL,
				ClaimId INT NOT NULL,
				[ClaimNumber] [varchar](255) NOT NULL,
				[LastName] [varchar](155) NOT NULL,
				[FirstName] [varchar](155) NOT NULL,
				[AmountPaid] [money] NULL,
				[RxNumber] [varchar](100) NOT NULL,
				[RxDate] DATETIME2 NOT NULL,
				[AdjustorName] [varchar](255) NULL,
				[AdjustorPh] [varchar](30) NULL,
				[Carrier] [varchar](255) NOT NULL,
				[CarrierPh] [varchar](30) NULL,
				[ReversedDate] [datetime2](7) NULL,
				[PrescriptionStatus] [varchar](100) NULL,
				[InvoiceNumber] [varchar](100) NULL
			);

		INSERT #Results (RowId, PrescriptionId, ClaimId, ClaimNumber,LastName,FirstName,AmountPaid,RxNumber,RxDate,AdjustorName,
							 AdjustorPh,Carrier,CarrierPh,ReversedDate,PrescriptionStatus,InvoiceNumber)
        SELECT  RowId = ROW_NUMBER() OVER (ORDER BY cl.ClaimID ASC, pre.DateFilled ASC)
               ,pre.PrescriptionID PrescriptionId
               ,cl.ClaimID ClaimId
               ,cl.ClaimNumber
               ,p.LastName
               ,p.FirstName
               ,prepay.AmountPaid
               ,pre.RxNumber
               ,pre.DateFilled RxDate
               ,a.AdjustorName
               ,a.PhoneNumber AdjustorPh
               ,pay.GroupName Carrier
               ,pay.PhoneNumber CarrierPh
               ,pre.ReversedDate
               ,ps.StatusName PrescriptionStatus
               ,i.InvoiceNumber
        FROM    dbo.SkippedPaymentExclusion c
                INNER JOIN dbo.Prescription AS pre ON pre.PrescriptionID = c.PrescriptionID
                INNER JOIN dbo.Claim AS cl ON pre.ClaimID = cl.ClaimID
                INNER JOIN dbo.Payor AS pay ON cl.PayorID = pay.PayorID
				INNER JOIN #Carriers AS ca ON ca.PayorID = pay.PayorID
                INNER JOIN dbo.Patient AS p ON cl.PatientID = p.PatientID
                OUTER APPLY
				(
					SELECT  AmountPaid = SUM(pp.AmountPaid)
					FROM    dbo.PrescriptionPayment AS pp
					WHERE   pp.PrescriptionID = pre.PrescriptionID
				) AS prepay
                LEFT JOIN dbo.PrescriptionStatus AS ps ON pre.PrescriptionStatusID = ps.PrescriptionStatusID
                LEFT JOIN dbo.Invoice AS i ON pre.InvoiceID = i.InvoiceID
                LEFT JOIN dbo.Adjustor AS a ON cl.AdjustorID = a.AdjustorID;

		SELECT @TotalRowCount = COUNT(*) FROM #Results AS r;

		SELECT r.RowId
				,r.PrescriptionId
				,r.ClaimId
				,r.ClaimNumber
                ,r.LastName
                ,r.FirstName
                ,r.AmountPaid
                ,r.RxNumber
                ,RxDate = FORMAT(r.RxDate, 'M/d/yyyy')
                ,r.AdjustorName
                ,r.AdjustorPh
                ,r.Carrier
                ,r.CarrierPh
                ,ReversedDate = FORMAT(r.ReversedDate, 'M/d/yyyy')
                ,r.PrescriptionStatus
                ,r.InvoiceNumber 
		FROM #Results AS r
		ORDER BY r.ClaimId ASC, r.RxDate ASC
		OFFSET @PageSize * (@PageNumber - 1) ROWS
		FETCH NEXT @PageSize ROWS ONLY;
    END
GO

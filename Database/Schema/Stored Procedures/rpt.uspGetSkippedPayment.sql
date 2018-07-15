SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/1/2018
 Description:       Gets Skipped Payments report 
 Example Execute:
					DECLARE @Carriers [dbo].[udtPayorID], @TotalRowCount INT
                    EXECUTE [rpt].[uspGetSkippedPayment] @Carriers, 1, 5000, @TotalRowCount OUTPUT
					SELECT @TotalRowCount
 =============================================
*/
CREATE PROC [rpt].[uspGetSkippedPayment]
(
	@Carriers [dbo].[udtPayorID] READONLY,
	@PageNumber INT,
	@PageSize INT,
	@TotalRowCount INT OUTPUT
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        
			-- Testing
			/*
			  DECLARE @Carriers [dbo].[udtPayorID], @PageNumber INT = 1, @PageSize INT = 5000, @TotalRowCount INT
			  EXEC(N'DROP TABLE IF EXISTS #AllPayments;')
			  EXEC(N'DROP TABLE IF EXISTS #MadePayments;')
			  EXEC(N'DROP TABLE IF EXISTS #MissedPayments;')
			  EXEC(N'DROP TABLE IF EXISTS #ClaimsPreFiltered;')
			  EXEC(N'DROP TABLE IF EXISTS #Carriers;')
			  EXEC(N'DROP TABLE IF EXISTS #Results;')
			*/

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

			CREATE TABLE #Carriers (PayorID INT NOT NULL PRIMARY KEY)

			IF NOT EXISTS (SELECT * FROM @Carriers)
				BEGIN
					INSERT #Carriers (PayorID) SELECT p.PayorID FROM dbo.Payor AS p
				END
			ELSE
				BEGIN
					INSERT #Carriers (PayorID) SELECT PayorID FROM @Carriers
				END

			CREATE TABLE #ClaimsPreFiltered (ClaimID INT NOT NULL PRIMARY KEY);

			INSERT #ClaimsPreFiltered (ClaimID)
			SELECT  m.ClaimID
			FROM
			(
				SELECT  c.ClaimID, RxDate = p.DateFilled
				FROM    dbo.Claim AS c
						INNER JOIN dbo.Prescription AS p ON c.ClaimID = p.ClaimID
						INNER JOIN dbo.PrescriptionPayment AS pp ON p.PrescriptionID = pp.PrescriptionID
			) m
			INNER JOIN
			(
				SELECT  c.ClaimID, RxDate = p.DateFilled
				FROM    dbo.Prescription AS p
						INNER JOIN dbo.Claim AS c ON p.ClaimID = c.ClaimID
						LEFT JOIN dbo.PrescriptionPayment AS pp ON p.PrescriptionID = pp.PrescriptionID
				WHERE   ISNULL(pp.AmountPaid, 0) = 0
			) mm ON m.ClaimID = mm.ClaimID
			INNER JOIN dbo.Claim AS cl ON m.ClaimID = cl.ClaimID
			INNER JOIN #Carriers AS ca ON cl.PayorID = ca.PayorID
			WHERE   mm.RxDate <= m.RxDate
			GROUP BY m.ClaimID

			-- Assumption: A zero payment counts as a missed payment.
			CREATE TABLE #MissedPayments
			(
				ClaimID INT NOT NULL
			   ,PrescriptionID INT NOT NULL
			   ,AmountPaid MONEY NULL
			   ,RxDate DATETIME2 NOT NULL
			   ,PRIMARY KEY CLUSTERED (PrescriptionID, ClaimID)
			);
			INSERT  #MissedPayments (ClaimID, PrescriptionID, AmountPaid, RxDate)
			SELECT  c.ClaimID, p.PrescriptionID, NULL, p.DateFilled
			FROM    dbo.Prescription AS p
					INNER JOIN dbo.Claim AS c ON p.ClaimID = c.ClaimID
					INNER JOIN #ClaimsPreFiltered AS f ON c.ClaimID = f.ClaimID
					LEFT JOIN dbo.PrescriptionPayment AS pp ON p.PrescriptionID = pp.PrescriptionID
			WHERE   ISNULL(pp.AmountPaid, 0) = 0;

			CREATE TABLE #MadePayments
			(
				ClaimID INT NOT NULL
			   ,PrescriptionID INT NOT NULL 
			   ,AmountPaid MONEY NULL
			   ,RxDate DATETIME2 NOT NULL
			);
			INSERT	#MadePayments (ClaimID, PrescriptionID, AmountPaid, RxDate)
			SELECT	c.ClaimID, p.PrescriptionID, pp.AmountPaid, p.DateFilled RxDate
			FROM    dbo.Claim AS c
					INNER JOIN #ClaimsPreFiltered AS f ON c.ClaimID = f.ClaimID
					INNER JOIN dbo.Prescription AS p ON c.ClaimID = p.ClaimID
					INNER JOIN dbo.PrescriptionPayment AS pp ON p.PrescriptionID = pp.PrescriptionID;

			CREATE TABLE #AllPayments
			(
				ClaimID INT NOT NULL
			   ,PrescriptionID INT NOT NULL
			   ,AmountPaid MONEY NULL
			   ,RxDate DATETIME2 NOT NULL
			   ,NoPayments BIT NOT NULL
			   ,RowID INT NOT NULL
			);
			INSERT #AllPayments (ClaimID, PrescriptionID, AmountPaid, RxDate, NoPayments, RowID)
			SELECT  a.ClaimID
				   ,a.PrescriptionID
				   ,a.AmountPaid
				   ,a.RxDate
				   ,NoPayments = CASE WHEN a.AmountPaid IS NULL THEN DENSE_RANK() OVER (ORDER BY CASE WHEN a.AmountPaid IS NULL THEN 0 ELSE 1 END ASC) ELSE 0 END
				   ,RowID = ROW_NUMBER() OVER (PARTITION BY a.ClaimID ORDER BY a.RxDate ASC, a.PrescriptionID ASC)
			FROM
			(
				SELECT  mp.ClaimID, mp.PrescriptionID, mp.AmountPaid, mp.RxDate
				FROM    #MissedPayments AS mp
				UNION ALL
				SELECT  mp.ClaimID, mp.PrescriptionID, mp.AmountPaid, mp.RxDate
				FROM    #MadePayments AS mp
			) AS a LEFT JOIN dbo.SkippedPaymentExclusion AS spe ON a.PrescriptionID = spe.PrescriptionID
			WHERE 1 = CASE WHEN a.AmountPaid IS NULL AND spe.SkippedPaymentExclusionID IS NULL THEN 1
					       WHEN a.AmountPaid IS NOT NULL THEN 1
						   ELSE 0
					  END

			UPDATE ap SET ap.RxDate = DATEADD(MILLISECOND, ap.RowID, ap.RxDate) FROM #AllPayments AS ap;

			ALTER TABLE #AllPayments WITH CHECK ADD PRIMARY KEY CLUSTERED (ClaimID, RxDate);

			WITH RecursiveSequenceCTE AS
			(
				-- Anchor: First non-payment.
				SELECT  d.ClaimID, d.PrescriptionID, d.AmountPaid, d.RxDate, d.NoPayments, d.RowID, Seq = 1
				FROM    #AllPayments AS d
				WHERE   d.RxDate =
				(
					SELECT  MIN(id.RxDate)
					FROM    #AllPayments AS id
					WHERE   id.ClaimID = d.ClaimID
							AND id.NoPayments = 1
				)
        
				UNION ALL
    
				-- Recursive part
				SELECT
					q.ClaimID,
					q.PrescriptionID,
					q.AmountPaid,
					q.RxDate,
					q.NoPayments,
					q.RowID,
					q.Seq
				FROM 
				(
					-- Next row in sequence of [Date]
					SELECT
						d.ClaimID,
						d.PrescriptionID,
						d.AmountPaid,
						d.RxDate,
						d.NoPayments,
						d.RowID,
						Seq = 
							CASE
								-- Same status, increment sequence
								WHEN d.NoPayments = r.NoPayments
									THEN r.Seq + 1 
								-- Otherwise, restart sequence at 1
								ELSE 1
							END,
						Rn = ROW_NUMBER() OVER (ORDER BY d.RxDate)
					FROM RecursiveSequenceCTE AS r INNER JOIN #AllPayments AS d ON d.ClaimID = R.ClaimID AND d.RxDate > R.RxDate
				) AS q
				WHERE
					-- Current date is the first one
					-- that is higher than the current
					-- recursive date
					q.Rn = 1
			)
			INSERT #Results (RowId, PrescriptionId, ClaimId, ClaimNumber,LastName,FirstName,AmountPaid,RxNumber,RxDate,AdjustorName,
							 AdjustorPh,Carrier,CarrierPh,ReversedDate,PrescriptionStatus,InvoiceNumber)
			SELECT  ROW_NUMBER() OVER (ORDER BY c.ClaimID ASC, c.RxDate ASC)
				   ,pre.PrescriptionID
				   ,cl.ClaimID
				   ,cl.ClaimNumber
				   ,p.LastName
				   ,p.FirstName
				   ,c.AmountPaid
				   ,pre.RxNumber
				   ,pre.DateFilled
				   ,a.AdjustorName
				   ,a.PhoneNumber
				   ,pay.GroupName
				   ,pay.PhoneNumber
				   ,pre.ReversedDate
				   ,ps.StatusName
				   ,i.InvoiceNumber
			FROM    RecursiveSequenceCTE c
					INNER JOIN dbo.Claim AS cl ON c.ClaimID = cl.ClaimID
					INNER JOIN dbo.Payor AS pay ON cl.PayorID = pay.PayorID
					INNER JOIN dbo.Patient AS p ON cl.PatientID = p.PatientID
					INNER JOIN dbo.Prescription AS pre ON pre.PrescriptionID = c.PrescriptionID
					LEFT JOIN dbo.PrescriptionStatus AS ps ON pre.PrescriptionStatusID = ps.PrescriptionStatusID
					LEFT JOIN dbo.Invoice AS i ON pre.InvoiceID = i.InvoiceID
					LEFT JOIN dbo.Adjustor AS a ON cl.AdjustorID = a.AdjustorID
			WHERE   1 = CASE
							WHEN c.NoPayments = 1 THEN 1 -- Obviously, all of the Missed Payments.
							WHEN c.NoPayments = 0
								 AND c.Seq = 1 THEN 1 ELSE 0
						END
			ORDER BY c.ClaimID ASC, c.RxDate ASC
			OPTION (MAXRECURSION 0);
					
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
            
        IF (@@TRANCOUNT > 0)
            COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
            ROLLBACK;
                
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

        RAISERROR(N'%s (line %d): %s', @ErrSeverity, @ErrState, @ErrProc, @ErrLine, @ErrMsg);
    END CATCH
END
GO

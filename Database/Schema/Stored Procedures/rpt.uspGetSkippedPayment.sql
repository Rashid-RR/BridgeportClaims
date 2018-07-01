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
                    EXECUTE [rpt].[uspGetSkippedPayment]
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
        
			-- TODO: implement REAL LOGIC!!!!!!!!!!!!!!

			SET NOCOUNT ON;
			SET XACT_ABORT ON;
			CREATE TABLE #Results (
				[ClaimNumber] [varchar](255) NOT NULL,
				[LastName] [varchar](155) NOT NULL,
				[FirstName] [varchar](155) NOT NULL,
				[AmountPaid] [money] NULL,
				[RxNumber] [varchar](100) NOT NULL,
				[RxDate] [nvarchar](4000) NULL,
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

			INSERT #Results (ClaimNumber,LastName,FirstName,AmountPaid,RxNumber,RxDate,AdjustorName,
							 AdjustorPh,Carrier,CarrierPh,ReversedDate,PrescriptionStatus,InvoiceNumber)
			SELECT  c.ClaimNumber
				   ,p.LastName
				   ,p.FirstName
				   ,pp.AmountPaid
				   ,pre.RxNumber
				   ,RxDate = FORMAT(pre.DateFilled, 'M/d/yyyy')
				   ,a.AdjustorName
				   ,AdjustorPh = a.PhoneNumber
				   ,Carrier = pay.GroupName
				   ,CarrierPh = pay.PhoneNumber
				   ,pre.ReversedDate
				   ,PrescriptionStatus = ps.StatusName
				   ,i.InvoiceNumber
			FROM    dbo.Claim AS c
					INNER JOIN dbo.Payor AS pay ON c.PayorID = pay.PayorID
					INNER JOIN #Carriers ca ON pay.PayorID = ca.PayorID
					INNER JOIN dbo.Patient AS p ON c.PatientID = p.PatientID
					INNER JOIN dbo.Prescription AS pre ON c.ClaimID = pre.ClaimID
					LEFT JOIN dbo.Invoice AS i ON pre.InvoiceID = i.InvoiceID
					LEFT JOIN dbo.PrescriptionStatus AS ps ON pre.PrescriptionStatusID = ps.PrescriptionStatusID
					LEFT JOIN dbo.PrescriptionPayment AS pp ON pre.PrescriptionID = pp.PrescriptionID
					LEFT JOIN dbo.Adjustor AS a ON c.AdjustorID = a.AdjustorID  
					
			SELECT @TotalRowCount = COUNT(*) FROM #Results AS r

			SELECT r.ClaimNumber
                  ,r.LastName
                  ,r.FirstName
                  ,r.AmountPaid
                  ,r.RxNumber
                  ,r.RxDate
                  ,r.AdjustorName
                  ,r.AdjustorPh
                  ,r.Carrier
                  ,r.CarrierPh
                  ,r.ReversedDate
                  ,r.PrescriptionStatus
                  ,r.InvoiceNumber 
			FROM #Results AS r
			ORDER BY r.ClaimNumber, r.Carrier
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

        RAISERROR(N'%s (line %d): %s',    -- Message text w formatting
            @ErrSeverity,        -- Severity
            @ErrState,            -- State
            @ErrProc,            -- First argument (string)
            @ErrLine,            -- Second argument (int)
            @ErrMsg);            -- First argument (string)
    END CATCH
END
GO

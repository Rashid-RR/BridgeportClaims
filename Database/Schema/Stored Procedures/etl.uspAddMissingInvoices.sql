SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/23/2017
	Description:	The Prescription 
	Sample Execute:
					EXEC etl.uspAddMissingInvoices 1
*/
CREATE PROC [etl].[uspAddMissingInvoices] @DebugOnly BIT = 0
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @SQL NVARCHAR(1000);
	BEGIN TRY
		BEGIN TRAN
		SET @SQL = N'ALTER TABLE dbo.Invoice ADD PrescriptionID INT NULL;';
		IF @DebugOnly = 1
			PRINT @SQL;
		ELSE
			EXECUTE sys.sp_executesql @SQL;

		SET @SQL =
	  N'WITH MissingInvoicesCTE ( InvoiceNumber, InvoiceDate, RowID, PrescriptionID )
		AS ( SELECT slf.[4]
				  , slf.[5]
				  , slf.RowID
				  , slf.PrescriptionID
			 FROM   etl.StagedLakerFile AS slf
			 WHERE slf.[4] IS NOT NULL
					AND slf.[5] IS NOT NULL
					AND slf.PrescriptionID IS NOT NULL
		   )
		INSERT dbo.Invoice (InvoiceNumber, InvoiceDate, Amount, ETLRowID, PrescriptionID)
		SELECT c.InvoiceNumber
			 , c.InvoiceDate
			 , 0
			 , c.RowID
			 , c.PrescriptionID
		FROM   MissingInvoicesCTE AS c
			   LEFT JOIN dbo.Invoice AS i ON i.InvoiceNumber = c.InvoiceNumber AND i.InvoiceDate = c.InvoiceDate
			   LEFT JOIN dbo.Prescription AS p ON p.InvoiceID = i.InvoiceID
		WHERE  i.InvoiceID IS NULL
			   AND p.PrescriptionID IS NULL';
		IF @DebugOnly = 1
			PRINT @SQL;
		ELSE
			EXECUTE sys.sp_executesql @SQL;

		SET @SQL = N'ALTER TABLE dbo.Invoice DROP COLUMN PrescriptionID';
		IF @DebugOnly = 1
			PRINT @SQL;
		ELSE
			EXECUTE sys.sp_executesql @SQL;
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
			, @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE()

		RAISERROR(N'%s (line %d): %s',	-- Message text w formatting
			@ErrSeverity,		-- Severity
			@ErrState,			-- State
			@ErrProc,			-- First argument (string)
			@ErrLine,			-- Second argument (int)
			@ErrMsg)			-- First argument (string)
	END CATCH
END
GO

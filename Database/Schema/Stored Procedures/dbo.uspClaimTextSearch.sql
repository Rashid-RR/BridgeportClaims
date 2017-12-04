SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/29/2017
	Description:	Proc that searches the Claim and Patient tables by
					Last Name, First Name or Claim Number.
	Sample Execute:
					EXEC [dbo].[uspClaimTextSearch] 'black'
*/
CREATE PROC [dbo].[uspClaimTextSearch]
    @SearchText VARCHAR(500)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;

		IF (@SearchText IS NULL)
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				RAISERROR(N'Error. The @SearchText parameter cannot be NULL.', 16, 1) WITH NOWAIT;
				RETURN;
			END

		SELECT          [c].[ClaimID] ClaimId
					  , [p].[LastName]
					  , [p].[FirstName]
					  , [c].[ClaimNumber]
					  , [py].[GroupName] GroupNumber
		FROM            [dbo].[Claim]   AS [c]
						INNER JOIN  [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
						INNER JOIN  [dbo].[Payor]   AS [py] ON [py].[PayorID] = [c].[PayorID]
		WHERE           [p].[FirstName] LIKE '%' + @SearchText + '%'
						OR  [p].[LastName] LIKE '%' + @SearchText + '%'
						OR  [c].[ClaimNumber] LIKE '%' + @SearchText + '%'
	
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

        RAISERROR(N'%s (line %d): %s',	-- Message text w formatting
			@ErrSeverity,		-- Severity
			@ErrState,			-- State
			@ErrProc,			-- First argument (string)
			@ErrLine,			-- Second argument (int)
			@ErrMsg);			-- First argument (string)
	END CATCH
END
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		2/4/2018
 Description:		Responsible for gathering the Dashboard KPI's
 Example Execute:
					EXECUTE [dbo].[uspDashboard]
 =============================================
*/
CREATE PROC [dbo].[uspDashboard]
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
		DECLARE @One INT = 1
		DECLARE @LocalNow DATE = CONVERT(DATE, [dtme].[udfGetLocalDate]()),
			    @LastWorkDay DATE;

		SELECT      TOP (@One)
					@LastWorkDay = c.[DateID]
		FROM        [dtme].[Calendar] AS [c]
		WHERE       c.[DateID] < @LocalNow
					AND c.[IsWeekend] = 0
					AND c.[IsHoliday] = 0
		ORDER BY    c.[DateID] DESC

		-- Testing, hack the date:
		-- SET @LastWorkDay = '2018-01-09'

		-- Unindexed Images KPI
		DECLARE @TotalImagesScannedYesterday INT,
				@TotalImagesIndexedYesterday INT,
				@TotalImagesRemainingYesterday INT,
				@TotalNewPrescriptions INT,
				@TotalReversedPrescriptions INT,
				@TotalNewEpisodes INT,
				@TotalResolvedEpisodes INT,
				@TotalUnresolvedEpisodes INT,
				@FileWatcherHealthy BIT;

		SELECT      TOP (@One) @FileWatcherHealthy = CONVERT(BIT,IIF([nl].[Level] = 'Info', 1, 0))
		FROM        [util].[NLog] AS [nl]
		WHERE		nl.[MachineName] = 'BPSERVER'
		ORDER BY    nl.[NLogID] DESC

		IF @FileWatcherHealthy IS NULL
			SET @FileWatcherHealthy = 0;


		SELECT  @TotalNewEpisodes = SUM(IIF([e].[Created] = @LastWorkDay, 1, 0)),
				@TotalResolvedEpisodes = SUM(IIF([e].[ResolvedDateUTC] IS NOT NULL, 1, 0)),
				@TotalUnresolvedEpisodes = SUM(IIF([e].[ResolvedDateUTC] IS NULL, 1, 0))
		FROM    [dbo].[Episode] AS [e]

		CREATE TABLE #Document
		(   DocumentID   INT  NOT NULL PRIMARY KEY
		  , DocumentDate DATE NOT NULL
		  , INDEX idxDocumentTemporaryDocumentDocumentDate NONCLUSTERED ([DocumentDate])
		)
		INSERT INTO [#Document]
		(   [DocumentID]
		  , [DocumentDate])
		SELECT  d.[DocumentID]
			  , d.[DocumentDate]
		FROM    [dbo].[Document] AS [d]
		WHERE   [d].[DocumentDate] = @LastWorkDay
				AND [d].[Archived] = 0


		SELECT  @TotalImagesScannedYesterday = COUNT(*)
		FROM    [#Document] AS [d]
		WHERE   [d].[DocumentDate] = @LastWorkDay;

		SELECT          @TotalImagesIndexedYesterday = COUNT(*)
		FROM            [#Document]      AS [d]
			INNER JOIN  [dbo].[DocumentIndex] AS [di] ON [di].[DocumentID] = [d].[DocumentID]
		WHERE           [d].[DocumentDate] = @LastWorkDay;

		SELECT          @TotalImagesRemainingYesterday = COUNT(*)
		FROM            [#Document]      AS [d]
			LEFT JOIN   [dbo].[DocumentIndex] AS [di] ON [di].[DocumentID] = [d].[DocumentID]
		WHERE           [di].[DocumentID] IS NULL
						AND [d].[DocumentDate] = @LastWorkDay

		SELECT  @TotalNewPrescriptions = COUNT(*), @TotalReversedPrescriptions = SUM(IIF([p].[IsReversed] = 1, 1, 0))
		FROM    [dbo].[Prescription] AS [p]
		WHERE   CONVERT(DATE, [p].[CreatedOnUTC]) >= @LastWorkDay

		SELECT  @LastWorkDay LastWorkDate, 
				@TotalImagesScannedYesterday TotalImagesScanned, 
				@TotalImagesIndexedYesterday TotalImagesIndexed,
				@TotalImagesRemainingYesterday TotalImagesRemaining,
				DiariesAdded = ISNULL(COUNT(*),0),
				TotalDiariesResolved = ISNULL(SUM(IIF([d].[DateResolved] IS NOT NULL, 1, 0)),0),
				TotalDiariesUnResolved = ISNULL(SUM(IIF([d].[DateResolved] IS NOT NULL, 0, 1)),0),
				NewClaims = (SELECT COUNT(*) FROM [dbo].[Claim] AS [c]
				WHERE CONVERT(DATE, [c].[CreatedOnUTC]) >= @LastWorkDay),
				NewPrescriptions = @TotalNewPrescriptions,
				NewReversedPrescriptions = @TotalReversedPrescriptions,
				NewInvoicesPrinted = (SELECT COUNT(*) FROM [dbo].[Invoice] AS [i] WHERE [i].[InvoiceDate] = @LastWorkDay),
				NewPaymentsPosted = (SELECT SUM([pp].[AmountPaid]) FROM [dbo].[PrescriptionPayment] AS [pp] WHERE [pp].[DatePosted] = @LastWorkDay),
				NewEpisodes = @TotalNewEpisodes,
				TotalResolvedEpisodes = @TotalResolvedEpisodes,
				TotalUnresolvedEpisodes = @TotalUnresolvedEpisodes,
				FileWatcherHealthy = @FileWatcherHealthy
		FROM    [dbo].[Diary] AS [d]
		WHERE   d.[CreatedDate] = @LastWorkDay

			
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

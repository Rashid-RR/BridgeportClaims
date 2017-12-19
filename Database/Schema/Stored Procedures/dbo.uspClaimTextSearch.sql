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
					EXEC [dbo].[uspClaimTextSearch] 'BLACK|and|TXA0165', 0, '|'
*/
CREATE PROC [dbo].[uspClaimTextSearch]
(
    @SearchText VARCHAR(800), 
	@ExactMatch BIT,
	@Delimiter CHAR(1)
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
		DECLARE @WildCard CHAR(1) = '%', @NewLine CHAR(1) = CHAR(10) + CHAR(13)
		IF (@SearchText IS NULL)
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				RAISERROR(N'Error. The @SearchText parameter cannot be NULL.', 16, 1) WITH NOWAIT;
				RETURN -1;
			END

		DECLARE @SearchTerms TABLE (ID BIGINT NOT NULL, SearchText VARCHAR(800) NOT NULL)
		INSERT @SearchTerms ([ID], [SearchText])
		SELECT  [ItemNumber]
			  , [Item]
		FROM    [util].[udfDelimitedSplit](@Delimiter, @SearchText)
		IF (SELECT COUNT(*) FROM @SearchTerms AS st) = 1
			BEGIN
				SELECT          ClaimId     = [c].[ClaimID]
							  , [p].[LastName]
							  , [p].[FirstName]
							  , [c].[ClaimNumber]
							  , GroupNumber = [py].[GroupName]
				FROM            [dbo].[Claim]   AS [c]
					INNER JOIN  [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
					INNER JOIN  [dbo].[Payor]   AS [py] ON [py].[PayorID] = [c].[PayorID]
					INNER JOIN  @SearchTerms    AS [st] ON 
						[p].[FirstName] LIKE CONCAT(IIF(@ExactMatch = 1, '', @WildCard), [st].[SearchText], IIF(@ExactMatch = 1, '', @WildCard))
						OR  [p].[LastName] LIKE CONCAT(IIF(@ExactMatch = 1, '', @WildCard), [st].[SearchText], IIF(@ExactMatch = 1, '', @WildCard))
						OR  [c].[ClaimNumber] LIKE CONCAT(IIF(@ExactMatch = 1, '', @WildCard), [st].[SearchText], IIF(@ExactMatch = 1, '', @WildCard))
			END
		ELSE
			BEGIN
				/* declare variables */
				DECLARE @SearchTermInternal VARCHAR(800)
					   ,@SQLStatement NVARCHAR(4000);
				SET @SQLStatement = N'DECLARE @ExactMatch BIT = ' + CONVERT(NVARCHAR(1), @ExactMatch) + ';' + @NewLine +
				N'DECLARE @WildCard CHAR(1) = ''%'';' + @NewLine +
				N'SELECT ClaimId = [c].[ClaimID]
						, [p].[LastName]
						, [p].[FirstName]
						, [c].[ClaimNumber]
						, GroupNumber = [py].[GroupName]
				FROM [dbo].[Claim] AS [c]
					INNER JOIN  [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
					INNER JOIN  [dbo].[Payor]   AS [py] ON [py].[PayorID] = [c].[PayorID]
				WHERE 1 = 1 ';
					  

				DECLARE SearchCrsr CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
				SELECT st.SearchText FROM @SearchTerms AS st;
				
				OPEN SearchCrsr;
				
				FETCH NEXT FROM SearchCrsr INTO @SearchTermInternal;
				
				WHILE (0 = @@FETCH_STATUS)
					BEGIN
						SET @SQLStatement += N' AND (' +
						N'[p].[FirstName] LIKE CONCAT(IIF(@ExactMatch = 1, '''', @WildCard), ''' + @SearchTermInternal + N''', IIF(@ExactMatch = 1, '''', @WildCard)) ' + @NewLine +
						N'OR  [p].[LastName] LIKE CONCAT(IIF(@ExactMatch = 1, '''', @WildCard), ''' + @SearchTermInternal + N''', IIF(@ExactMatch = 1, '''', @WildCard)) ' + @NewLine +
						N'OR  [c].[ClaimNumber] LIKE CONCAT(IIF(@ExactMatch = 1, '''', @WildCard), ''' + @SearchTermInternal + N''', IIF(@ExactMatch = 1, '''', @WildCard))' + N') ' + @NewLine;
						FETCH NEXT FROM SearchCrsr INTO @SearchTermInternal;
					END
				
				CLOSE SearchCrsr;
				DEALLOCATE SearchCrsr;
				EXEC [sys].[sp_executesql] @statement = @SQLStatement;
			END
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

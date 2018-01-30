SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/29/2017
	Description:	CRUD Proc inserting into [dbo].[DocumentIndex]
	Modified:		Removed performance Hog: [dtme].[udfGetLocalDateTime]
	Sample Execute:
					DECLARE @TotalPageSize INT
					EXEC [dbo].[uspGetEpisodes] @Resolved = 1, @OwnerID = 'e9c4cd49-251f-4782-97be-2e8f6d90c328',
						@SortColumn = N'PatientName', @SortDirection = N'asc',
						@PageNumber = 1, @PageSize = 50000, @TotalPageSize = @TotalPageSize
*/
CREATE PROC [dbo].[uspGetEpisodes]
(
	@Resolved BIT,
	@OwnerID NVARCHAR(128),
	@SortColumn VARCHAR(50),
	@SortDirection VARCHAR(5),
	@PageNumber INTEGER,
	@PageSize INTEGER,
	@TotalPageSize INTEGER OUTPUT
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;

		-- Param Sniffing
		DECLARE @iResolved BIT = @Resolved,
				@iSortColumn VARCHAR(50) = @SortColumn,
				@iSortDirection VARCHAR(5) = @SortDirection,
				@iPageNumber INTEGER = @PageNumber,
				@iPageSize INTEGER = @PageSize;

		CREATE TABLE [#Episodes](
			[EpisodeId] [int] NOT NULL,
			[Owner] [nvarchar](202) NOT NULL,
			[Created] [datetime2](7) NULL,
			[PatientName] [nvarchar](312) NOT NULL,
			[ClaimNumber] [varchar](255) NOT NULL,
			[Type] [varchar](255) NULL,
			[Pharmacy] [varchar](60) NULL,
			[Carrier] [varchar](255) NOT NULL,
			[EpisodeNote] [varchar](8000) NOT NULL
		);

		DECLARE @Spacing NVARCHAR(2) = N', ';
		INSERT INTO [#Episodes] ([EpisodeId],[Owner],[Created],[PatientName],[ClaimNumber]
				,[Type],[Pharmacy],[Carrier],[EpisodeNote])
		SELECT          EpisodeId     = [ep].[EpisodeID]
					  ,	[Owner]       = CONCAT([u].[LastName], @Spacing, [u].[FirstName])
					  , [Created]     = ep.CreatedOnUTC
					  , [PatientName] = CONCAT([pa].LastName, @Spacing, [pa].FirstName)
					  , [ClaimNumber] = cl.ClaimNumber
					  , [Type]        = et.TypeName
					  , [Pharmacy]    = ph.PharmacyName
					  , [Carrier]     = py.GroupName
					  , [EpisodeNote] = ep.Note
		FROM            dbo.Episode         AS ep
			INNER JOIN  dbo.Claim           AS cl ON ep.ClaimID = cl.ClaimID
			INNER JOIN  dbo.Patient         AS pa ON cl.PatientID = pa.PatientID
			INNER JOIN  dbo.Payor           AS py ON cl.PayorID = py.PayorID
			INNER JOIN  dbo.Pharmacy        AS ph ON ep.[PharmacyNABP] = ph.NABP
			LEFT JOIN   dbo.EpisodeType     AS et ON ep.EpisodeTypeID = et.EpisodeTypeID
			LEFT JOIN   dbo.DocumentIndex   AS di ON ep.DocumentID = di.DocumentID
			LEFT JOIN   [dbo].[AspNetUsers] AS [u] ON [u].[ID] = [ep].[AssignedUserID]
		WHERE @iResolved = CASE WHEN ep.ResolvedDateUTC IS NOT NULL THEN 1 ELSE 0 END
			  AND (@OwnerID IS NULL OR [u].[ID] = @OwnerID)

		SELECT @TotalPageSize = COUNT(*) FROM [#Episodes] AS [e]


		SELECT [e].[EpisodeId]
             , [e].[Owner]
             , [e].[Created]
             , [e].[PatientName]
             , [e].[ClaimNumber]
             , [e].[Type]
             , [e].[Pharmacy]
             , [e].[Carrier]
             , [e].[EpisodeNote] FROM [#Episodes] AS [e]
		ORDER BY CASE WHEN @iSortColumn = 'EpisodeId' AND @iSortDirection = 'ASC'
				THEN [e].[EpisodeID] END ASC,
			 CASE WHEN @iSortColumn = 'EpisodeId' AND @iSortDirection = 'DESC'
				THEN [e].[EpisodeID] END DESC,
			 CASE WHEN @iSortColumn = 'Owner' AND @iSortDirection = 'ASC'
				THEN [e].[Owner] END ASC,
			 CASE WHEN @iSortColumn = 'Owner' AND @iSortDirection = 'DESC'
				THEN [e].[Owner] END DESC,
			 CASE WHEN @iSortColumn = 'Created' AND @iSortDirection = 'ASC'
				THEN [e].[Created] END ASC,
			 CASE WHEN @iSortColumn = 'Created' AND @iSortDirection = 'DESC'
				THEN [e].[Created] END DESC,
			 CASE WHEN @iSortColumn = 'PatientName' AND @iSortDirection = 'ASC'
				THEN [e].[PatientName] END ASC,
			 CASE WHEN @iSortColumn = 'PatientName' AND @iSortDirection = 'DESC'
				THEN [e].[PatientName] END DESC,
			 CASE WHEN @iSortColumn = 'ClaimNumber' AND @iSortDirection = 'ASC'
				THEN [e].[ClaimNumber] END ASC,
			 CASE WHEN @iSortColumn = 'ClaimNumber' AND @iSortDirection = 'DESC'
				THEN [e].[ClaimNumber] END DESC,
			 CASE WHEN @iSortColumn = 'Type' AND @iSortDirection = 'ASC'
				THEN e.[Type] END ASC,
			 CASE WHEN @iSortColumn = 'Type' AND @iSortDirection = 'DESC'
				THEN e.[Type] END DESC,
			 CASE WHEN @iSortColumn = 'Pharmacy' AND @iSortDirection = 'ASC'
				THEN e.[Pharmacy] END ASC,
			 CASE WHEN @iSortColumn = 'Pharmacy' AND @iSortDirection = 'DESC'
				THEN e.[Pharmacy] END DESC,
			 CASE WHEN @iSortColumn = 'Carrier' AND @iSortDirection = 'ASC'
				THEN e.[Carrier] END ASC,
			 CASE WHEN @iSortColumn = 'Carrier' AND @iSortDirection = 'DESC'
				THEN e.[Carrier] END DESC,
			 CASE WHEN @iSortColumn = 'EpisodeNote' AND @iSortDirection = 'ASC'
				THEN [e].[EpisodeNote] END ASC,
			 CASE WHEN @iSortColumn = 'EpisodeNote' AND @iSortDirection = 'DESC'
				THEN [e].[EpisodeNote] END DESC
		OFFSET @iPageSize * (@iPageNumber - 1) ROWS
		FETCH NEXT @iPageSize ROWS ONLY;

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
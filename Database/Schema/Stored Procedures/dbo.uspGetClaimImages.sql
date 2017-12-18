SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	12/13/2017
	Description:	Proc that returns the results to be displayed in the Images blade of the Claims page.
	Sample Execute:
					EXEC [dbo].[uspGetClaimImages]
*/
CREATE PROC [dbo].[uspGetClaimImages]
(
	@ClaimID INTEGER,
	@SortColumn VARCHAR(50),
	@SortDirection VARCHAR(5),
	@PageNumber INTEGER,
	@PageSize INTEGER,
	@TotalRows INTEGER OUTPUT
)
AS 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;

		CREATE TABLE [#Images](
			DocumentID [int] NOT NULL PRIMARY KEY CLUSTERED,
			[Created] [datetime2](7) NOT NULL,
			[Type] [varchar](255) NOT NULL,
			[RxDate] [datetime2](7) NULL,
			[RxNumber] [varchar](100) NULL,
			[FileName] [varchar](1000) NOT NULL,
			[FileUrl] [nvarchar](500) NOT NULL
		);

		INSERT [#Images] ([DocumentID],[Created],[Type],[RxDate],[RxNumber],[FileName],[FileUrl])
		SELECT          DocumentId = [d].[DocumentID]
					  , Created = [d].[CreationTimeLocal]
					  , [Type]  = [dt].[TypeName]
					  , [di].[RxDate]
					  , [di].[RxNumber]
					  , [d].[FileName]
					  , [d].[FileUrl]
		FROM            [dbo].[Document]      AS [d]
			INNER JOIN  [dbo].[DocumentIndex] AS [di] ON [di].[DocumentID] = [d].[DocumentID]
			INNER JOIN  [dbo].[DocumentType]  AS [dt] ON [dt].[DocumentTypeID] = [di].[DocumentTypeID]
		WHERE		[di].[ClaimID] = @ClaimID

		SELECT @TotalRows = COUNT(*) FROM [#Images]

		SELECT [i].[DocumentID] DocumentId
             , [i].[Created]
             , [i].[Type]
             , [i].[RxDate]
             , [i].[RxNumber]
             , [i].[FileName]
			 , [i].[FileUrl]
		FROM [#Images] AS [i]
		ORDER BY CASE WHEN @SortColumn = 'DocumentID' AND @SortDirection = 'ASC'
					THEN [i].[DocumentID] END ASC,
				 CASE WHEN @SortColumn = 'DocumentID' AND @SortDirection = 'DESC'
					THEN [i].[DocumentID] END DESC,
				 CASE WHEN @SortColumn = 'Created' AND @SortDirection = 'ASC'
					THEN [i].[Created] END ASC,
				 CASE WHEN @SortColumn = 'Created' AND @SortDirection = 'DESC'
					THEN [i].[Created] END DESC,
				 CASE WHEN @SortColumn = 'Type' AND @SortDirection = 'ASC'
					THEN [i].[Type] END ASC,
				 CASE WHEN @SortColumn = 'Type' AND @SortDirection = 'DESC'
					THEN [i].[Type] END DESC,
				 CASE WHEN @SortColumn = 'RxDate' AND @SortDirection = 'ASC'
					THEN [i].[RxDate] END ASC,
				 CASE WHEN @SortColumn = 'RxDate' AND @SortDirection = 'DESC'
					THEN [i].[RxDate] END DESC,
				 CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'ASC'
					THEN [i].[RxNumber] END ASC,
				 CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'DESC'
					THEN [i].[RxNumber] END DESC,
				 CASE WHEN @SortColumn = 'FileName' AND @SortDirection = 'ASC'
					THEN [i].[FileName] END ASC,
				 CASE WHEN @SortColumn = 'FileName' AND @SortDirection = 'DESC'
					THEN [i].[FileName] END DESC
			OFFSET @PageSize * (@PageNumber - 1) ROWS
			FETCH NEXT @PageSize ROWS ONLY;

		IF (@@TRANCOUNT > 0)
			COMMIT
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

GO
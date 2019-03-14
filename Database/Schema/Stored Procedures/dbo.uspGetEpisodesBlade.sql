SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		2/10/2018
 Description:		Gets the Episodes for the Episodes blade
 Example Execute:
					EXECUTE [dbo].[uspGetEpisodesBlade] 775, 'Type', 'DESC'
 =============================================
*/
CREATE PROC [dbo].[uspGetEpisodesBlade]
(
	@ClaimID INTEGER,
	@SortColumn VARCHAR(50),
	@SortDirection VARCHAR(5)
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
            DECLARE @Call CHAR(4) = 'CALL';

			CREATE TABLE #Episodes
			(
				[Id] [int] NOT NULL PRIMARY KEY,
				[Created] [date] NULL,
				[Owner] [nvarchar](201) NULL,
				[Type] [varchar](255) NOT NULL,
				[Pharmacy] [varchar](60) NULL,
				[RxNumber] [varchar](100) NULL,
				[Resolved] [bit] NULL,
				[NoteCount] [int] NULL,
				[HasTree] [BIT] NOT NULL
			)
			INSERT INTO [#Episodes]
			(   [Id]
			  , [Created]
			  , [Owner]
			  , [Type]
			  , [Pharmacy]
			  , [RxNumber]
			  , [Resolved]
			  , [NoteCount]
			  , HasTree)
			SELECT  e.[Id]
					, e.[Created]
					, e.[Owner]
					, e.[Type]
					, e.[Pharmacy]
					, e.[RxNumber]
					, e.[Resolved]
					, e.[NoteCount]
					, e.HasTree
			FROM    dbo.vwEpisodeBlade AS [e]
			WHERE	[e].[ClaimID] = @ClaimID
                    AND [e].[Category] = @Call
			
			SELECT [c].[Id]
                 , [c].[Created]
                 , [c].[Owner]
                 , [c].[Type]
                 , [c].[Pharmacy]
                 , [c].[RxNumber]
                 , [c].[Resolved]
                 , [c].[NoteCount]
				 , c.HasTree
			FROM [#Episodes] c
			ORDER BY CASE WHEN @SortColumn = 'Id' AND @SortDirection = 'ASC'
					THEN [c].[Id] END ASC,
				 CASE WHEN @SortColumn = 'Id' AND @SortDirection = 'DESC'
					THEN [c].[Id] END DESC,
				 CASE WHEN @SortColumn = 'Created' AND @SortDirection = 'ASC'
					THEN [c].[Created] END ASC,
				 CASE WHEN @SortColumn = 'Created' AND @SortDirection = 'DESC'
					THEN [c].[Created] END DESC,
				 CASE WHEN @SortColumn = 'Owner' AND @SortDirection = 'ASC'
					THEN [c].[Owner] END ASC,
				 CASE WHEN @SortColumn = 'Owner' AND @SortDirection = 'DESC'
					THEN [c].[Owner] END DESC,
				 CASE WHEN @SortColumn = 'Type' AND @SortDirection = 'ASC'
					THEN [c].[Type] END ASC,
				 CASE WHEN @SortColumn = 'Type' AND @SortDirection = 'DESC'
					THEN [c].[Type] END DESC,
				 CASE WHEN @SortColumn = 'Pharmacy' AND @SortDirection = 'ASC'
					THEN [c].[Pharmacy] END ASC,
				 CASE WHEN @SortColumn = 'Pharmacy' AND @SortDirection = 'DESC'
					THEN [c].[Pharmacy] END DESC,
				 CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'ASC'
					THEN [c].[RxNumber] END ASC,
				 CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'DESC'
					THEN [c].[RxNumber] END DESC,
				 CASE WHEN @SortColumn = 'Resolved' AND @SortDirection = 'ASC'
					THEN [c].[Resolved] END ASC,
				 CASE WHEN @SortColumn = 'Resolved' AND @SortDirection = 'DESC'
					THEN [c].[Resolved] END DESC,
				 CASE WHEN @SortColumn = 'NoteCount' AND @SortDirection = 'ASC'
					THEN [c].[NoteCount] END ASC,
				 CASE WHEN @SortColumn = 'NoteCount' AND @SortDirection = 'DESC'
					THEN [c].[NoteCount] END DESC,
				CASE WHEN @SortColumn = 'HasTree' AND @SortDirection = 'ASC'
					THEN [c].[HasTree] END ASC,
				 CASE WHEN @SortColumn = 'HasTree' AND @SortDirection = 'DESC'
					THEN [c].[HasTree] END DESC;
			
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

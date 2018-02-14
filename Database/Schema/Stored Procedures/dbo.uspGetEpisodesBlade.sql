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
					DECLARE @UserID NVARCHAR(128)
					SELECT TOP (1) @UserID = ID FROM dbo.AspNetUsers
					EXECUTE [dbo].[uspGetEpisodesBlade] 775, 'Type', 'desc', @UserID
 =============================================
*/
CREATE PROC [dbo].[uspGetEpisodesBlade]
(
	@ClaimID INTEGER,
	@SortColumn VARCHAR(50),
	@SortDirection VARCHAR(5),
	@UserID NVARCHAR(128)
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
			CREATE TABLE #Episodes
			(
				[Id] [int] NOT NULL PRIMARY KEY,
				[Created] [date] NULL,
				[Owner] [nvarchar](201) NULL,
				[Type] [varchar](255) NOT NULL,
				[Role] [varchar](25) NULL,
				[Pharmacy] [varchar](60) NULL,
				[RxNumber] [varchar](100) NULL,
				[Category] [varchar](255) NOT NULL,
				[Resolved] [bit] NULL,
				[NoteCount] [int] NULL
			)
			INSERT INTO [#Episodes]
			(   [Id]
			  , [Created]
			  , [Owner]
			  , [Type]
			  , [Role]
			  , [Pharmacy]
			  , [RxNumber]
			  , [Category]
			  , [Resolved]
			  , [NoteCount])
			SELECT  e.[Id]
					, e.[Created]
					, e.[Owner]
					, e.[Type]
					, e.[Role]
					, e.[Pharmacy]
					, e.[RxNumber]
					, e.[Category]
					, e.[Resolved]
					, e.[NoteCount]
			FROM    dbo.vwEpisode AS e
					INNER JOIN [dbo].[EpisodeTypeUsersMapping] AS [m] ON [m].[EpisodeTypeID] = e.[EpisodeTypeID]
			WHERE	[ClaimID] = @ClaimID
					AND m.[UserID] = @UserID
			
			SELECT [c].[Id]
                 , [c].[Created]
                 , [c].[Owner]
                 , [c].[Type]
                 , [c].[Role]
                 , [c].[Pharmacy]
                 , [c].[RxNumber]
                 , [c].[Category]
                 , [c].[Resolved]
                 , [c].[NoteCount]
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
				 CASE WHEN @SortColumn = 'Role' AND @SortDirection = 'ASC'
					THEN [c].[Role] END ASC,
				 CASE WHEN @SortColumn = 'Role' AND @SortDirection = 'DESC'
					THEN [c].[Role] END DESC,
				 CASE WHEN @SortColumn = 'Pharmacy' AND @SortDirection = 'ASC'
					THEN [c].[Pharmacy] END ASC,
				 CASE WHEN @SortColumn = 'Pharmacy' AND @SortDirection = 'DESC'
					THEN [c].[Pharmacy] END DESC,
				 CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'ASC'
					THEN [c].[RxNumber] END ASC,
				 CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'DESC'
					THEN [c].[RxNumber] END DESC,
				 CASE WHEN @SortColumn = 'Category' AND @SortDirection = 'ASC'
					THEN [c].[Category] END ASC,
				 CASE WHEN @SortColumn = 'Category' AND @SortDirection = 'DESC'
					THEN [c].[Category] END DESC,
				 CASE WHEN @SortColumn = 'Resolved' AND @SortDirection = 'ASC'
					THEN [c].[Resolved] END ASC,
				 CASE WHEN @SortColumn = 'Resolved' AND @SortDirection = 'DESC'
					THEN [c].[Resolved] END DESC,
				 CASE WHEN @SortColumn = 'NoteCount' AND @SortDirection = 'ASC'
					THEN [c].[NoteCount] END ASC,
				 CASE WHEN @SortColumn = 'NoteCount' AND @SortDirection = 'DESC'
					THEN [c].[NoteCount] END DESC;
			
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

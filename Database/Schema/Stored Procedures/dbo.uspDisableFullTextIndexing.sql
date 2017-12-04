SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[uspDisableFullTextIndexing]
WITH EXECUTE AS CALLER, RECOMPILE
AS
BEGIN
	DECLARE @SQL NVARCHAR(1000)
	-- Drop Indexes
	SET @SQL = N'IF EXISTS (SELECT 1 FROM sys.fulltext_indexes WHERE object_id = object_id(''[Claim]''))
		DROP FULLTEXT INDEX ON [dbo].[Claim]'

	EXEC [sys].[sp_executesql] @SQL

	SET @SQL = N'IF EXISTS (SELECT 1 FROM sys.fulltext_indexes WHERE object_id = object_id(''[Patient]''))
		DROP FULLTEXT INDEX ON [dbo].[Patient]'
	
	EXEC [sys].[sp_executesql] @SQL
	
	-- Drop Catalog
	SET @SQL = N'IF EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE [name] = ''uftcatFullTextCatalog'')
		DROP FULLTEXT CATALOG [uftcatFullTextCatalog]'

	EXEC [sys].[sp_executesql] @SQL
END
GO

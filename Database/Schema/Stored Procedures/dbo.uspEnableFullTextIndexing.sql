SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[uspEnableFullTextIndexing]
WITH EXECUTE AS CALLER, RECOMPILE
AS
BEGIN
	--create catalog
	EXEC('
	IF NOT EXISTS (SELECT * FROM sys.fulltext_catalogs WHERE [name] = ''uftcatFullTextCatalog'')
		CREATE FULLTEXT CATALOG [uftcatFullTextCatalog] AS DEFAULT')
	
	--create indexes
	DECLARE @SQL NVARCHAR(1000)
	SET @SQL = 
	N'IF NOT EXISTS (SELECT * FROM sys.fulltext_indexes WHERE object_id = object_id(''[Claim]''))
		CREATE FULLTEXT INDEX ON [Claim]( [ClaimNumber])
		KEY INDEX [' + util.[udfGetPrimaryKeyIndexName]('dbo.Claim') +  '] ON [uftcatFullTextCatalog] WITH CHANGE_TRACKING AUTO'
	EXEC sys.[sp_executesql] @SQL
	SET @SQL = 
	N'IF NOT EXISTS (SELECT * FROM sys.fulltext_indexes WHERE object_id = object_id(''Patient''))
		CREATE FULLTEXT INDEX ON [dbo].[Patient]( [LastName], [FirstName])
		KEY INDEX [' + util.[udfGetPrimaryKeyIndexName](N'dbo.Patient') +  N'] ON [uftcatFullTextCatalog] WITH CHANGE_TRACKING AUTO'
	EXEC sys.[sp_executesql] @SQL
END
GO

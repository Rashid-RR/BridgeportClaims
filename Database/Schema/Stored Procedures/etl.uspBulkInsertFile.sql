SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/9/2017
	Description:	
	Sample Execute:
					EXEC etl.uspBulkInsertFile 'D:\TFS\JGPortal\BridgeportClaimsLakerExtractFiles\FileBytes_20170711_1418.csv'
*/
CREATE PROC [etl].[uspBulkInsertFile] @FullPathAndFileName VARCHAR(1000)
AS BEGIN
	SET NOCOUNT ON;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;
	SET DEADLOCK_PRIORITY HIGH;
	EXEC [util].[uspSmarterTruncateTable] 'etl.BulkInsertLakerFile';
	EXEC [util].[uspSmarterTruncateTable] 'etl.StagedLakerFile';
	EXEC [etl].[uspCleanupStagedLakerFileAddedColumns];
	BEGIN TRY
		BEGIN TRANSACTION;
			DECLARE @SQLStatement NVARCHAR(4000), @LineEnd CHAR(1) = CHAR(10) + CHAR(13);
			SET @SQLStatement = 'BULK INSERT [etl].[BulkInsertLakerFile] ' + @LineEnd +
			N'FROM ''' + @FullPathAndFileName + '''' + @LineEnd +
			N'WITH (   BATCHSIZE = 1000 ' + @LineEnd +
				   N', FIRE_TRIGGERS ' + @LineEnd +
				   N', KEEPIDENTITY ' + @LineEnd +
				   N', KEEPNULLS ' + @LineEnd +
				   N', MAXERRORS = 1 ' + @LineEnd +
				   N', TABLOCK ' + @LineEnd +
				   N', FIELDTERMINATOR = ''\t'' ' + @LineEnd +
				   N', ROWTERMINATOR = ''' + CHAR(10) + '''' + @LineEnd +
				 N')'
			EXEC [sys].[sp_executesql] @SQLStatement;
			INSERT INTO	[etl].[StagedLakerFile] ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31],[32],[33],[34],[35],[36],[37],[38],[39],[40],[41],[42],[43],[44],[45],[46],[47],[48],[49],[50],[51],[52],[53],[54],[55],[56],[57],[58],[59],[60],[61],[62],[63],[64],[65],[66],[67],[68],[69],[70],[71],[72],[73],[74],[75],[76],[77],[78],[79],[80],[81],[82],[83],[84],[85],[86],[87],[88],[89],[90],[91],[92],[93],[94],[95],[96],[97],[98],[99],[100],[101],[102],[103],[104],[105],[106],[107],[108],[109],[110],[111],[112],[113],[114],[115],[116],[117],[118],[119],[120],[121],[122],[123],[124],[125],[126],[127],[128],[129],[130],[131],[132],[133],[134],[135],[136],[137],[138],[139],[140],[141],[142],[143],[144],[145],[146],[147],[148],[149],[150],[151],[152],[153],[154])
			SELECT	[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31],[32],[33],[34],[35],[36],[37],[38],[39],[40],[41],[42],[43],[44],[45],[46],[47],[48],[49],[50],[51],[52],[53],[54],[55],[56],[57],[58],[59],[60],[61],[62],[63],[64],[65],[66],[67],[68],[69],[70],[71],[72],[73],[74],[75],[76],[77],[78],[79],[80],[81],[82],[83],[84],[85],[86],[87],[88],[89],[90],[91],[92],[93],[94],[95],[96],[97],[98],[99],[100],[101],[102],[103],[104],[105],[106],[107],[108],[109],[110],[111],[112],[113],[114],[115],[116],[117],[118],[119],[120],[121],[122],[123],[124],[125],[126],[127],[128],[129],[130],[131],[132],[133],[134],[135],[136],[137],[138],[139],[140],[141],[142],[143],[144],[145],[146],[147],[148],[149],[150],[151],[152],[153],[154] 
			FROM	[etl].[BulkInsertLakerFile]
			EXEC [util].[uspSmarterTruncateTable] 'etl.BulkInsertLakerFile'
			-- Finally, prepare the StagedLakerFile table with the ETL columns that it will need.
			EXEC [etl].[uspAddStagedLakerFileETLColumns];
		IF (@@TRANCOUNT > 0)
			COMMIT TRANSACTION;
	END TRY
    BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			ROLLBACK;
		THROW;
	END CATCH
END
GO

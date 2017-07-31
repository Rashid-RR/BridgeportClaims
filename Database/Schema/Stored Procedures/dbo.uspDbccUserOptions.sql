SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
    Author:         Jordan Gurney
    Date Created:   5/23/2017
    Description:    Executes DBCC USEROPTIONS
    Purpose:        Needed to retrieve the TRANSACTION ISOLATION LEVEL to verify 
                    that READ COMMITTED translates to READ_COMMITTED_SNAPSHOT
    Example Execute:
                    EXEC dbo.uspDbccUserOptions
*/
CREATE PROC [dbo].[uspDbccUserOptions]
AS BEGIN
    CREATE TABLE #Results (SetOption VARCHAR(500) NOT NULL, [Value] VARCHAR(500) NOT NULL)
    INSERT #Results ( SetOption, [Value] )
    EXEC sys.sp_executesql N'DBCC USEROPTIONS'
    SELECT r.SetOption
          ,r.[Value] 
    FROM #Results r
END

GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/10/2017
	Description:	NLog Proc to Log Application Logs
	Sample Execute:
					EXEC [dbo].[uspNLogInsert]
*/
CREATE PROCEDURE [dbo].[uspNLogInsert] (
        @machineName NVARCHAR(200)
      , @siteName NVARCHAR(200)
      , @logged DATETIME2
      , @level VARCHAR(5)
      , @userName NVARCHAR(200)
      , @message NVARCHAR(MAX)
      , @logger NVARCHAR(300)
      , @properties NVARCHAR(MAX)
      , @serverName NVARCHAR(200)
      , @port NVARCHAR(100)
      , @url NVARCHAR(2000)
      , @https BIT
      , @serverAddress NVARCHAR(100)
      , @remoteAddress NVARCHAR(100)
      , @callSite NVARCHAR(300)
      , @exception NVARCHAR(MAX)
) AS
BEGIN
    SET NOCOUNT ON;
    INSERT  [util].[NLog] 
           ( [MachineName]
            , [SiteName]
            , [Logged]
            , [Level]
            , [UserName]
            , [Message]
            , [Logger]
            , [Properties]
            , [ServerName]
            , [Port]
            , [Url]
            , [Https]
            , [ServerAddress]
            , [RemoteAddress]
            , [Callsite]
            , [Exception]
           )
    VALUES ( @machineName
           , @siteName
           , @logged
           , @level
           , @userName
           , @message
           , @logger
           , @properties
           , @serverName
           , @port
           , @url
           , @https
           , @serverAddress
           , @remoteAddress
           , @callSite
           , @exception
           )
END
GO

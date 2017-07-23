SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/22/2017
	Description:	Simulates the built-in, SQL Server GETDATE() or (more accurately SYSDATETIME() - used
					GETDATE() for brevity), but returns the current system datetime in Mountain Standard Time 
					instead of UTC (Azure Default).
	Sample Execute:
					SELECT util.udfGetDate()
*/
CREATE FUNCTION [dtme].[udfGetDate]()
RETURNS DATETIME2
AS BEGIN
RETURN
(
	SELECT [dtme].[udfGetLocalDateTime](SYSUTCDATETIME())
)
END
GO

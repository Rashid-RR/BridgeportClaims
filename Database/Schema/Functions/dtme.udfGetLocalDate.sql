SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	10/01/2017
	Description:	Simulates the built-in, SQL Server GETDATE() or (more accurately SYSDATETIME()
					(used dtme.GetLocalDate() for brevity), but returns the current system datetime
					in Mountain Standard Time or Mountain Daylight Time instead of UTC (Azure Default).
	Sample Execute:
					SELECT dtme.udfGetLocalDate()
*/
CREATE FUNCTION [dtme].[udfGetLocalDate]()
RETURNS DATETIME2
WITH SCHEMABINDING
AS
BEGIN
	-- Seems arbitrary, but can be used for testing.
	RETURN (dtme.udfGetLocalDateTime(SYSUTCDATETIME()))
END 
GO

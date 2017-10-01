SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	10/01/2017
	Description:	Simulates the built-in, SQL Server SYSUTCDATETIME(),
					but can be spoofed for testing.
	Sample Execute:
					SELECT dtme.udfGetUtcDate()
*/
CREATE FUNCTION [dtme].[udfGetUtcDate]()
RETURNS DATETIME2
WITH SCHEMABINDING
AS
BEGIN
	-- Seems arbitrary, but can be used for testing.
	RETURN SYSUTCDATETIME()
END 
GO

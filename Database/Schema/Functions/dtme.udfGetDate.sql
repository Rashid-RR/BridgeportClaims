SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dtme].[udfGetDate]()
RETURNS DATETIME2
AS BEGIN
    -- Arbitrary wrapper for common Azure datetime function for "NOW" but in UTC. Can be changed if necessary.
    RETURN SYSUTCDATETIME()
END
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/12/2017
	Description:	Trims leading zeros from a string (needed for ETL)
	Sample Execute:
					SELECT util.udfTrimLeadingZeros('010084700')
*/
CREATE FUNCTION [util].[udfTrimLeadingZeros](@Input VARCHAR(4000))
RETURNS VARCHAR(4000)
AS BEGIN RETURN
(
	SELECT SUBSTRING(@Input, PATINDEX('%[^0]%', @Input + '.'), LEN(@Input))
)
END
GO

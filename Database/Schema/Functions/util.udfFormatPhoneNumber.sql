SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [util].[udfFormatPhoneNumber](@PhoneNo VARCHAR(30))
RETURNS VARCHAR(25)
AS
BEGIN
DECLARE @Formatted VARCHAR(25)

IF (LEN(@PhoneNo) <> 10)
    SET @Formatted = @PhoneNo
ELSE
    SET @Formatted = '(' + LEFT(@PhoneNo, 3) + ') ' + SUBSTRING(@PhoneNo, 4, 3) + '-' + SUBSTRING(@PhoneNo, 7, 4)

RETURN @Formatted
END
GO

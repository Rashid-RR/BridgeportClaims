SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[udfGetNumeric] (@AlphaNumeric VARCHAR(256))
RETURNS VARCHAR(256)
AS
    BEGIN
        DECLARE @intAlpha INT
        SET @intAlpha = PATINDEX('%[^0-9]%', @AlphaNumeric)
        BEGIN
            WHILE @intAlpha > 0
                BEGIN
                    SET @AlphaNumeric = STUFF(@AlphaNumeric, @intAlpha, 1, '')
                    SET @intAlpha = PATINDEX('%[^0-9]%', @AlphaNumeric)
                END
        END
        RETURN ISNULL(@AlphaNumeric, 0)
    END
GO

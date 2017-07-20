SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [util].[uspGenerateRandomString] @sLength TINYINT = 10
AS
    BEGIN
        SET NOCOUNT ON
        DECLARE @counter TINYINT
          , @nextChar CHAR(1)
          , @randomString VARCHAR(50)
        
        SET @counter = 1
        SET @randomString = ''

        WHILE @counter <= @sLength
            BEGIN
                SELECT  @nextChar = CHAR(48 + CONVERT(INT, ( 122 - 48 + 1 )
                                         * RAND()))

                IF ASCII(@nextChar) NOT IN ( 58, 59, 60, 61, 62, 63, 64, 91,
                                             92, 93, 94, 95, 96 )
                    BEGIN
                        SELECT  @randomString = @randomString + @nextChar
                        SET @counter = @counter + 1
                    END
            END
        SELECT  @randomString
    END

GO

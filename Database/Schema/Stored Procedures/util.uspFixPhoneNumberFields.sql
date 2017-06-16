SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Date:			5/25/2017
	Description:	Finds all tables with the phrase "Phone" somewhere in the column name,
					updates the data type to a VARCHAR(30) if the length is less than 30,
					then formats phone number that are 10 digits long (ignoring other lengths)
					to (xxx) xxx-xxxx.
	Example Execute:
					EXEC util.uspFixPhoneNumberFields
*/
CREATE PROC [util].[uspFixPhoneNumberFields]
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @SQLStatement NVARCHAR(4000)
	DECLARE PhoneNumberCursor CURSOR LOCAL FAST_FORWARD FOR
		SELECT N'ALTER TABLE ' + QUOTENAME(c.TABLE_SCHEMA) + N'.' + QUOTENAME(c.TABLE_NAME) +
				N' ALTER COLUMN ' + QUOTENAME(c.COLUMN_NAME) + N' VARCHAR(30) ' + IIF(c.IS_NULLABLE = N'NO', N'NOT NULL', N'NULL')
		FROM   INFORMATION_SCHEMA.COLUMNS AS c
			   INNER JOIN INFORMATION_SCHEMA.TABLES AS t ON c.TABLE_NAME = t.TABLE_NAME
		WHERE  c.COLUMN_NAME LIKE '%Phone%' AND t.TABLE_TYPE = 'BASE TABLE'
			   AND c.CHARACTER_MAXIMUM_LENGTH < 30
			   AND c.CHARACTER_MAXIMUM_LENGTH > 0 -- Doesn't touch MAX

	OPEN PhoneNumberCursor
	FETCH NEXT FROM PhoneNumberCursor INTO @SQLStatement
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRY
				BEGIN TRAN
					EXEC [sys].[sp_executesql] @SQLStatement
					IF @@TRANCOUNT > 0
						COMMIT
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK;
				THROW;
				BREAK
			END CATCH

			FETCH NEXT FROM PhoneNumberCursor INTO @SQLStatement
		END
	CLOSE PhoneNumberCursor
	DEALLOCATE PhoneNumberCursor

	DECLARE PhoneNumberFormatter CURSOR LOCAL FAST_FORWARD FOR
		SELECT 'UPDATE ' + QUOTENAME(c.TABLE_SCHEMA) + '.' + QUOTENAME(c.TABLE_NAME) + ' SET '
			 + QUOTENAME(c.COLUMN_NAME) + ' = [util].[udfFormatPhoneNumber](' + QUOTENAME(c.COLUMN_NAME) + ')' +
			 ' WHERE ' + QUOTENAME(c.COLUMN_NAME) + ' IS NOT NULL'
		FROM   INFORMATION_SCHEMA.COLUMNS AS c
			   INNER JOIN INFORMATION_SCHEMA.TABLES AS t ON c.TABLE_NAME = t.TABLE_NAME
		WHERE  c.COLUMN_NAME LIKE '%Phone%' AND t.TABLE_TYPE = 'BASE TABLE'

	OPEN PhoneNumberFormatter
	FETCH NEXT FROM PhoneNumberFormatter INTO @SQLStatement
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRY
				BEGIN TRAN
					EXEC [sys].[sp_executesql] @SQLStatement
					IF @@TRANCOUNT > 0
						COMMIT
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK;
				THROW;
				BREAK
			END CATCH
			FETCH NEXT FROM PhoneNumberFormatter INTO @SQLStatement
		END
	CLOSE PhoneNumberFormatter
	DEALLOCATE PhoneNumberFormatter
END
GO

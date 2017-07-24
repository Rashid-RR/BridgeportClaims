DECLARE   @DB VARCHAR(30) = 'BridgeportClaims'
		, @PrntMsg NVARCHAR(1000)
		, @ThreeDaysAgo VARCHAR(30) = CONVERT(VARCHAR, DATEADD(DAY, -3, GETDATE()), 112)
		, @TwoDaysAgo VARCHAR(30) = CONVERT(VARCHAR, DATEADD(DAY, -2, GETDATE()), 112)
		, @Today VARCHAR(30) = CONVERT(VARCHAR, GETDATE(), 112)
		, @Tomorrow VARCHAR(30) = CONVERT(VARCHAR, DATEADD(DAY, 1, GETDATE()), 112)
		, @ConcatDBName SYSNAME
		, @Command NVARCHAR(4000)

-- Check today and tomorrow, if wrong, uncomment below to fix
-- SELECT @Today, @Tomorrow
SELECT @Today = CONVERT(VARCHAR,DATEADD(DAY, -1, @Today), 112), @Tomorrow = CONVERT(VARCHAR,DATEADD(DAY, -1, @Tomorrow), 112)

IF @DB IS NULL OR @ThreeDaysAgo IS NULL OR @TwoDaysAgo IS NULL OR @Today IS NULL OR @Tomorrow IS NULL
	BEGIN
		RAISERROR(N'Error. one or more scalar variables was not properly populated', 16, 1) WITH NOWAIT
		RETURN
	END

IF EXISTS (SELECT * FROM [sys].[databases] AS [d] WHERE [d].[name] = @DB + @ThreeDaysAgo)
	BEGIN
		SET @ConcatDBName = @DB + @ThreeDaysAgo
		SET @PrntMsg = 'Ok, dropping the database ' + @ConcatDBName + '...'
		RAISERROR(@PrntMsg, 10, 1) WITH NOWAIT
		SET @Command = N'DROP DATABASE ' + @ConcatDBName
		EXEC [sys].[sp_executesql] @Command
	END
IF EXISTS (SELECT * FROM [sys].[databases] AS [d] WHERE [d].[name] = @DB + @TwoDaysAgo)
	BEGIN
		SET @ConcatDBName = @DB + @TwoDaysAgo
		SET @PrntMsg = 'Ok, dropping the database ' + @ConcatDBName + '...'
		RAISERROR(@PrntMsg, 10, 1) WITH NOWAIT
		SET @Command = N'DROP DATABASE ' + @ConcatDBName
		EXEC [sys].[sp_executesql] @Command
	END
IF EXISTS (SELECT * FROM [sys].[databases] AS [d] WHERE [d].[name] = @DB + @Today)
	BEGIN
		SET @ConcatDBName = @DB + @Today
		SET @PrntMsg = 'Ok, dropping the database ' + @ConcatDBName + '...'
		RAISERROR(@PrntMsg, 10, 1) WITH NOWAIT
		SET @Command = N'DROP DATABASE ' + @ConcatDBName
		EXEC [sys].[sp_executesql] @Command
	END
-- Kill tomorrow's in case we have a stragler.
IF EXISTS (SELECT * FROM [sys].[databases] AS [d] WHERE [d].[name] = @DB + @Tomorrow)
	BEGIN
		SET @ConcatDBName = @DB + @Tomorrow
		SET @PrntMsg = 'Ok, dropping the database ' + @ConcatDBName + '...'
		RAISERROR(@PrntMsg, 10, 1) WITH NOWAIT
		SET @Command = N'DROP DATABASE ' + @ConcatDBName
		EXEC [sys].[sp_executesql] @Command
	END
SET @ConcatDBName = @DB + @Today
SET @PrntMsg = 'Ok now, creating the database ' + @ConcatDBName + '....'
RAISERROR(@PrntMsg, 10, 1) WITH NOWAIT
SET @Command = N'CREATE DATABASE ' + @ConcatDBName + ' AS COPY OF ' + @DB
EXEC [sys].[sp_executesql] @Command



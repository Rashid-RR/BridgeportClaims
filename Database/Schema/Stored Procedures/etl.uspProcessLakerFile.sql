SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/24/2017
	Description:	Main ETL Proc
	Sample Execute:
					EXEC etl.uspProcessLakerFile
*/
CREATE PROC [etl].[uspProcessLakerFile]
WITH RECOMPILE -- Not for performance, but out of
			   -- necessity for compilation and avoiding parser errors.
AS BEGIN
	SET NOCOUNT ON;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;
	SET DEADLOCK_PRIORITY HIGH;
	-- Testing
	-- Preparatory work
	-- Rename row 1 to RowID
	/*EXEC(N'EXEC sys.sp_rename ''[etl].[StagedLakerFile].[1]'',''RowID'',''column''')
	-- Reduce Character limit on our RowID from 8000 to 50
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN RowID VARCHAR(50) NOT NULL')
	-- Add Primary Key to our Unique Row ID for our Staging Table
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] WITH CHECK ADD CONSTRAINT pkStagedLakerFile
	PRIMARY KEY CLUSTERED ([RowID] ASC) WITH (FILLFACTOR=100,DATA_COMPRESSION=ROW)')*/
	-- Import tables should already be prepared, but if not, let's run Proc
	EXEC [etl].[uspAddStagedLakerFileETLColumns]
	IF EXISTS
	(
		SELECT * FROM [sys].[columns] AS [c]
		INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
		INNER JOIN [sys].[schemas] AS [s] ON [s].[schema_id] = [t].[schema_id]
		WHERE [s].[name] = 'etl'
		AND [t].[name] = 'StagedLakerFile'
		AND [c].[name] = 'StageID'
	)
		EXEC(N'ALTER TABLE [etl].[StagedLakerFile] DROP COLUMN StageID')

	IF OBJECT_ID(N'tempdb..#Payor') IS NOT NULL
			DROP TABLE #Payor
	IF OBJECT_ID(N'tempdb..#Adjustor') IS NOT NULL
			DROP TABLE #Adjustor
	IF OBJECT_ID(N'tempdb..#PatientImport') IS NOT NULL
			DROP TABLE #PatientImport
	IF OBJECT_ID(N'tempdb..#ProcessPatient') IS NOT NULL
			DROP TABLE #ProcessPatient
	IF OBJECT_ID(N'tempdb..#ProcessPharmacy') IS NOT NULL
			DROP TABLE #ProcessPharmacy
	IF OBJECT_ID(N'tempdb..#ProcessClaim') IS NOT NULL
			DROP TABLE #ProcessClaim
	IF OBJECT_ID(N'tempdb..#ClaimImport') IS NOT NULL
			DROP TABLE #ClaimImport
	IF OBJECT_ID(N'tempdb..#InvoiceImport') IS NOT NULL
			DROP TABLE #InvoiceImport
	IF OBJECT_ID(N'tempdb..#ProcessInvoice') IS NOT NULL
			DROP TABLE #ProcessInvoice
	IF OBJECT_ID(N'tempdb..#PharmacyImport') IS NOT NULL
			DROP TABLE #PharmacyImport
	IF OBJECT_ID(N'tempdb..#UpdatePatient') IS NOT NULL
			DROP TABLE #UpdatePatient
	IF OBJECT_ID(N'tempdb..#UpdateClaim') IS NOT NULL
			DROP TABLE #UpdateClaim
	IF OBJECT_ID(N'tempdb..#TransientPatient') IS NOT NULL
			DROP TABLE #TransientPatient
	IF OBJECT_ID(N'tempdb..#UpdateInvoice') IS NOT NULL
			DROP TABLE #UpdateInvoice

	DECLARE @TotalRowCount INT = (SELECT COUNT(*) FROM [etl].[StagedLakerFile])
	-- Clear out tables that we're going to be loading
	IF EXISTS (SELECT * FROM [sys].[views] AS [v] WHERE [v].[name] = 'vwPrescriptionNote')
		DROP VIEW [dbo].[vwPrescriptionNote]
	EXEC [util].[uspSmarterTruncateTable] 'dbo.ClaimNote'
	EXEC [util].[uspSmarterTruncateTable] 'dbo.PrescriptionNoteMapping'
	EXEC [util].[uspSmarterTruncateTable] 'dbo.PrescriptionNote'
	EXEC [util].[uspSmarterTruncateTable] 'dbo.Prescription'
	EXEC [util].[uspSmarterTruncateTable] 'dbo.AcctPayable'
	EXEC [util].[uspSmarterTruncateTable] 'dbo.Invoice'
	EXEC [util].[uspSmarterTruncateTable] 'dbo.Pharmacy'
	EXEC [util].[uspSmarterTruncateTable] 'dbo.Episode'
	EXEC [util].[uspSmarterTruncateTable] 'dbo.ClaimsUserHistory'
	EXEC [util].[uspSmarterTruncateTable] 'dbo.Claim'
	EXEC [util].[uspSmarterTruncateTable] 'dbo.Patient'
	EXEC [util].[uspSmarterTruncateTable] 'dbo.Adjustor'
	EXEC [util].[uspSmarterTruncateTable] 'dbo.Payor'
				
	DECLARE @RowCountCheck INT
	IF NOT EXISTS
	(
		SELECT	* 
		FROM	[etl].[StagedLakerFile]
	)
	BEGIN
		IF @@TRANCOUNT > 0
			ROLLBACK;
		RAISERROR(N'Nothing to Process. The etl.StagedLakerFile table is empty', 1, 1) WITH NOWAIT;
		RETURN;
	END
	/********************************************************************************************
	Import Payor Section
	********************************************************************************************/
	CREATE TABLE #Payor ([42] [varchar](8000) NULL,[43] [varchar](8000) NULL,[44] [varchar](8000) NULL,[45] 
		[varchar](8000) NULL,[46] [varchar](8000) NULL,[StateID] [int] NULL,[48] [varchar](8000) NULL,[49] [varchar](8000) NULL,[50]
		[varchar](8000) NULL, RowNumber INT NOT NULL, DenseRank INT NOT NULL, ETLRowID VARCHAR(50) NOT NULL)
	INSERT [#Payor] ([42],[43],[44],[45],[46],[StateID],[48],[49],[50],[RowNumber],[DenseRank],[ETLRowID])
	SELECT [s].[42], [s].[43],[s].[44],[s].[45],[s].[46],[us].[StateID],s.[48],s.[49],s.[50]
			,ROW_NUMBER() OVER (PARTITION BY [s].[42], [s].[43],[s].[44],[s].[45],[s].[46],[us].[StateID],s.[48],s.[49],s.[50] ORDER BY [s].[42] ASC) RowNumber
			,DENSE_RANK() OVER (ORDER BY [s].[42], [s].[43],[s].[44],[s].[45],[s].[46],[us].[StateID],s.[48],s.[49],s.[50] ASC) DenseRank, [s].[RowID]
	FROM   [etl].[StagedLakerFile] AS s
			LEFT JOIN [dbo].[UsState] AS [us] ON [us].[StateCode] = [s].[47]
	WHERE  [s].[47] IS NOT NULL
	
	-- Actual Payor Import
	INSERT [dbo].[Payor] ([GroupName],[BillToName],[BillToAddress1],[BillToAddress2],[BillToCity],[BillToStateID],[BillToPostalCode],[PhoneNumber],[FaxNumber],[ETLRowID])
	SELECT [42],[43],[44],[45],[46],[StateID],[48],[49],[50],[ETLRowID] 
	FROM [#Payor] AS [p]
	WHERE [RowNumber] = 1
	ORDER BY [42] ASC
	SET @RowCountCheck = @@ROWCOUNT;
			
	IF (SELECT COUNT(*) FROM [dbo].[Payor] AS [p]) != @RowCountCheck
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'The Payor count QA check failed', 16, 1) WITH NOWAIT
			RETURN
		END

	UPDATE s
	SET    [s].[PayorID] = [pi].[PayorID]
	FROM   [etl].[StagedLakerFile] AS s WITH (TABLOCKX)
			INNER JOIN dbo.Payor AS [pi] ON [s].[42] = [pi].[GroupName]; -- This one's easier than most beacause Group Name is Unique
			
	-- Another Payor QA Check
	IF @RowCountCheck != (SELECT COUNT(DISTINCT [PayorID]) FROM etl.[StagedLakerFile])
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK;
			RAISERROR(N'Error. The row count for the Payor Insert does not match the corresponding Row Count for distinct PayorID''s in the Staging table', 16, 1) WITH NOWAIT;
			RETURN;
		END

	-- Another Payor QA check. This ensures that there is a Payor record on every line.
	EXEC [sys].[sp_executesql] N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN PayorID INT NOT NULL';
	-- Ok, if that worked, change it back.
	EXEC [sys].[sp_executesql] N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN PayorID INT NULL';
	/********************************************************************************************
	End Payor Section
	********************************************************************************************/

	/********************************************************************************************
	Import Adjustor Section 
	Notes: It is impossible to distinctly identify an Adjustor unless they all have distinct AdjustorName's.
			In this data set, I found 432 unique, Adjustor names, but when I imported all of them, with their corresponding
			PayorID's, this results in a total of 457 rows being imported. Because I found multiple Payor ID's for a single
			Adjustor name more than once.
	Query to identify problem:
			WITH AdjustorCTE
			AS ( SELECT   [s].[PayorID]
						, [s].[21]
						, RowID = ROW_NUMBER() OVER ( PARTITION BY [s].[21]
														ORDER BY [s].[21]
																, [s].[PayorID]
													)
					FROM     [etl].[StagedLakerFile] AS s
					WHERE    [s].[21] IS NOT NULL
					GROUP BY [s].[PayorID]
						, [s].[21]
				)
			SELECT [a].[21], MAX(a.[RowID]) TotalPayorIDs
			FROM   [AdjustorCTE] AS a
			WHERE  [a].[RowID] > 1
			GROUP BY [a].[21]

			RETURN

			SELECT Payor = [p].[GroupName]
					, a.*
			FROM   [dbo].[Adjustor] AS [a]
					INNER JOIN [dbo].[Payor] AS [p] ON [p].[PayorID] = [a].[PayorID]
			WHERE  [a].[AdjustorName] = 'NATASHA IRIZARRY'
	********************************************************************************************/
	CREATE TABLE #Adjustor (PayorID INT NOT NULL, AdjustorName VARCHAR(255) NOT NULL, RowNumber INT NOT NULL, 
							DenseRank INT NOT NULL, ETLRowID VARCHAR(50) NOT NULL)
	INSERT [#Adjustor] ([PayorID],[AdjustorName],[RowNumber],[DenseRank],[ETLRowID])
	SELECT s.[PayorID],s.[21],ROW_NUMBER() OVER (PARTITION BY s.[PayorID], s.[21] ORDER BY s.[21] ASC) RowNumber,
	DENSE_RANK() OVER (ORDER BY s.[PayorID], s.[21] ASC) DenseRank, [s].[RowID]
	FROM etl.[StagedLakerFile] AS s 
	WHERE s.[21] IS NOT NULL 
	ORDER BY s.[21]
			
	-- Actual Adjustor Import
	INSERT [dbo].[Adjustor] ([PayorID], [AdjustorName], [ETLRowID])
	SELECT a.[PayorID], a.[AdjustorName], [a].[ETLRowID] FROM [#Adjustor] AS [a]
	WHERE [a].[RowNumber] = 1
	SET @RowCountCheck = @@ROWCOUNT;
				
	UPDATE s SET s.AdjustorID = a.[AdjustorID]
	FROM   [etl].[StagedLakerFile] AS s WITH (TABLOCKX)
			INNER JOIN dbo.Adjustor AS a ON a.AdjustorName = s.[21]
	WHERE  a.PayorID = s.PayorID;
			
	-- Adjustor QA Check
	IF @RowCountCheck != (SELECT COUNT(DISTINCT [s].[AdjustorID]) FROM etl.[StagedLakerFile] AS s)
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK;
			RAISERROR(N'Error. The row count for the Payor Insert does not match the corresponding Row Count for distinct PayorID''s in the Staging table', 16, 1) WITH NOWAIT;
			RETURN;
		END
			
	/********************************************************************************************
	End Adjustor Section
	********************************************************************************************/

	/********************************************************************************************
	Begin Patient Section
	********************************************************************************************/
	CREATE TABLE #PatientImport
	(
		[DateOfBirth] [varchar] (8000) NULL,
		[LastName] [varchar] (8000) NULL,
		[FirstName] [varchar] (8000) NULL,
		[Address1] [varchar] (8000) NULL,
		[Address2] [varchar] (8000) NULL,
		[City] [varchar] (8000) NULL,
		[StateID] [int] NULL,
		[PostalCode] [varchar] (8000) NULL,
		[PhoneNumber] [varchar] (8000) NULL,
		[GenderID] [int] NULL,
		[RowNumber] INT NOT NULL,
		DenseRank INT NOT NULL,
		[ETLRowID] VARCHAR(50) NOT NULL
	)
	INSERT [#PatientImport] ([DateOfBirth],[LastName], [FirstName], [Address1], [Address2], [City], [StateID],[PostalCode]
									, [PhoneNumber],[GenderID], [RowNumber], [DenseRank], [ETLRowID])
	SELECT p.[20], p.[10], p.[11], p.[12],p.[13],p.[14], [us].[StateID],p.[16],p.[17],g.[GenderID]
	,ROW_NUMBER() OVER (PARTITION BY p.[20], p.[10], p.[11], p.[12],p.[13],p.[14], p.[15],p.[16],p.[17],P.[18] ORDER BY p.[20]) RowID
	,DENSE_RANK() OVER (ORDER BY p.[20], p.[10], p.[11], p.[12],p.[13],p.[14], [us].[StateID],p.[16],p.[17],g.[GenderID]) DenseRank, p.[RowID]
	FROM [etl].[StagedLakerFile] AS p
	LEFT JOIN [dbo].[UsState] AS [us] ON p.[15] = [us].[StateCode]
	LEFT JOIN [dbo].[Gender] AS [g] ON g.[GenderCode] = p.[18]
	WHERE p.[10] IS NOT NULL

	-- Patient import statement
	INSERT [dbo].[Patient] ([DateOfBirth],[LastName],[FirstName], [Address1],[Address2],[City],[StateID],
							[PostalCode],[PhoneNumber],[GenderID],[ETLRowID])
	SELECT [p].[DateOfBirth], [p].[LastName], [p].[FirstName], [p].[Address1], [p].[Address2], [p].[City], [p].[StateID]
			, [p].[PostalCode], [p].[PhoneNumber], [p].[GenderID], [p].[ETLRowID] 
	FROM [#PatientImport] AS p WHERE [p].[RowNumber] = 1
	SET @RowCountCheck = @@ROWCOUNT

	CREATE TABLE #ProcessPatient (PatientID INT, DenseRank INT NOT NULL, ETLRowID VARCHAR(50) NOT NULL)
	INSERT [#ProcessPatient] ([PatientID], [DenseRank], ETLRowID)
	SELECT [p].[PatientID]
			, [i].[DenseRank]
			, [s].[RowID]
	FROM   [etl].[StagedLakerFile] AS s
			INNER JOIN [#PatientImport] AS i ON [i].[ETLRowID] = [s].[RowID]
			LEFT JOIN [dbo].[Patient] AS [p] ON [s].[RowID] = [p].[ETLRowID];

	CREATE TABLE #UpdatePatient (ETLRowID VARCHAR(50) NOT NULL, PatientID INT NULL);
	INSERT #UpdatePatient ([ETLRowID],[PatientID])
	SELECT [pp].ETLRowID
			, PatientID = MIN([pp].[PatientID]) OVER ( 
					PARTITION BY [pp].[DenseRank] ORDER BY [pp].[DenseRank]
					ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING )
	FROM   [#ProcessPatient] AS [pp]
			
	IF EXISTS
	(
		SELECT *
		FROM   #UpdatePatient AS [tp]
		WHERE  [tp].[ETLRowID] IS NULL
				OR [tp].[PatientID] IS NULL
	)
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. There are not supposed to be any NULL PatientID''s or ETLRowID''s in the transient table', 16, 1) WITH NOWAIT
			RETURN
		END

	UPDATE s SET s.PatientID = u.PatientID
	FROM   [etl].[StagedLakerFile] s WITH (TABLOCKX)
			INNER JOIN #UpdatePatient u ON [s].[RowID] = u.[ETLRowID]

	-- Patient QA Check
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN PatientID INT NOT NULL')

	-- Change it back
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN PatientID INT NULL')
			
	-- Patient QA Check
	IF @RowCountCheck != (SELECT COUNT(DISTINCT slf.[PatientID]) FROM [etl].[StagedLakerFile] AS [slf])
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK;
			RAISERROR(N'Error. The QA check for the count of rows inserted into Patient, and the distinct count of PatientID''s found in the StagedLakerFile table failed.', 16, 1) WITH NOWAIT;
			RETURN;
		END

	/********************************************************************************************
	Phew... that was a hard one. End Patient Section
	********************************************************************************************/

	/********************************************************************************************
	Begin Claim Section
	Notes: We have an issue with the unique identifier "ClaimNumber + PersonCode".
	SELECT DISTINCT [s].[8],[s].[9]--,[s].[19],[s].[22],[s].[25],[s].[125],s.[PatientID],s.[PayorID],s.[AdjustorID]
	FROM   [etl].[StagedLakerFile] AS s
	WHERE  s.[PatientID] IS NOT NULL
			AND s.[PayorID] IS NOT NULL
	-- Returns 863 records, but when I add the rest of the necessary columns, this duplicates some rows
	SELECT DISTINCT [s].[8],[s].[9],[s].[19],[s].[22],[s].[25],[s].[125],s.[PatientID],s.[PayorID],s.[AdjustorID]
	FROM   [etl].[StagedLakerFile] AS s
	WHERE  s.[PatientID] IS NOT NULL
			AND s.[PayorID] IS NOT NULL
	-- And this now returns 874 Records. Query to find the dupes:
	SELECT   [c].[8]
			, [c].[9]
			, COUNT(*)
	FROM     (   SELECT [s].[8]
						, [s].[9]
						, [s].[19]
						, [s].[22]
						, [s].[25]
						, [s].[125]
						, [s].[PatientID]
						, [s].[PayorID]
						, [s].[AdjustorID]
						, RowID = ROW_NUMBER() OVER ( PARTITION BY [s].[8]
																, [s].[9]
																, [s].[19]
																, [s].[22]
																, [s].[25]
																, [s].[125]
																, [s].[PatientID]
																, [s].[PayorID]
																, [s].[AdjustorID]
													ORDER BY [s].[8]
													)
						, DenseRank = DENSE_RANK() OVER ( ORDER BY [s].[8]
																, [s].[9]
																, [s].[19]
																, [s].[22]
																, [s].[25]
																, [s].[125]
																, [s].[PatientID]
																, [s].[PayorID]
																, [s].[AdjustorID]
														)
						, [s].[StageID]
					FROM   [etl].[StagedLakerFile] AS s
					WHERE  [s].[PatientID] IS NOT NULL
						AND [s].[PayorID] IS NOT NULL
				) AS c
	WHERE    [c].[RowID] = 1
	GROUP BY [c].[8]
			, [c].[9]
	HAVING   COUNT(*) > 1
	********************************************************************************************/
	CREATE TABLE #ClaimImport(
				[8] [varchar](8000) NULL,
				[9] [varchar](8000) NULL,
				[19] [varchar](8000) NULL,
				[22] [varchar](8000) NULL,
				[25] [varchar](8000) NULL,
				[125] [varchar](8000) NULL,
				[PatientID] [int] NULL,
				[PayorID] [int] NOT NULL,
				[AdjustorID] [int] NULL,
				[RowNumber] [int] NULL,
				[DenseRank] [int] NULL,
				[ETLRowID] VARCHAR(50) NOT NULL)
	INSERT INTO [#ClaimImport] ([8],[9],[19],[22],[25],[125],[PatientID],[PayorID],[AdjustorID],[RowNumber],[DenseRank],[ETLRowID])
	SELECT [s].[8],[s].[9],[s].[19],[s].[22],[s].[25],[s].[125],s.[PatientID],s.[PayorID],s.[AdjustorID]
	,ROW_NUMBER() OVER (PARTITION BY [s].[8],[s].[9],[s].[19],[s].[22],[s].[25],[s].[125],s.[PatientID],s.[PayorID],s.[AdjustorID] ORDER BY [s].[8]) RowNumber
	,DENSE_RANK() OVER (ORDER BY [s].[8],[s].[9],[s].[19],[s].[22],[s].[25],[s].[125],s.[PatientID],s.[PayorID],s.[AdjustorID]) DenseRank, s.[RowID]
	FROM   [etl].[StagedLakerFile] AS s
	WHERE  1 = 1
			AND s.[PatientID] IS NOT NULL
			AND s.[PayorID] IS NOT NULL

	-- Claim Import Statement
	INSERT [dbo].[Claim] ([ClaimNumber]
				, [PersonCode]
				, [DateOfInjury]
				, [RelationCode]
				, [PreviousClaimNumber] -- Note, need to check on the mapping of "PolicyNum"
				, [TermDate]
				, [PatientID]
				, [PayorID]
				, [AdjusterID]
				, [ETLRowID]
				, [IsFirstParty] -- Check on this, doesn't allow NULLs and is not in the mapping file.
				)
	SELECT	[ci].[8],[ci].[9],[ci].[19],[ci].[22],[ci].[25],[ci].[125],[ci].[PatientID],[ci].[PayorID],[ci].[AdjustorID],[ci].[ETLRowID], 1
	FROM	[#ClaimImport] AS [ci] 
	WHERE	[ci].[RowNumber] = 1
	SET @RowCountCheck = @@ROWCOUNT
			
	CREATE TABLE #ProcessClaim (ClaimID INT, DenseRank INT NOT NULL, ETLRowID VARCHAR(50) NOT NULL)
	INSERT #ProcessClaim ([ClaimID], [DenseRank], ETLRowID)
	SELECT [c].[ClaimID]
			, [i].[DenseRank]
			, [s].[RowID]
	FROM   [etl].[StagedLakerFile] AS s
			INNER JOIN [#ClaimImport] AS i ON [i].[ETLRowID] = [s].[RowID]
			LEFT JOIN [dbo].[Claim] AS [c] ON [s].[RowID] = [c].[ETLRowID];

	CREATE TABLE #UpdateClaim (ETLRowID VARCHAR(50) NOT NULL, ClaimID INT NOT NULL)
	INSERT #UpdateClaim ([ETLRowID],[ClaimID])
	SELECT [pc].ETLRowID
			, ClaimID = MIN([pc].[ClaimID]) OVER ( 
					PARTITION BY [pc].[DenseRank] ORDER BY [pc].[DenseRank]
					ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING)
	FROM   [#ProcessClaim] AS [pc]
			
	UPDATE [s]
	SET    [s].[ClaimID] = [c].[ClaimID]
	FROM   #UpdateClaim AS c
			INNER JOIN [etl].[StagedLakerFile] AS s WITH (TABLOCKX) ON [s].[RowID] = [c].[ETLRowID];
			
	-- QA Check, make sure that there are NO records with an empty Claim ID
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN [ClaimID] INT NOT NULL')

	-- Ok, set it back.
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN [ClaimID] INT NULL')

	-- Claim QA Check
	IF @RowCountCheck != (SELECT COUNT(DISTINCT slf.[ClaimID]) FROM [etl].[StagedLakerFile] AS [slf])
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK;
			RAISERROR(N'Error. The QA check for the count of rows inserted into Claim, and the distinct count of ClaimID''s found in the StagedLakerFile table failed.', 16, 1) WITH NOWAIT;
			RETURN;
		END
			
	/********************************************************************************************
	End Claim Section
	********************************************************************************************/
	/********************************************************************************************
	Begin Invoice Section
	********************************************************************************************/
	CREATE TABLE #InvoiceImport (ARItemKey VARCHAR(1),Amount MONEY,InvoiceNumber VARCHAR(8000),InvoiceDate VARCHAR(8000),[PayorID] INT NOT NULL
								,[ClaimID] INT NOT NULL, RowNumber INT NOT NULL, DenseRank INT NOT NULL, ETLRowID VARCHAR(50) NOT NULL)
	INSERT INTO [#InvoiceImport] ([ARItemKey],[Amount],[InvoiceNumber],[InvoiceDate],[PayorID],[ClaimID],RowNumber,[DenseRank],[ETLRowID])
	SELECT '' -- Have to check on Mapping of ARIItemKey
		,  0 -- Have to check on Mapping of Amount
		, s.[4],s.[5],s.[PayorID],[s].[ClaimID]
		,ROW_NUMBER() OVER (PARTITION BY s.[4],s.[5],s.[PayorID],[s].[ClaimID] ORDER BY [s].[4]) RowID
		,DENSE_RANK() OVER (ORDER BY s.[4],s.[5],s.[PayorID],[s].[ClaimID]) DenseRank
		,s.[RowID]
	FROM   [etl].[StagedLakerFile] AS s
	WHERE  1 = 1
			AND s.[ClaimID] IS NOT NULL
			AND [s].[4] IS NOT NULL
			AND [s].[5] IS NOT NULL
			AND [s].[PayorID] IS NOT NULL
			
	-- Invoice Import Statement
	INSERT [dbo].[Invoice] ([ARItemKey],[Amount],[InvoiceNumber],[InvoiceDate],[ETLRowID])
	SELECT [i].[ARItemKey],[i].[Amount],[i].[InvoiceNumber],[i].[InvoiceDate],[i].[ETLRowID]
	FROM [#InvoiceImport] i WHERE i.RowNumber = 1
	SET @RowCountCheck = @@ROWCOUNT
			
	CREATE TABLE #ProcessInvoice ([InvoiceID] INT, DenseRank INT NOT NULL, ETLRowID VARCHAR(50) NOT NULL)
	INSERT #ProcessInvoice ([InvoiceID], [DenseRank], ETLRowID)
	SELECT [inv].[InvoiceID]
			, [i].[DenseRank]
			, [s].[RowID]
	FROM   [etl].[StagedLakerFile] AS s
			INNER JOIN [#InvoiceImport] AS i ON [i].[ETLRowID] = [s].[RowID]
			LEFT JOIN [dbo].[Invoice] AS [inv] ON [s].[RowID] = [inv].[ETLRowID];
	
	CREATE TABLE #UpdateInvoice (ETLRowID VARCHAR(50) NOT NULL, InvoiceID INT NOT NULL)
	INSERT #UpdateInvoice ([ETLRowID],[InvoiceID])
	SELECT [inv].ETLRowID
			, InvoiceID = MIN([inv].[InvoiceID]) OVER ( 
					PARTITION BY [inv].[DenseRank] ORDER BY [inv].[DenseRank]
					ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING )
	FROM   [#ProcessInvoice] AS [inv]
	
	UPDATE [s]
	SET    [s].InvoiceID = [c].InvoiceID
	FROM   #UpdateInvoice AS c
		   INNER JOIN [etl].[StagedLakerFile] AS s WITH (TABLOCKX) ON [s].[RowID] = [c].[ETLRowID];

	-- Invoice QA Check
	IF @RowCountCheck != (SELECT COUNT(DISTINCT s.InvoiceID) FROM [etl].[StagedLakerFile] AS s)
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK;
			RAISERROR(N'Error. The QA check for the count of rows inserted into Invoice, and the distinct count of InvoiceID''s found in the StagedLakerFile table failed.', 16, 1) WITH NOWAIT;
			RETURN;
		END

	/********************************************************************************************
	End Invoice Section
	********************************************************************************************/
	
	/********************************************************************************************
	Begin Prescription Section
	********************************************************************************************/
	INSERT INTO [dbo].[Prescription] ([ClaimID],[DateSubmitted],[RxNumber],[DateFilled],[RefillDate],[RefillNumber],[MONY],
			[DAW],[Quantity],[DaySupply],[NDC],[LabelName],[GPI],[BillIngrCost],[BillDispFee],[BilledTax],[BilledCopay],
			[BilledAmount],[PayIngrCost],[PayDispFee],[PayTax],[PayableAmount],[DEA],[PrescriberNPI],[AWPUnit],[Usual],
			[Compound],[Strength],[GPIGenName],[TheraClass],[Generic],[PharmacyNABP],[Prescriber],[TransactionType],[TranID],
			[InvoiceID],[ETLRowID])
	SELECT	s.[ClaimID],s.[3],s.[60],s.[61],s.[62],s.[63],s.[64],s.[65],s.[66],s.[67],s.[68],s.[69],s.[70],s.[71],s.[72],s.[73]
			,s.[74],s.[75],s.[76],s.[77],s.[78],s.[79],s.[80],s.[81],s.[105],s.[122],
			ISNULL(s.[123],'') -- TODO: Remove at some point, this column is non-nullable.
			,s.[137],s.[143],s.[146]
			,'Y','','','','',s.[InvoiceID],s.[RowID] -- Question for Adam: this isn't in the mapping file. [Generic],[PharmacyNABP]
	FROM	[etl].[StagedLakerFile] AS s

	UPDATE e SET e.[PrescriptionID] = [p].[PrescriptionID]
	FROM   [dbo].[Prescription] AS [p]
			INNER JOIN [etl].[StagedLakerFile] AS e ON [e].[RowID] = [p].[ETLRowID]
	SET @RowCountCheck = @@ROWCOUNT

	-- QA check, ensure that there is a Prescription for every record.
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN PrescriptionID INT NOT NULL')

	-- Ok, set it back.
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN PrescriptionID INT NULL')
			
	-- Prescription QA Check
	IF (@RowCountCheck != (SELECT COUNT(*) FROM 
			(SELECT DISTINCT s.[ClaimID],s.[3],s.[60],s.[61],s.[62],s.[63],s.[64],s.[65],s.[66],s.[67],
			s.[68],s.[69],s.[70],s.[71],s.[72],s.[73]
			,s.[74],s.[75],s.[76],s.[77],s.[78],s.[79],s.[80],s.[81],s.[105],s.[122],s.[123],s.[137],s.[143],s.[146]
				FROM [etl].[StagedLakerFile] AS s
				) AS a))
		BEGIN
			IF (@@TRANCOUNT > 0)
				ROLLBACK TRANSACTION;
			RAISERROR(N'Something went wrong. The total row count of the Staging table didn''t match the distinct count of Prescription columns.', 16, 1) WITH NOWAIT
			RETURN
		END

	/********************************************************************************************
	Begin Pharmacy Section
	********************************************************************************************/
	CREATE TABLE #PharmacyImport
	(
		[89] [varchar] (500) NOT NULL,
		[90] [varchar] (8000) NULL,
		[91] [varchar] (8000) NULL,
		[92] [varchar] (8000) NULL,
		[93] [varchar] (8000) NULL,
		[94] [varchar] (8000) NULL,
		[StateID] [int] NULL,
		[96] [varchar] (8000) NULL,
		[97] [varchar] (8000) NULL,
		[RowNumber] [int] NULL,
		[DenseRank] [int] NULL,
		ETLRowID VARCHAR(50) NOT NULL,
		INDEX idxTempPharmacyImport NONCLUSTERED ([89])
	)
	INSERT INTO [#PharmacyImport] ([89],[90],[91],[92],[93],[94],[StateID],[96],[97],[RowNumber],[DenseRank],ETLRowID)
	SELECT [s].[89],[s].[90],[s].[91],[s].[92],[s].[93],[s].[94],[us].[StateID],[s].[96],[s].[97]
		,ROW_NUMBER() OVER (PARTITION BY [s].[89],[s].[90],[s].[91],[s].[92],[s].[93],[s].[94],[us].[StateID],[s].[96],[s].[97] ORDER BY [s].[89]) RowID
		,DENSE_RANK() OVER (ORDER BY [s].[89],[s].[90],[s].[91],[s].[92],[s].[93],[s].[94],[us].[StateID],[s].[96],[s].[97]) DenseRank, 
		[s].[RowID]
	FROM [etl].[StagedLakerFile] AS [s]
			LEFT JOIN [dbo].[UsState] AS [us] ON [us].[StateCode] = [s].[95]
	WHERE [s].[89] IS NOT NULL
			OR [s].[90] IS NOT NULL
			OR [s].[91] IS NOT NULL
			OR [s].[92] IS NOT NULL
			OR [s].[93] IS NOT NULL
			OR [s].[94] IS NOT NULL
			OR [us].[StateID] IS NOT NULL
			OR [s].[96] IS NOT NULL
			OR [s].[97] IS NOT NULL

	INSERT INTO [dbo].[Pharmacy] ([NABP],[NPI],[PharmacyName],[Address1],[Address2],[City],[StateID],[PostalCode],[DispType], [ETLRowID])
	SELECT	[p].[89],[p].[90],[p].[91],[p].[92],[p].[93],[p].[94],[p].[StateID],[p].[96],util.udfTrimLeadingZeros([p].[97]),[p].[ETLRowID]
	FROM	[#PharmacyImport] AS [p]
	WHERE	[p].[RowNumber] = 1
	SET @RowCountCheck = @@ROWCOUNT

	CREATE TABLE #ProcessPharmacy ([PharmacyID] INT, DenseRank INT NOT NULL, ETLRowID VARCHAR(50) NOT NULL)
	INSERT #ProcessPharmacy ([PharmacyID],[DenseRank],ETLRowID)
	SELECT [p].[PharmacyID]
			, [i].[DenseRank]
			, [s].[RowID]
	FROM   [etl].[StagedLakerFile] AS s
			INNER JOIN [#PharmacyImport] AS i ON [i].[ETLRowID] = [s].[RowID]
			LEFT JOIN [dbo].[Pharmacy] AS [p] ON [s].[RowID] = [p].[ETLRowID];

	WITH WindowingMagicCTE AS
	(
		SELECT ph.ETLRowID
				, PharmacyID = MIN(ph.[PharmacyID]) OVER (
						PARTITION BY ph.[DenseRank] ORDER BY ph.[DenseRank]
						ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING)
		FROM   #ProcessPharmacy AS ph WHERE ph.[PharmacyID] IS NOT NULL AND ph.ETLRowID IS NOT NULL
	)
	UPDATE [s]
	SET    [s].[PharmacyID] = [c].[PharmacyID]
	FROM   [WindowingMagicCTE] AS c
			INNER JOIN [etl].[StagedLakerFile] AS s WITH (TABLOCKX) ON [c].[ETLRowID] = [s].[RowID]
	SET @RowCountCheck = @@ROWCOUNT
			
	-- Pharmacy QA Check
	IF @RowCountCheck != (SELECT COUNT(DISTINCT s.PharmacyID) FROM [etl].[StagedLakerFile] AS s)
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK;
			RAISERROR(N'Error. The QA check for the count of rows inserted into Pharmacy, and the distinct count of PharmacyID''s found in the StagedLakerFile table failed.', 16, 1) WITH NOWAIT;
			RETURN;
		END

	IF (SELECT COUNT(DISTINCT [p].[NPI]) FROM [dbo].[Pharmacy] AS [p]) != @RowCountCheck
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK;
			RAISERROR(N'Error. The QA check for the count of unique NPI numbers for a Pharmacy did not match the total quantity inserted for Pharmacies', 16, 1) WITH NOWAIT
			RETURN
		END
			
	/********************************************************************************************
	End Pharmacy Section
	********************************************************************************************/

	/********************************************************************************************
	Begin AcctPayables Section
	********************************************************************************************/
	INSERT INTO [dbo].[AcctPayable] ([CheckNumber],[CheckDate],[AmountPaid],[ClaimID],[InvoiceID], [ETLRowID])
	SELECT [s].[6]
			, [s].[7]
			, 0
			, [s].[ClaimID]
			, [s].[InvoiceID]
			, [s].[RowID]
	FROM   [etl].[StagedLakerFile] AS [s]
	WHERE  1 = 1
			AND [s].[InvoiceID] IS NOT NULL
			AND [s].[6] IS NOT NULL
	SET @RowCountCheck = @@ROWCOUNT

	UPDATE s SET s.AcctPayableID = [p].[AcctPayableID]
	FROM   [dbo].[AcctPayable] AS [p]
			INNER JOIN [etl].[StagedLakerFile] AS [s] ON [s].[RowID] = [p].[ETLRowID]
	IF @@ROWCOUNT != @RowCountCheck
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK;
			RAISERROR(N'Error. The count of PaymentIDs updated in the Staging table did not match the count of Payments imported.', 16, 1) WITH NOWAIT
			RETURN
		END

	IF @RowCountCheck != (SELECT COUNT(DISTINCT AcctPayableID) FROM [etl].[StagedLakerFile])
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK;
			RAISERROR(N'Error. The count of PaymentIDs updated in the Staging table did not match the count of Payments imported.', 16, 1) WITH NOWAIT
			RETURN
		END

	/********************************************************************************************
	End Payments Section
	********************************************************************************************/

	/********************************************************************************************
	*********************************************************************************************
	End ETL
	*********************************************************************************************
	********************************************************************************************/
	-- Re-create indexed view.
	EXEC [dbo].[uspCreateIndexedPrescriptionNoteView]
END
GO

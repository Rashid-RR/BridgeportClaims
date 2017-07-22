SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/9/2017
	Description:	The main ETL proc.
	TODO:			Add a QA check in for Claim so that ClaimID in staging table is made NOT NULL
	Sample Execute:
					EXEC etl.uspProcessLakerFile 1, 0
*/
CREATE PROC [etl].[uspProcessLakerFile] 
(
	@Cleanup BIT = 1
)
AS BEGIN
	SET NOCOUNT ON;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;
	SET DEADLOCK_PRIORITY HIGH;
	BEGIN TRY
		BEGIN TRAN;
			-- Testing
			/*DECLARE @ReProcess BIT = 1,
					@Cleanup BIT = 1
			IF OBJECT_ID(N'tempdb..#PatientImport') IS NOT NULL
					DROP TABLE #PatientImport
			IF OBJECT_ID(N'tempdb..#ProcessPatient') IS NOT NULL
					DROP TABLE #ProcessPatient
			IF OBJECT_ID(N'tempdb..#ProcessClaim') IS NOT NULL
					DROP TABLE #ProcessClaim
			IF OBJECT_ID(N'tempdb..#ClaimImport') IS NOT NULL
					DROP TABLE #ClaimImport
			IF OBJECT_ID(N'tempdb..#InvoiceImport') IS NOT NULL
					DROP TABLE #InvoiceImport
			IF OBJECT_ID(N'tempdb..#ProcessInvoice') IS NOT NULL
					DROP TABLE #ProcessInvoice
			IF OBJECT_ID(N'tempdb..#PharmacyImport') IS NOT NULL
					DROP TABLE #PharmacyImport*/

			DECLARE @TotalRowCount INT = (SELECT COUNT(*) FROM [etl].[StagedLakerFile])
		
			-- Clear out tables regarle
			IF EXISTS (SELECT * FROM [sys].[views] AS [v] WHERE [v].[name] = 'vwPrescriptionNote')
				DROP VIEW [dbo].[vwPrescriptionNote]
			EXEC [util].[uspSmarterTruncateTable] 'dbo.Prescription'
			EXEC [util].[uspSmarterTruncateTable] 'dbo.Payment'
			EXEC [util].[uspSmarterTruncateTable] 'dbo.Invoice'
			EXEC [util].[uspSmarterTruncateTable] 'dbo.Pharmacy'
			EXEC [util].[uspSmarterTruncateTable] 'dbo.Episode'
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
			-- Add Row ID for our Staging Table
			IF NOT EXISTS (   SELECT *
							  FROM   [sys].[columns] AS [c]
							  WHERE  [c].[object_id] = OBJECT_ID(N'[etl].[StagedLakerFile]', N'U')
								     AND [c].[name] = 'StageID'
						  )
				EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ADD StageID INT IDENTITY')
			IF NOT EXISTS (   SELECT *
							  FROM   [sys].[columns] AS [c]
							  WHERE  [c].[object_id] = OBJECT_ID(N'[etl].[StagedLakerFile]', N'U')
									 AND [c].[name] = 'PayorID'
						  )
				BEGIN
					EXEC [sys].[sp_executesql] N'ALTER TABLE [etl].[StagedLakerFile] ADD PayorID INT NULL';
				END

			/********************************************************************************************
			Import Payor Section
			********************************************************************************************/
			DECLARE @PayorID TABLE (PayorID INT NOT NULL, [GroupName] VARCHAR(255) NOT NULL)
			INSERT [dbo].[Payor] ([GroupName],[BillToName],[BillToAddress1]
									,[BillToAddress2],[BillToCity],[BillToStateID],[BillToPostalCode],[PhoneNumber],[FaxNumber]
									  ) OUTPUT [Inserted].[PayorID], [Inserted].[GroupName] INTO @PayorID ([PayorID], [GroupName])
			SELECT [s].[42], [s].[43],[s].[44],[s].[45],[s].[46],[us].[StateID],s.[48],s.[49],s.[50]
			FROM   [etl].[StagedLakerFile] AS s
				   LEFT JOIN [dbo].[UsState] AS [us] ON [us].[StateCode] = [s].[47]
			WHERE  [s].[47] IS NOT NULL
			GROUP BY [s].[42], [s].[43],[s].[44],[s].[45],[s].[46],[us].[StateID],s.[48],s.[49],s.[50]
			SET @RowCountCheck = @@ROWCOUNT;
	
			UPDATE s
			SET    [s].[PayorID] = [pi].[PayorID]
			FROM   [etl].[StagedLakerFile] AS s WITH (TABLOCKX)
				   INNER JOIN @PayorID AS [pi] ON [s].[42] = [pi].[GroupName];
	
			-- Payor QA Check
			IF @RowCountCheck != (SELECT COUNT(DISTINCT [s].[PayorID]) FROM etl.[StagedLakerFile] AS s)
				BEGIN
					IF @@TRANCOUNT > 0
						ROLLBACK;
					RAISERROR(N'Error. The row count for the Payor Insert does not match the corresponding Row Count for distinct PayorID''s in the Staging table', 16, 1) WITH NOWAIT;
					RETURN;
				END

			-- Another Payor QA check
			EXEC [sys].[sp_executesql] N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN PayorID INT NOT NULL';

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
			IF NOT EXISTS (   SELECT *
							  FROM   [sys].[columns] AS [c]
							  WHERE  [c].[object_id] = OBJECT_ID(N'[etl].[StagedLakerFile]', N'U')
									 AND [c].[name] = 'AdjustorID'
						  )
				BEGIN
					EXEC [sys].[sp_executesql] N'ALTER TABLE [etl].[StagedLakerFile] ADD AdjustorID INT NULL';
				END

			DECLARE @AdjustorID TABLE (AdjustorID INT NOT NULL, AdjustorName VARCHAR(255) NOT NULL, PayorID INT NOT NULL)
			INSERT [dbo].[Adjustor] ([PayorID], [AdjustorName])
			OUTPUT [Inserted].[AdjustorID], [Inserted].[AdjustorName], [Inserted].[PayorID] INTO @AdjustorID
			SELECT s.[PayorID], s.[21] FROM etl.[StagedLakerFile] AS s WHERE s.[21] IS NOT NULL GROUP BY s.[PayorID], s.[21] ORDER BY s.[21]
			SET @RowCountCheck = @@ROWCOUNT;
	
			UPDATE s SET s.AdjustorID = a.[AdjustorID]
			FROM   [etl].[StagedLakerFile] AS s WITH (TABLOCKX)
				   INNER JOIN @AdjustorID AS a ON a.AdjustorName = s.[21]
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
			IF NOT EXISTS (   SELECT *
							  FROM   [sys].[columns] AS [c]
							  WHERE  [c].[object_id] = OBJECT_ID(N'[etl].[StagedLakerFile]', N'U')
									 AND [c].[name] = 'PatientID'
						  )
				BEGIN
					EXEC [sys].[sp_executesql] N'ALTER TABLE [etl].[StagedLakerFile] ADD PatientID INT NULL'
				END
			IF NOT EXISTS (   SELECT *
							  FROM   [sys].[columns] AS [c]
							  WHERE  [c].[object_id] = OBJECT_ID(N'[dbo].[Patient]', N'U')
									 AND [c].[name] = 'StageID'
						  )
				BEGIN
					EXEC [sys].[sp_executesql] N'ALTER TABLE [dbo].[Patient] ADD StageID INT NOT NULL';
				END
				
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
					[RowID] INT NOT NULL,
					DenseRank INT NOT NULL,
					[StageID] [int] NOT NULL
				)
			INSERT [#PatientImport] ([DateOfBirth],[LastName], [FirstName], [Address1], [Address2], [City], [StateID],[PostalCode]
			                               , [PhoneNumber],[GenderID], [RowID], [DenseRank], [StageID])
			SELECT p.[20], p.[10], p.[11], p.[12],p.[13],p.[14], [us].[StateID],p.[16],p.[17],g.[GenderID]
			,ROW_NUMBER() OVER (PARTITION BY p.[20], p.[10], p.[11], p.[12],p.[13],p.[14], p.[15],p.[16],p.[17],P.[18] ORDER BY p.[20]) RowID
			,DENSE_RANK() OVER (ORDER BY p.[20], p.[10], p.[11], p.[12],p.[13],p.[14], [us].[StateID],p.[16],p.[17],g.[GenderID]) DenseRank, p.[StageID]
			FROM [etl].[StagedLakerFile] AS p
			LEFT JOIN [dbo].[UsState] AS [us] ON p.[15] = [us].[StateCode]
			LEFT JOIN [dbo].[Gender] AS [g] ON g.[GenderCode] = p.[18]
			WHERE p.[10] IS NOT NULL
			
			-- Patient import statement
			INSERT [dbo].[Patient] ([DateOfBirth],[LastName],[FirstName], [Address1],[Address2],[City],[StateID],
									[PostalCode],[PhoneNumber],[GenderID],[StageID])
			SELECT [p].[DateOfBirth], [p].[LastName], [p].[FirstName], [p].[Address1], [p].[Address2], [p].[City], [p].[StateID]
                 , [p].[PostalCode], [p].[PhoneNumber], [p].[GenderID], [p].[StageID] 
			FROM [#PatientImport] AS p WHERE [p].[RowID] = 1
			SET @RowCountCheck = @@ROWCOUNT

			CREATE TABLE #ProcessPatient (PatientID INT, DenseRank INT NOT NULL, StageID INT NOT NULL)
			INSERT [#ProcessPatient] ([PatientID], [DenseRank], [StageID])
			SELECT [p].[PatientID]
				 , [i].[DenseRank]
				 , [s].[StageID]
			FROM   [etl].[StagedLakerFile] AS s
				   INNER JOIN [#PatientImport] AS i ON [i].[StageID] = [s].[StageID]
				   LEFT JOIN [dbo].[Patient] AS [p] ON [s].[StageID] = [p].[StageID];

			WITH WindowingMagicCTE AS
            (
				SELECT [pp].[StageID]
					 , PatientID = MIN([pp].[PatientID]) OVER ( 
								PARTITION BY [pp].[DenseRank] ORDER BY [pp].[DenseRank]
								ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING )
				FROM   [#ProcessPatient] AS [pp]
			)
			UPDATE [s]
			SET    [s].[PatientID] = [c].[PatientID]
			FROM   [WindowingMagicCTE] AS c
				   INNER JOIN [etl].[StagedLakerFile] AS s WITH (TABLOCKX) ON [s].[StageID] = [c].[StageID];

			EXEC [sys].[sp_executesql] N'ALTER TABLE dbo.Patient DROP COLUMN StageID'

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
			IF NOT EXISTS (   SELECT *
							  FROM   [sys].[columns] AS [c]
							  WHERE  [c].[object_id] = OBJECT_ID(N'[etl].[StagedLakerFile]', N'U')
									 AND [c].[name] = 'ClaimID'
						  )
				BEGIN
					EXEC [sys].[sp_executesql] N'ALTER TABLE [etl].[StagedLakerFile] ADD ClaimID INT NULL'
				END
			IF NOT EXISTS (   SELECT *
							  FROM   [sys].[columns] AS [c]
							  WHERE  [c].[object_id] = OBJECT_ID(N'[dbo].[Claim]', N'U')
									 AND [c].[name] = 'StageID'
						  )
				BEGIN
					EXEC [sys].[sp_executesql] N'ALTER TABLE [dbo].[Claim] ADD StageID INT NULL'
				END
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
						[RowID] [int] NULL,
						[DenseRank] [int] NULL,
						[StageID] [int] NOT NULL)
			INSERT INTO [#ClaimImport] ([8],[9],[19],[22],[25],[125],[PatientID],[PayorID],[AdjustorID],[RowID],[DenseRank],[StageID])
			SELECT [s].[8],[s].[9],[s].[19],[s].[22],[s].[25],[s].[125],s.[PatientID],s.[PayorID],s.[AdjustorID]
			,ROW_NUMBER() OVER (PARTITION BY [s].[8],[s].[9],[s].[19],[s].[22],[s].[25],[s].[125],s.[PatientID],s.[PayorID],s.[AdjustorID] ORDER BY [s].[8]) RowID
			,DENSE_RANK() OVER (ORDER BY [s].[8],[s].[9],[s].[19],[s].[22],[s].[25],[s].[125],s.[PatientID],s.[PayorID],s.[AdjustorID]) DenseRank, s.[StageID]
			FROM   [etl].[StagedLakerFile] AS s
			WHERE  s.[PatientID] IS NOT NULL
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
					   , [StageID]
					   , [IsFirstParty] -- Check on this, doesn't allow NULLs and is not in the mapping file.
                     )
			SELECT	[ci].[8],[ci].[9],[ci].[19],[ci].[22],[ci].[25],[ci].[125],[ci].[PatientID],[ci].[PayorID],[ci].[AdjustorID],[ci].[StageID], 1
			FROM	[#ClaimImport] AS [ci] 
			WHERE	[ci].[RowID] = 1
			SET @RowCountCheck = @@ROWCOUNT

			CREATE TABLE #ProcessClaim (ClaimID INT, DenseRank INT NOT NULL, StageID INT NOT NULL)
			INSERT #ProcessClaim ([ClaimID], [DenseRank], [StageID])
			SELECT [c].[ClaimID]
				 , [i].[DenseRank]
				 , [s].[StageID]
			FROM   [etl].[StagedLakerFile] AS s
				   INNER JOIN [#ClaimImport] AS i ON [i].[StageID] = [s].[StageID]
				   LEFT JOIN [dbo].[Claim] AS [c] ON [s].[StageID] = [c].[StageID];

			WITH WindowingMagicCTE AS
            (
				SELECT [pc].[StageID]
					 , ClaimID = MIN([pc].[ClaimID]) OVER ( 
								PARTITION BY [pc].[DenseRank] ORDER BY [pc].[DenseRank]
								ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING)
				FROM   [#ProcessClaim] AS [pc]
			)
			UPDATE [s]
			SET    [s].[ClaimID] = [c].[ClaimID]
			FROM   [WindowingMagicCTE] AS c
				   INNER JOIN [etl].[StagedLakerFile] AS s WITH (TABLOCKX) ON [s].[StageID] = [c].[StageID];
			
			EXEC [sys].[sp_executesql] N'ALTER TABLE dbo.Claim DROP COLUMN StageID'

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
			IF NOT EXISTS (   SELECT *
							  FROM   [sys].[columns] AS [c]
							  WHERE  [c].[object_id] = OBJECT_ID(N'[etl].[StagedLakerFile]', N'U')
									 AND [c].[name] = 'InvoiceID'
						  )
				BEGIN
					EXEC [sys].[sp_executesql] N'ALTER TABLE [etl].[StagedLakerFile] ADD InvoiceID INT NULL'
				END
			IF NOT EXISTS (   SELECT *
							  FROM   [sys].[columns] AS [c]
							  WHERE  [c].[object_id] = OBJECT_ID(N'[dbo].[Invoice]', N'U')
									 AND [c].[name] = 'StageID'
						  )
				BEGIN
					EXEC [sys].[sp_executesql] N'ALTER TABLE [dbo].[Invoice] ADD StageID INT NULL'
				END
			
			CREATE TABLE #InvoiceImport (ARItemKey VARCHAR(1),Amount MONEY,InvoiceNumber VARCHAR(8000),InvoiceDate VARCHAR(8000),[PayorID] INT NOT NULL
										,[ClaimID] INT NOT NULL, RowID INT NOT NULL, DenseRank INT NOT NULL,StageID INT NOT NULL)
			INSERT INTO [#InvoiceImport] ([ARItemKey],[Amount],[InvoiceNumber],[InvoiceDate],[PayorID],[ClaimID],[RowID],[DenseRank],[StageID])
			SELECT '' -- Have to check on Mapping of ARIItemKey
				,  0 -- Have to check on Mapping of Amount
				, s.[4],s.[5],s.[PayorID],[s].[ClaimID]
				,ROW_NUMBER() OVER (PARTITION BY s.[4],s.[5],s.[PayorID],[s].[ClaimID] ORDER BY [s].[4]) RowID
				,DENSE_RANK() OVER (ORDER BY s.[4],s.[5],s.[PayorID],[s].[ClaimID]) DenseRank
				,s.[StageID]
			FROM   [etl].[StagedLakerFile] AS s
			WHERE  1 = 1
				   AND s.[ClaimID] IS NOT NULL
				   AND [s].[4] IS NOT NULL
				   AND [s].[5] IS NOT NULL
				   AND [s].[PayorID] IS NOT NULL

			-- Invoice Import Statement
			INSERT [dbo].[Invoice] ([ARItemKey],[Amount],[InvoiceNumber],[InvoiceDate],[PayorID],[ClaimID],StageID)
			SELECT [i].[ARItemKey],[i].[Amount],[i].[InvoiceNumber],[i].[InvoiceDate],[i].[PayorID],[i].[ClaimID],[i].[StageID]
			FROM [#InvoiceImport] i WHERE i.[RowID] = 1
			SET @RowCountCheck = @@ROWCOUNT

			CREATE TABLE #ProcessInvoice ([InvoiceID] INT, DenseRank INT NOT NULL, StageID INT NOT NULL)
			INSERT #ProcessInvoice ([InvoiceID], [DenseRank], [StageID])
			SELECT [inv].[InvoiceID]
				 , [i].[DenseRank]
				 , [s].[StageID]
			FROM   [etl].[StagedLakerFile] AS s
				   INNER JOIN [#InvoiceImport] AS i ON [i].[StageID] = [s].[StageID]
				   LEFT JOIN [dbo].[Invoice] AS [inv] ON [s].[StageID] = [inv].[StageID];

			WITH WindowingMagicCTE AS
            (
				SELECT [inv].[StageID]
					 , InvoiceID = MIN([inv].[InvoiceID]) OVER ( 
								PARTITION BY [inv].[DenseRank] ORDER BY [inv].[DenseRank]
								ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING )
				FROM   [#ProcessInvoice] AS [inv] WHERE [inv].[InvoiceID] IS NOT NULL AND inv.[StageID] IS NOT NULL
			)
			UPDATE [s]
			SET    [s].InvoiceID = [c].InvoiceID
			FROM   [WindowingMagicCTE] AS c
				   INNER JOIN [etl].[StagedLakerFile] AS s WITH (TABLOCKX) ON [s].[StageID] = [c].[StageID];
			SET @RowCountCheck = @@ROWCOUNT

			EXEC [sys].[sp_executesql] N'ALTER TABLE dbo.Invoice DROP COLUMN StageID'
			
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
			IF NOT EXISTS (   SELECT *
							  FROM   [sys].[columns] AS [c]
							  WHERE  [c].[object_id] = OBJECT_ID(N'[etl].[StagedLakerFile]', N'U')
									 AND [c].[name] = 'PharmacyID'
						  )
				BEGIN
					EXEC [sys].[sp_executesql] N'ALTER TABLE [etl].[StagedLakerFile] ADD PharmacyID INT NULL'
				END

			INSERT INTO [dbo].[Prescription] ([ClaimID],[DateSubmitted],[RxNumber],[DateFilled],[RefillDate],[RefillNumber],[MONY],
					[DAW],[Quantity],[DaySupply],[NDC],[LabelName],[GPI],[BillIngrCost],[BillDispFee],[BilledTax],[BilledCopay],
					[BilledAmount],[PayIngrCost],[PayDispFee],[PayTax],[PayableAmount],[DEA],[PrescriberNPI],[AWPUnit],[Usual],
					[Compound],[Strength],[GPIGenName],[TheraClass],[Generic],[PharmacyNABP],[Prescriber],[TransactionType],[TranID])
					-- [AWP],[PrescriptionTran],[InvoiceID])
			SELECT	s.[ClaimID],s.[3],s.[60],s.[61],s.[62],s.[63],s.[64],s.[65],s.[66],s.[67],s.[68],s.[69],s.[70],s.[71],s.[72],s.[73]
					,s.[74],s.[75],s.[76],s.[77],s.[78],s.[79],s.[80],s.[81],s.[105],s.[122],
					ISNULL(s.[123],'') -- TODO: Remove at some point, this column is non-nullable.
					,s.[137],s.[143],s.[146]
					,'Y','','','','' -- Question for Adam: this isn't in the mapping file. [Generic],[PharmacyNABP]
			FROM	[etl].[StagedLakerFile] AS s
			
			-- Prescription QA Check
			IF (@TotalRowCount != (SELECT COUNT(*) FROM 
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
			IF NOT EXISTS
            (
				SELECT * FROM [sys].[columns] AS [c]
				INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
				WHERE [t].[name] = 'Pharmacy'
					AND [c].[name] = 'StageID'
			)
				EXEC(N'ALTER TABLE dbo.Pharmacy ADD StageID INT NULL')

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
				[RowID] [bigint] NULL,
				[DenseRank] [bigint] NULL,
				StageID INT NOT NULL,
				INDEX idxTempPharmacyImport NONCLUSTERED ([89])
			)
			INSERT INTO [#PharmacyImport] ([89],[90],[91],[92],[93],[94],[StateID],[96],[97],[RowID],[DenseRank],StageID)
			SELECT [s].[89],[s].[90],[s].[91],[s].[92],[s].[93],[s].[94],[us].[StateID],[s].[96],[s].[97]
				,ROW_NUMBER() OVER (PARTITION BY [s].[89],[s].[90],[s].[91],[s].[92],[s].[93],[s].[94],[us].[StateID],[s].[96],[s].[97] ORDER BY [s].[89]) RowID
				,DENSE_RANK() OVER (ORDER BY [s].[89],[s].[90],[s].[91],[s].[92],[s].[93],[s].[94],[us].[StateID],[s].[96],[s].[97]) DenseRank, [s].[StageID]
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

			INSERT INTO [dbo].[Pharmacy] ([NABP],[NPI],[PharmacyName],[Address1],[Address2],[City],[StateID],[PostalCode],[DispType])
			SELECT	[p].[89],[p].[90],[p].[91],[p].[92],[p].[93],[p].[94],[p].[StateID],[p].[96],util.udfTrimLeadingZeros([p].[97])
			FROM	[#PharmacyImport] AS [p]
			WHERE	[p].[RowID] = 1
			SET @RowCountCheck = @@ROWCOUNT

			CREATE TABLE #ProcessPharmacy ([PharmacyID] INT, DenseRank INT NOT NULL, StageID INT NOT NULL)
			INSERT #ProcessPharmacy ([PharmacyID],[DenseRank],[StageID])
			SELECT [p].[PharmacyID]
				 , [i].[DenseRank]
				 , [s].[StageID]
			FROM   [etl].[StagedLakerFile] AS s
				   INNER JOIN [#PharmacyImport] AS i ON [i].[StageID] = [s].[StageID]
				   LEFT JOIN [dbo].[Pharmacy] AS [p] ON [s].[StageID] = [p].[StageID];

			WITH WindowingMagicCTE AS
            (
				SELECT ph.[StageID]
					 , PharmacyID = MIN(ph.[PharmacyID]) OVER (
								PARTITION BY ph.[DenseRank] ORDER BY ph.[DenseRank]
								ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING)
				FROM   #ProcessPharmacy AS ph WHERE ph.[PharmacyID] IS NOT NULL AND ph.[StageID] IS NOT NULL
			)
			UPDATE [s]
			SET    [s].[PharmacyID] = [c].[PharmacyID]
			FROM   [WindowingMagicCTE] AS c
				   INNER JOIN [etl].[StagedLakerFile] AS s WITH (TABLOCKX) ON [s].[StageID] = [c].[StageID];
			SET @RowCountCheck = @@ROWCOUNT
			
			EXEC [sys].[sp_executesql] N'ALTER TABLE dbo.Pharmacy DROP COLUMN StageID'
			
			-- Pharmacy QA Check
			IF @RowCountCheck != (SELECT COUNT(DISTINCT s.PharmacyID) FROM [etl].[StagedLakerFile] AS s)
				BEGIN
					IF @@TRANCOUNT > 0
						ROLLBACK;
					RAISERROR(N'Error. The QA check for the count of rows inserted into Pharmacy, and the distinct count of PharmacyID''s found in the StagedLakerFile table failed.', 16, 1) WITH NOWAIT;
					RETURN;
				END
			
			/********************************************************************************************
			End Pharmacy Section
			********************************************************************************************/

			/********************************************************************************************
			Begin Payments Section
			********************************************************************************************/
			INSERT INTO [dbo].[Payment] ([CheckNumber],[CheckDate],[AmountPaid],[ClaimID],[InvoiceID])
			SELECT [s].[6]
				 , [s].[7]
				 , 0
				 , [s].[ClaimID]
				 , [s].[InvoiceID]
			FROM   [etl].[StagedLakerFile] AS [s]
			WHERE  1 = 1
				   AND [s].[InvoiceID] IS NOT NULL
				   AND [s].[6] IS NOT NULL

			/********************************************************************************************
			End Payments Section
			********************************************************************************************/

			/********************************************************************************************
			*********************************************************************************************
			Final Steps. Cleanup Staging Table
			*********************************************************************************************
			********************************************************************************************/
			IF @Cleanup = 1
				BEGIN
					EXEC [sys].[sp_executesql] N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN PayorID INT NULL';
					UPDATE [etl].[StagedLakerFile] SET PayorID = NULL WHERE 1 =1;
					UPDATE [etl].[StagedLakerFile] SET [AdjustorID] = NULL WHERE 1 =1;
					UPDATE [etl].[StagedLakerFile] SET [PatientID] = NULL WHERE 1 =1;
					UPDATE [etl].[StagedLakerFile] SET [InvoiceID] = NULL WHERE 1 = 1;
					EXEC [sys].[sp_executesql] N'ALTER TABLE [etl].[StagedLakerFile] DROP COLUMN StageID';
				END
			-- Re-create indexed view.
			EXEC [dbo].[uspCreateIndexedPrescriptionNoteView]
		IF @@TRANCOUNT > 0
			COMMIT
	END TRY
    BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK;
		THROW;
	END CATCH
END
GO

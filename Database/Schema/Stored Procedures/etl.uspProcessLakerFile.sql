SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	9/5/2017
	Description:	Master proc to Process Laker File
	Sample Execute:
					EXEC etl.uspProcessLakerFile
*/
CREATE PROC [etl].[uspProcessLakerFile]
AS BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	SET DEADLOCK_PRIORITY HIGH;
	SET NOCOUNT ON;
	BEGIN TRAN;
	EXEC [etl].[uspCleanupStagedLakerFileAddedColumns]
	EXEC [etl].[uspAddStagedLakerFileETLColumns]

	-- Update the various ID's from previous File
	UPDATE [s]
	SET    [s].[PayorID] = [b].[PayorID],
		   [s].[AcctPayableID] = [b].[AcctPayableID],
		   [s].[AdjustorID] = [b].[AdjustorID],
		   [s].[PatientID] = [b].[PatientID],
		   [s].[InvoiceID] = [b].[InvoiceID],
		   [s].[ClaimID] = [b].[ClaimID],
		   [s].[PrescriptionID] = [b].[PrescriptionID],
		   [s].[NABP] = [b].[NABP]
	FROM   [etl].[StagedLakerFile] AS [s]
		   INNER JOIN [etl].[StagedLakerFileBackup] AS [b] ON [b].[RowID] = [s].[RowID]

	IF OBJECT_ID(N'tempdb..#PayorUpdate') IS NOT NULL
		DROP TABLE #PayorUpdate
	IF OBJECT_ID(N'tempdb..#Payor') IS NOT NULL
		DROP TABLE #Payor
	IF OBJECT_ID(N'tempdb..#New') IS NOT NULL
		DROP TABLE #New
	IF OBJECT_ID(N'tempdb..#PayorUpdate') IS NOT NULL
		DROP TABLE #PayorUpdate
	IF OBJECT_ID(N'tempdb..#Adjustor') IS NOT NULL
		DROP TABLE #Adjustor
	IF OBJECT_ID(N'tempdb..#AdjustorUpdate') IS NOT NULL
		DROP TABLE #AdjustorUpdate
	IF OBJECT_ID(N'tempdb..#Patient') IS NOT NULL
		DROP TABLE #Patient
	IF OBJECT_ID(N'tempdb..#PatientUpdate') IS NOT NULL
		DROP TABLE #PatientUpdate
	IF OBJECT_ID(N'tempdb..#PatientDupes') IS NOT NULL
		DROP TABLE #PatientDupes
	IF OBJECT_ID(N'tempdb..#Claim ') IS NOT NULL
		DROP TABLE #Claim
	IF OBJECT_ID(N'tempdb..#ClaimUpdate') IS NOT NULL
		DROP TABLE #ClaimUpdate
	IF OBJECT_ID(N'tempdb..#Invoice') IS NOT NULL
		DROP TABLE #Invoice
	IF OBJECT_ID(N'tempdb..#InvoiceUpdate') IS NOT NULL
		DROP TABLE #InvoiceUpdate
	IF OBJECT_ID(N'tempdb..#Pharmacy') IS NOT NULL
		DROP TABLE #Pharmacy
	IF OBJECT_ID(N'tempdb..#PrescriptionUpdate') IS NOT NULL
		DROP TABLE #PrescriptionUpdate
	IF OBJECT_ID(N'tempdb..#PharmacyUpdate') IS NOT NULL
		DROP TABLE #PharmacyUpdate

	CREATE TABLE #New ([RowID] [varchar](50) NOT NULL,[2] [varchar](8000) NULL,
	[3] [varchar](8000) NULL,[4] [varchar](8000) NULL,[5] [varchar](8000) NULL,[6] [varchar](8000) NULL,
	[7] [varchar](8000) NULL,[8] [varchar](8000) NULL,[9] [varchar](8000) NULL,[10] [varchar](8000) NULL,
	[11] [varchar](8000) NULL,[12] [varchar](8000) NULL,[13] [varchar](8000) NULL,[14] [varchar](8000) NULL,
	[15] [varchar](8000) NULL,[16] [varchar](8000) NULL,[17] [varchar](8000) NULL,[18] [varchar](8000) NULL,
	[19] [varchar](8000) NULL,[20] [varchar](8000) NULL,[21] [varchar](8000) NULL,[22] [varchar](8000) NULL,
	[23] [varchar](8000) NULL,[24] [varchar](8000) NULL,[25] [varchar](8000) NULL,[26] [varchar](8000) NULL,
	[27] [varchar](8000) NULL,[28] [varchar](8000) NULL,[29] [varchar](8000) NULL,[30] [varchar](8000) NULL,
	[31] [varchar](8000) NULL,[32] [varchar](8000) NULL,[33] [varchar](8000) NULL,[34] [varchar](8000) NULL,
	[35] [varchar](8000) NULL,[36] [varchar](8000) NULL,[37] [varchar](8000) NULL,[38] [varchar](8000) NULL,
	[39] [varchar](8000) NULL,[40] [varchar](8000) NULL,[41] [varchar](8000) NULL,[42] [varchar](8000) NULL,
	[43] [varchar](8000) NULL,[44] [varchar](8000) NULL,[45] [varchar](8000) NULL,[46] [varchar](8000) NULL,
	[47] [varchar](8000) NULL,[48] [varchar](8000) NULL,[49] [varchar](8000) NULL,[50] [varchar](8000) NULL,
	[51] [varchar](8000) NULL,[52] [varchar](8000) NULL,[53] [varchar](8000) NULL,[54] [varchar](8000) NULL,
	[55] [varchar](8000) NULL,[56] [varchar](8000) NULL,[57] [varchar](8000) NULL,[58] [varchar](8000) NULL,
	[59] [varchar](8000) NULL,[60] [varchar](8000) NULL,[61] [varchar](8000) NULL,[62] [varchar](8000) NULL,
	[63] [varchar](8000) NULL,[64] [varchar](8000) NULL,[65] [varchar](8000) NULL,[66] [varchar](8000) NULL,
	[67] [varchar](8000) NULL,[68] [varchar](8000) NULL,[69] [varchar](8000) NULL,[70] [varchar](8000) NULL,
	[71] [varchar](8000) NULL,[72] [varchar](8000) NULL,[73] [varchar](8000) NULL,[74] [varchar](8000) NULL,
	[75] [varchar](8000) NULL,[76] [varchar](8000) NULL,[77] [varchar](8000) NULL,[78] [varchar](8000) NULL,
	[79] [varchar](8000) NULL,[80] [varchar](8000) NULL,[81] [varchar](8000) NULL,[82] [varchar](8000) NULL,
	[83] [varchar](8000) NULL,[84] [varchar](8000) NULL,[85] [varchar](8000) NULL,[86] [varchar](8000) NULL,
	[87] [varchar](8000) NULL,[88] [varchar](8000) NULL,[89] [varchar](8000) NULL,[90] [varchar](8000) NULL,
	[91] [varchar](8000) NULL,[92] [varchar](8000) NULL,[93] [varchar](8000) NULL,[94] [varchar](8000) NULL,
	[95] [varchar](8000) NULL,[96] [varchar](8000) NULL,[97] [varchar](8000) NULL,[98] [varchar](8000) NULL,
	[99] [varchar](8000) NULL,[100] [varchar](8000) NULL,[101] [varchar](8000) NULL,[102] [varchar](8000) NULL,
	[103] [varchar](8000) NULL,[104] [varchar](8000) NULL,[105] [varchar](8000) NULL,[106] [varchar](8000) NULL,
	[107] [varchar](8000) NULL,[108] [varchar](8000) NULL,[109] [varchar](8000) NULL,[110] [varchar](8000) NULL,
	[111] [varchar](8000) NULL,[112] [varchar](8000) NULL,[113] [varchar](8000) NULL,[114] [varchar](8000) NULL,
	[115] [varchar](8000) NULL,[116] [varchar](8000) NULL,[117] [varchar](8000) NULL,[118] [varchar](8000) NULL,
	[119] [varchar](8000) NULL,[120] [varchar](8000) NULL,[121] [varchar](8000) NULL,[122] [varchar](8000) NULL,
	[123] [varchar](8000) NULL,[124] [varchar](8000) NULL,[125] [varchar](8000) NULL,[126] [varchar](8000) NULL,
	[127] [varchar](8000) NULL,[128] [varchar](8000) NULL,[129] [varchar](8000) NULL,[130] [varchar](8000) NULL,
	[131] [varchar](8000) NULL,[132] [varchar](8000) NULL,[133] [varchar](8000) NULL,[134] [varchar](8000) NULL,
	[135] [varchar](8000) NULL,[136] [varchar](8000) NULL,[137] [varchar](8000) NULL,[138] [varchar](8000) NULL,
	[139] [varchar](8000) NULL,[140] [varchar](8000) NULL,[141] [varchar](8000) NULL,[142] [varchar](8000) NULL,
	[143] [varchar](8000) NULL,[144] [varchar](8000) NULL,[145] [varchar](8000) NULL,[146] [varchar](8000) NULL,
	[147] [varchar](8000) NULL,[148] [varchar](8000) NULL,[149] [varchar](8000) NULL,[150] [varchar](8000) NULL,
	[151] [varchar](8000) NULL,[152] [varchar](8000) NULL,[153] [varchar](8000) NULL,[154] [varchar](8000) NULL,
	[PayorID] [int] NULL,[AcctPayableID] [int] NULL,[AdjustorID] [int] NULL,[PatientID] [int] NULL,
	[InvoiceID] [int] NULL,[ClaimID] [int] NULL,[PrescriptionID] [int] NULL,[NABP] VARCHAR(7) NULL)
	INSERT [#New] ([RowID],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16]
	,[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31],[32],[33],[34],[35]
	,[36],[37],[38],[39],[40],[41],[42],[43],[44],[45],[46],[47],[48],[49],[50],[51],[52],[53],[54],[55],[56],[57]
	,[58],[59],[60],[61],[62],[63],[64],[65],[66],[67],[68],[69],[70],[71],[72],[73],[74],[75],[76],[77],[78]
	,[79],[80],[81],[82],[83],[84],[85],[86],[87],[88],[89],[90],[91],[92],[93],[94],[95],[96],[97],[98],[99]
	,[100],[101],[102],[103],[104],[105],[106],[107],[108],[109],[110],[111],[112],[113],[114],[115],[116],[117],[118],
	[119],[120],[121],[122],[123],[124],[125],[126],[127],[128],[129],[130],[131],[132],[133],[134],[135],[136],[137]
	,[138],[139],[140],[141],[142],[143],[144],[145],[146],[147],[148],[149],[150],[151],[152],[153],[154],[PayorID]
	,[AcctPayableID],[AdjustorID],[PatientID],[InvoiceID],[ClaimID],[PrescriptionID],[NABP])
	SELECT [slf].[RowID],[slf].[2],[slf].[3],[slf].[4],[slf].[5],[slf].[6],[slf].[7]
	,[slf].[8],[slf].[9],[slf].[10],[slf].[11],[slf].[12],[slf].[13],[slf].[14],[slf].[15],[slf].[16],[slf].[17]
	,[slf].[18],[slf].[19],[slf].[20],[slf].[21],[slf].[22],[slf].[23],[slf].[24],[slf].[25],[slf].[26],[slf].[27]
	,[slf].[28],[slf].[29],[slf].[30],[slf].[31],[slf].[32],[slf].[33],[slf].[34],[slf].[35],[slf].[36],[slf].[37]
	,[slf].[38],[slf].[39],[slf].[40],[slf].[41],[slf].[42],[slf].[43],[slf].[44],[slf].[45],[slf].[46],[slf].[47]
	,[slf].[48],[slf].[49],[slf].[50],[slf].[51],[slf].[52],[slf].[53],[slf].[54],[slf].[55],[slf].[56],[slf].[57]
	,[slf].[58],[slf].[59],[slf].[60],[slf].[61],[slf].[62],[slf].[63],[slf].[64],[slf].[65],[slf].[66],[slf].[67]
	,[slf].[68],[slf].[69],[slf].[70],[slf].[71],[slf].[72],[slf].[73],[slf].[74],[slf].[75],[slf].[76],[slf].[77]
	,[slf].[78],[slf].[79],[slf].[80],[slf].[81],[slf].[82],[slf].[83],[slf].[84],[slf].[85],[slf].[86],[slf].[87]
	,[slf].[88],[slf].[89],[slf].[90],[slf].[91],[slf].[92],[slf].[93],[slf].[94],[slf].[95],[slf].[96],[slf].[97]
	,[slf].[98],[slf].[99],[slf].[100],[slf].[101],[slf].[102],[slf].[103],[slf].[104],[slf].[105],[slf].[106]
	,[slf].[107],[slf].[108],[slf].[109],[slf].[110],[slf].[111],[slf].[112],[slf].[113],[slf].[114],[slf].[115]
	,[slf].[116],[slf].[117],[slf].[118],[slf].[119],[slf].[120],[slf].[121],[slf].[122],[slf].[123],[slf].[124]
	,[slf].[125],[slf].[126],[slf].[127],[slf].[128],[slf].[129],[slf].[130],[slf].[131],[slf].[132],[slf].[133]
	,[slf].[134],[slf].[135],[slf].[136],[slf].[137],[slf].[138],[slf].[139],[slf].[140],[slf].[141],[slf].[142]
	,[slf].[143],[slf].[144],[slf].[145],[slf].[146],[slf].[147],[slf].[148],[slf].[149],[slf].[150],[slf].[151]
	,[slf].[152],[slf].[153],[slf].[154],[slf].[PayorID],[slf].[AcctPayableID],[slf].[AdjustorID],[slf].[PatientID]
	,[slf].[InvoiceID],[slf].[ClaimID],[slf].[PrescriptionID],[slf].NABP
	FROM [etl].[StagedLakerFile] AS [slf]
	LEFT JOIN [etl].[StagedLakerFileBackup] AS [slfb] ON [slfb].[RowID] = [slf].[RowID]
	WHERE [slfb].[RowID] IS NULL

	DECLARE @RowCountCheck INT, @NewRowsImported INT, @UpdatedRowCount INT
	DECLARE @UTCNow DATETIME2 = SYSUTCDATETIME()
	/********************************************************************************************
	Import New Payors
	********************************************************************************************/
	CREATE TABLE #Payor ([42] [varchar](8000) NULL,[43] [varchar](8000) NULL,[44] [varchar](8000) NULL,[45] 
			[varchar](8000) NULL,[46] [varchar](8000) NULL,[StateID] [int] NULL,[48] [varchar](8000) NULL,[49] [varchar](8000) NULL,[50]
			[varchar](8000) NULL, RowNumber INT NOT NULL, DenseRank INT NOT NULL, ETLRowID VARCHAR(50) NOT NULL)
	INSERT [#Payor] ([42],[43],[44],[45],[46],[StateID],[48],[49],[50],[RowNumber],[DenseRank],[ETLRowID])
	SELECT [s].[42], [s].[43],[s].[44],[s].[45],[s].[46],[us].[StateID],s.[48],s.[49],s.[50]
			,ROW_NUMBER() OVER (PARTITION BY [s].[42], [s].[43],[s].[44],[s].[45],[s].[46],[us].[StateID],s.[48],s.[49],s.[50] ORDER BY [s].[42] ASC) RowNumber
			,DENSE_RANK() OVER (ORDER BY [s].[42], [s].[43],[s].[44],[s].[45],[s].[46],[us].[StateID],s.[48],s.[49],s.[50] ASC) DenseRank, [s].[RowID]
	FROM	[#New] AS [s]
			LEFT JOIN [dbo].[UsState] AS [us] ON [us].[StateCode] = [s].[47]
			LEFT JOIN [dbo].[Payor] AS [p] ON [p].[GroupName] = [s].[42]
	WHERE	[p].[PayorID] IS NULL
	SET @NewRowsImported = @@ROWCOUNT

	-- Actual Payor Import
	INSERT [dbo].[Payor] ([GroupName],[BillToName],[BillToAddress1],[BillToAddress2],[BillToCity],[BillToStateID],[BillToPostalCode],[PhoneNumber],[FaxNumber],[ETLRowID],[CreatedOnUTC],[UpdatedOnUTC])
	SELECT [42],[43],[44],[45],[46],[StateID],[48],[49],[50],[ETLRowID],@UTCNow,@UTCNow
	FROM [#Payor] AS [p]
	WHERE [RowNumber] = 1
	ORDER BY [42] ASC
	SET @RowCountCheck = @@ROWCOUNT;

	IF (SELECT COUNT(*) FROM [dbo].[Payor] AS [p] WHERE [p].[CreatedOnUTC] = @UTCNow) != @RowCountCheck
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'The Payor count QA check failed', 16, 1) WITH NOWAIT
			RETURN
		END

	UPDATE s SET [s].[PayorID] = [pi].[PayorID]
	FROM   #New AS s WITH (TABLOCKX)
		   INNER JOIN dbo.Payor AS [pi] ON [s].[42] = [pi].[GroupName]; -- This one's easier than most beacause Group Name is Unique
	SET @RowCountCheck = @@ROWCOUNT

	-- QA check that we were able to update every record
	EXEC(N'ALTER TABLE [#New] ALTER COLUMN [PayorID] INTEGER NOT NULL')

	EXEC(N'ALTER TABLE [#New] ALTER COLUMN [PayorID] INTEGER NULL')

	UPDATE	[slf] SET [slf].[PayorID] = [n].[PayorID]
	FROM	[etl].[StagedLakerFile] AS [slf]
			INNER JOIN [#New] AS [n] ON [n].[RowID] = [slf].[RowID]

	-- QA check that we were able to update every record
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN [PayorID] INTEGER NOT NULL')

	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN [PayorID] INTEGER NULL')

	-- Now deal with updating prior Payors that may have any information updated.
	CREATE TABLE #PayorUpdate (PayorID INT NOT NULL,[42] [varchar](8000) NULL,[43] [varchar](8000) NULL,
		[44] [varchar](8000) NULL,[45] [varchar](8000) NULL,
		[46] [varchar](8000) NULL,[StateID] [int] NULL,[48] [varchar](8000) NULL,[49] [varchar](8000) NULL,[50] [varchar](8000) NULL)
	INSERT [#PayorUpdate] (PayorID,[42],[43],[44],[45],[46],[StateID],[48],[49],[50])
	SELECT  [s].[PayorID],s.[42],s.[43],s.[44],s.[45],s.[46],us.[StateID],s.[48],s.[49],s.[50]
	FROM	[etl].[StagedLakerFile] AS [s]
			LEFT JOIN [dbo].[UsState] AS [us] ON [us].[StateCode] = [s].[47]
	GROUP BY [s].[PayorID],s.[42],s.[43],s.[44],s.[45],s.[46],us.[StateID],s.[48],s.[49],s.[50]

	WAITFOR DELAY '00:00:00.050' -- wait 50 milliseconds to get a new UTCNow
	SELECT @UTCNow = SYSUTCDATETIME();

	WITH UpdatedPayorsCTE AS
	(
		SELECT	[PayorID],[42],[43],[44],[45],[46],[StateID],[48],[49],[50] 
		FROM	#PayorUpdate
		EXCEPT
		SELECT	p.[PayorID],p.[GroupName],p.[BillToName],p.[BillToAddress1],p.[BillToAddress2],p.[BillToCity], us.[StateID],p.[BillToPostalCode],p.[PhoneNumber],p.[FaxNumber]
		FROM	[dbo].[Payor] AS [p] LEFT JOIN [dbo].[UsState] AS [us] ON [us].[StateID] = [p].[BillToStateID]
	
		UNION

		SELECT	p.[PayorID],p.[GroupName],p.[BillToName],p.[BillToAddress1],p.[BillToAddress2],p.[BillToCity], us.[StateID],p.[BillToPostalCode],p.[PhoneNumber],p.[FaxNumber]
		FROM	[dbo].[Payor] AS [p] LEFT JOIN [dbo].[UsState] AS [us] ON [us].[StateID] = [p].[BillToStateID]
		EXCEPT
		SELECT	[PayorID],[42],[43],[44],[45],[46],[StateID],[48],[49],[50] 
		FROM	#PayorUpdate
	)
	UPDATE	p SET p.[GroupName] = s.[42],
			p.[BillToName] = s.[43],
			p.[BillToAddress1] = s.[44],
			p.[BillToAddress2] = s.[45],
			p.[BillToCity] = s.[46],
			p.[BillToStateID] = s.[StateID],
			p.[BillToPostalCode] = [48],
			p.[PhoneNumber] = [49],
			p.[FaxNumber] = [50],
			p.[UpdatedOnUTC] = @UTCNow
	FROM	[UpdatedPayorsCTE] AS s INNER JOIN [dbo].[Payor] AS [p] ON [p].[PayorID] = [s].[PayorID]
	SET @UpdatedRowCount = @@ROWCOUNT

	IF @UpdatedRowCount != (SELECT COUNT(*) FROM [dbo].[Payor] AS [p] WHERE [p].[UpdatedOnUTC] = @UTCNow)
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The QA check for the count of rows updated, and the count of Updated Payor Records didn''t match', 16, 1) WITH NOWAIT
			RETURN
		END

	/********************************************************************************************
	Import New Adjustors
	********************************************************************************************/
	WAITFOR DELAY '00:00:00.050' -- wait 50 milliseconds to get a new UTCNow
	SELECT @UTCNow = SYSUTCDATETIME();
	CREATE TABLE #Adjustor (PayorID INT NOT NULL, AdjustorName VARCHAR(255) NOT NULL, RowNumber INT NOT NULL, 
								DenseRank INT NOT NULL, ETLRowID VARCHAR(50) NOT NULL)
	INSERT [#Adjustor] ([PayorID],[AdjustorName],[RowNumber],[DenseRank],[ETLRowID])
	SELECT s.[PayorID],s.[21],ROW_NUMBER() OVER (PARTITION BY s.[PayorID], s.[21] ORDER BY s.[21] ASC) RowNumber,
	DENSE_RANK() OVER (ORDER BY s.[PayorID], s.[21] ASC) DenseRank, [s].[RowID]
	FROM [#New] AS s LEFT JOIN [dbo].[Adjustor] AS [a] ON s.[21] = [a].[AdjustorName]
	WHERE s.[21] IS NOT NULL
		  AND [a].[AdjustorID] IS NULL
	ORDER BY s.[21]
	SET @NewRowsImported = @@ROWCOUNT

	-- Actual Adjustor Import
	INSERT [dbo].[Adjustor] ([PayorID], [AdjustorName], [ETLRowID], [CreatedOnUTC], [UpdatedOnUTC])
	SELECT a.[PayorID], a.[AdjustorName], [a].[ETLRowID], @UTCNow, @UTCNow
	FROM [#Adjustor] AS [a]
	WHERE [a].[RowNumber] = 1
	SET @RowCountCheck = @@ROWCOUNT;

	IF (SELECT COUNT(*) FROM [dbo].[Adjustor] AS [p] WHERE [p].[CreatedOnUTC] = @UTCNow) != @RowCountCheck
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'The Adjustor count QA check failed', 16, 1) WITH NOWAIT
			RETURN
		END

	-- Update the temp table with new Ajustor ID's
	UPDATE s SET [s].[AdjustorID] = [a].[AdjustorID]
	FROM   #New AS s WITH (TABLOCKX)
		   INNER JOIN dbo.[Adjustor] AS [a] ON [s].[21] = [a].[AdjustorName]
	WHERE  [a].[PayorID] = [s].[PayorID]
	SET @RowCountCheck = @@ROWCOUNT

	-- Update the StagedLakerFile with #New's Adjustor ID's
	UPDATE	[slf] SET [slf].[AdjustorID] = [n].[AdjustorID]
	FROM	[etl].[StagedLakerFile] AS [slf]
			INNER JOIN [#New] AS [n] ON [n].[RowID] = [slf].[RowID]
	WHERE	[n].[AdjustorID] IS NOT NULL
	IF @RowCountCheck != @@ROWCOUNT
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The Updated Ajustor ID''s in the StagedLakerFile does not match the count of updated Adjustor ID''s in #New', 16, 1) WITH NOWAIT
			RETURN
		END

	-- Adjustor's are unique, in that the matching criteria that we normally would use for an 
	-- update is the one and only field that we have. Therefore there will be no update.

	/********************************************************************************************
	Import New Patients
	********************************************************************************************/
	WAITFOR DELAY '00:00:00.050' -- wait 50 milliseconds to get a new UTCNow
	SELECT @UTCNow = SYSUTCDATETIME();

	CREATE TABLE #Patient ([DateOfBirth] [varchar] (8000) NULL,[LastName] [varchar] (8000) NULL,[FirstName] [varchar] (8000) NULL,
	[Address1] [varchar] (8000) NULL,[Address2] [varchar] (8000) NULL,[City] [varchar] (8000) NULL,
	[StateID] [int] NULL,[PostalCode] [varchar] (8000) NULL,[PhoneNumber] [varchar] (8000) NULL,
	[GenderID] [int] NULL,[RowNumber] INT NOT NULL,DenseRank INT NOT NULL,[ETLRowID] VARCHAR(50) NOT NULL)
	INSERT [#Patient] ([DateOfBirth],[LastName],[FirstName],[Address1],[Address2],[City],[StateID],[PostalCode]
									,[PhoneNumber],[GenderID],[RowNumber],[DenseRank],[ETLRowID])
	SELECT n.[20], n.[10], n.[11], n.[12],n.[13],n.[14], [us].[StateID],n.[16],n.[17],g.[GenderID]
	,ROW_NUMBER() OVER (PARTITION BY n.[20], n.[10], n.[11], n.[12],n.[13],n.[14], n.[15],n.[16],n.[17],n.[18] ORDER BY n.[20]) RowID
	,DENSE_RANK() OVER (ORDER BY n.[20], n.[10], n.[11], n.[12],n.[13],n.[14], [us].[StateID],n.[16],n.[17],g.[GenderID]) DenseRank, n.[RowID]
	FROM #New AS n
	LEFT JOIN [dbo].[UsState] AS [us] ON n.[15] = [us].[StateCode]
	LEFT JOIN [dbo].[Gender] AS [g] ON g.[GenderCode] = n.[18]
	WHERE n.[10] IS NOT NULL

	-- Actual Patient Import
	INSERT [dbo].[Patient] ([DateOfBirth],[LastName],[FirstName], [Address1],[Address2],[City],[StateID],
							[PostalCode],[PhoneNumber],[GenderID],[ETLRowID],[CreatedOnUTC],[UpdatedOnUTC])
	SELECT [p].[DateOfBirth], [p].[LastName], [p].[FirstName], [p].[Address1], [p].[Address2], [p].[City], [p].[StateID]
			, [p].[PostalCode], [p].[PhoneNumber], [p].[GenderID], [p].[ETLRowID], @UTCNow, @UTCNow
	FROM [#Patient] AS p LEFT JOIN [dbo].[Patient] AS [p2] ON [p2].[DateOfBirth] = [p].[DateOfBirth]
		AND [p2].[LastName] = [p].[LastName]
		AND [p2].[FirstName] = [p].[FirstName]
	WHERE [p].[RowNumber] = 1
		  AND [p2].[PatientID] IS NULL
	SET @RowCountCheck = @@ROWCOUNT

	IF (SELECT COUNT(*) FROM [dbo].[Patient] AS [p] WHERE [p].[CreatedOnUTC] = @UTCNow) != @RowCountCheck
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'The Patient count QA check failed', 16, 1) WITH NOWAIT
			RETURN
		END

	-- Update the temp table with new Patient ID's
	UPDATE s SET [s].[PatientID] = [pat].[PatientID]
	FROM   #New AS s WITH (TABLOCKX)
		   CROSS APPLY (   SELECT TOP 1 [p].[PatientID]
						   FROM   [dbo].[Patient] AS [p]
						   WHERE  [s].[10] = [p].[LastName]
								  AND [s].[11] = [p].[FirstName]
								  AND [s].[20] = [p].[DateOfBirth]
						   ORDER BY [p].[CreatedOnUTC] DESC
					   ) AS pat
	WHERE	1 = 1 -- Override SQL Prompt warning (normally a bad practice)
	SET @RowCountCheck = @@ROWCOUNT

	IF (SELECT COUNT(*) FROM [#New]) != @RowCountCheck
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'The Patients updated does not match the total records in #New', 16, 1) WITH NOWAIT
			RETURN
		END

	-- Update the StagedLakerFile with #New's Adjustor ID's
	UPDATE	[slf] SET [slf].[PatientID] = [n].[PatientID]
	FROM	[etl].[StagedLakerFile] AS [slf]
			left JOIN [#New] AS [n] ON [n].[RowID] = [slf].[RowID]
	WHERE [slf].[PatientID] IS NULL
	IF @RowCountCheck != @@ROWCOUNT
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The Updated Patient ID''s in the StagedLakerFile does not match the count of updated Patient ID''s in #New', 16, 1) WITH NOWAIT
			RETURN
		END

	-- QA check that we were able to update every record
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN [PatientID] INT NOT NULL')
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN [PatientID] INT NULL')

	-- Now deal with updating prior Patients that may have any information updated.
	CREATE TABLE #PatientUpdate (PatientID INT NOT NULL,[20] VARCHAR(8000) NULL,[10] VARCHAR(8000) NULL,[11] VARCHAR(8000),
	[12] VARCHAR(8000) NULL,[13] VARCHAR(8000) NULL,[14] VARCHAR(8000),StateID INT NULL,[16] VARCHAR(8000) NULL,[17] VARCHAR(8000) NULL,
	GenderID INT NULL)
	INSERT [#PatientUpdate] ([PatientID],[20],[10],[11],[12],[13],[14],[StateID],[16],[17],[GenderID])
	SELECT n.[PatientID], n.[20], n.[10], n.[11], n.[12],n.[13],n.[14], [us].[StateID],n.[16],n.[17],g.[GenderID]
	FROM [etl].[StagedLakerFile] AS n
	LEFT JOIN [dbo].[UsState] AS [us] ON n.[15] = [us].[StateCode]
	LEFT JOIN [dbo].[Gender] AS [g] ON g.[GenderCode] = n.[18]
	GROUP BY [n].[PatientID], n.[20], n.[10], n.[11], n.[12],n.[13],n.[14], [us].[StateID],n.[16],n.[17],g.[GenderID];

	-- We have an issue with duplicates that we need to exclude from any potential update until these are fixed.
	CREATE TABLE #PatientDupes (DateOfBirth VARCHAR(8000) NOT NULL,LastName VARCHAR(8000) NOT NULL,FirstName VARCHAR(8000) NOT NULL)
	INSERT [#PatientDupes] ([DateOfBirth],[LastName],[FirstName])
	SELECT	[20],[10],[11]
	FROM	#PatientUpdate
	GROUP BY [20],[10],[11]
	HAVING COUNT(*) > 1

	WAITFOR DELAY '00:00:00.050' -- wait 50 milliseconds to get a new UTCNow
	SELECT @UTCNow = SYSUTCDATETIME();

	WITH UpdatedPatientsCTE AS
	(
		SELECT	[PatientID],[20],[10],[11],[12],[13],[14],[StateID],[16],[17],[GenderID]
		FROM	#PatientUpdate pu
		WHERE	NOT EXISTS (SELECT * 
							FROM #PatientDupes pd
							WHERE pd.[DateOfBirth] = [pu].[20]
							AND pd.[LastName] = pu.[10]
							AND pd.[FirstName] = pu.[11])
		EXCEPT
		SELECT	[PatientID],[DateOfBirth],[LastName],[FirstName],[Address1],[Address2],[City],[StateID],[PostalCode],[PhoneNumber],[GenderID]
		FROM	[dbo].[Patient] AS [p]
	
		UNION

		SELECT	[PatientID],[20],[10],[11],[12],[13],[14],[StateID],[16],[17],[GenderID]
		FROM	#PatientUpdate pu
		WHERE	NOT EXISTS (SELECT * 
							FROM #PatientDupes pd
							WHERE pd.[DateOfBirth] = [pu].[20]
							AND pd.[LastName] = pu.[10]
							AND pd.[FirstName] = pu.[11])
		EXCEPT
		SELECT	[PatientID],[20],[10],[11],[12],[13],[14],[StateID],[16],[17],[GenderID]
		FROM	#PatientUpdate
	)
	UPDATE	p SET [p].[DateOfBirth] = [s].[20]
				, [p].[LastName] = [s].[10]
				, [p].[FirstName] = [s].[11]
				, [p].[Address1] = [s].[12]
				, [p].[Address2] = [s].[13]
				, [p].[City] = [s].[14]
				, [p].[StateID] = [s].[StateID]
				, [p].[PostalCode] = [s].[16]
				, [p].[PhoneNumber] = [s].[17]
				, [p].[GenderID] = [s].[GenderID]
				, [p].[UpdatedOnUTC] = @UTCNow
	FROM	UpdatedPatientsCTE AS s INNER JOIN [dbo].[Patient] AS [p] ON [p].[PatientID] = [s].[PatientID]
	SET @UpdatedRowCount = @@ROWCOUNT

	IF @UpdatedRowCount != (SELECT COUNT(*) FROM [dbo].[Patient] AS [p] WHERE [p].[UpdatedOnUTC] = @UTCNow)
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The QA check for the count of rows updated, and the count of Updated Patient Records didn''t match', 16, 1) WITH NOWAIT
			RETURN
		END

	/********************************************************************************************
	Import New Claims
	********************************************************************************************/
	WAITFOR DELAY '00:00:00.050' -- wait 50 milliseconds to get a new UTCNow
	SELECT @UTCNow = SYSUTCDATETIME();
	-- Ensure that we are chalk full of Patient ID's and 
	IF EXISTS
	(
		SELECT * FROM [etl].[StagedLakerFile] AS [slf] WHERE [slf].[PatientID] IS NULL
	)
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The PatientID''s  in the etl.StagedLakerFile are not all NULL. They should have all been updated by this point', 16, 1) WITH NOWAIT
			RETURN
		END
	IF EXISTS
	(
		SELECT * FROM [etl].[StagedLakerFile] AS [slf] WHERE [slf].[PayorID] IS NULL
	)
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The PayorID''s  in the etl.StagedLakerFile are not all NULL. They should have all been updated by this point', 16, 1) WITH NOWAIT
			RETURN
		END

	CREATE TABLE #Claim ([8] [varchar](8000) NULL,[9] [varchar](8000) NULL,[19] [varchar](8000) NULL,
	[22] [varchar](8000) NULL,[25] [varchar](8000) NULL,[125] [varchar](8000) NULL,[PatientID] [int] NULL,
	[PayorID] [int] NOT NULL,[AdjustorID] [int] NULL,[RowNumber] [int] NULL,[DenseRank] [int] NULL,[ETLRowID] VARCHAR(50) NOT NULL)
	INSERT INTO [#Claim] ([8],[9],[19],[22],[25],[125],[PatientID],[PayorID],[AdjustorID],[RowNumber],[DenseRank],[ETLRowID])
	SELECT [s].[8],[s].[9],[s].[19],[s].[22],[s].[25],[s].[125],s.[PatientID],s.[PayorID],s.[AdjustorID]
	,ROW_NUMBER() OVER (PARTITION BY [s].[8],[s].[9],[s].[19],[s].[22],[s].[25],[s].[125],s.[PatientID],s.[PayorID],s.[AdjustorID] ORDER BY [s].[8]) RowNumber
	,DENSE_RANK() OVER (ORDER BY [s].[8],[s].[9],[s].[19],[s].[22],[s].[25],[s].[125],s.[PatientID],s.[PayorID],s.[AdjustorID]) DenseRank, s.[RowID]
	FROM   [#New] AS s LEFT JOIN [dbo].[Claim] AS [c] ON [c].[PersonCode] = [s].[9]
		   AND [c].[ClaimNumber] = [s].[8]
	WHERE  [c].[ClaimID] IS NULL
	SET @NewRowsImported = @@ROWCOUNT


	-- Actual Claims Import
	INSERT [dbo].[Claim] ([ClaimNumber],[PersonCode],[DateOfInjury],[RelationCode],[PreviousClaimNumber],
	[TermDate],[PatientID],[PayorID],[AdjusterID],[ETLRowID],[IsFirstParty],[UpdatedOnUTC],[CreatedOnUTC])
	SELECT [s].[8],[s].[9],[s].[19],[s].[22],[s].[25],[s].[125],s.[PatientID],s.[PayorID],s.[AdjustorID],s.[ETLRowID],1,@UTCNow,@UTCNow
	FROM #Claim  AS [s]
	WHERE [RowNumber] = 1
	SET @RowCountCheck = @@ROWCOUNT;

	IF (SELECT COUNT(*) FROM [dbo].[Claim] AS [c] WHERE [c].[CreatedOnUTC] = @UTCNow) != @RowCountCheck
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The new Claim records imported count does not match the rows affected for inserted Claims', 16, 1) WITH NOWAIT
			RETURN
		END

	UPDATE s SET [s].[ClaimID] = [c].[ClaimID]
	FROM   #New AS s WITH ( TABLOCKX )
		   CROSS APPLY (   SELECT TOP 1 c.[ClaimID], c.ClaimNumber, c.PersonCode
						   FROM   [dbo].[Claim] AS [c]
						   where  [s].[8] = [c].[ClaimNumber]
								  AND [c].[PersonCode] = [s].[9]
								  AND [c].[PatientID] = [s].[PatientID]
						   ORDER BY c.[CreatedOnUTC] DESC
					   ) AS c
	WHERE 1 = 1 -- Moving around SQL Prompt (normally a bad practice)
	SET @RowCountCheck = @@ROWCOUNT

	-- QA check that we were able to update every record
	EXEC(N'ALTER TABLE [#New] ALTER COLUMN [ClaimID] INTEGER NOT NULL')
	EXEC(N'ALTER TABLE [#New] ALTER COLUMN [ClaimID] INTEGER NULL')

	-- Update the StagedLakerFile with #New's Claim ID's
	UPDATE	[slf] SET [slf].[ClaimID] = [n].[ClaimID]
	FROM	[etl].[StagedLakerFile] AS [slf]
			INNER JOIN [#New] AS [n] ON [n].[RowID] = [slf].[RowID]
	IF @RowCountCheck != @@ROWCOUNT
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The Updated Claim ID''s in the StagedLakerFile does not match the count of updated Claim ID''s in #New', 16, 1) WITH NOWAIT
			RETURN
		END

	-- QA check that we were able to update every record in StagedLakerFile
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN [ClaimID] INTEGER NOT NULL')
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN [ClaimID] INTEGER NULL')

	CREATE TABLE #ClaimUpdate (ClaimID INT NOT NULL, [8] [varchar](8000) NULL,[9] [varchar](8000) NULL,[19] [varchar](8000) NULL,
	[22] [varchar](8000) NULL,[25] [varchar](8000) NULL,[125] [varchar](8000) NULL,[PatientID] [int] NULL,
	[PayorID] [int] NOT NULL,[AdjustorID] [int] NULL)
	INSERT INTO #ClaimUpdate ([ClaimID],[8],[9],[19],[22],[25],[125],[PatientID],[PayorID],[AdjustorID])
	SELECT [s].[ClaimID],[s].[8],[s].[9],[s].[19],[s].[22]
	,[s].[25],[s].[125],[s].[PatientID],[s].[PayorID],[s].[AdjustorID]
	FROM   [etl].[StagedLakerFile] AS s

	WAITFOR DELAY '00:00:00.050' -- wait 50 milliseconds to get a new UTCNow
	SELECT @UTCNow = SYSUTCDATETIME();

	WITH UpdatedClaimsCTE AS
	(
		SELECT	[ClaimID],[8],[9],[19],[22],[25],[125],[PatientID],[PayorID],[AdjustorID]
		FROM	#ClaimUpdate c
		EXCEPT
		SELECT	[c].[ClaimID],[ClaimNumber],[PersonCode],[DateOfInjury],[RelationCode],[PreviousClaimNumber],[TermDate],[PatientID],[PayorID],[AdjusterID]
		FROM	[dbo].[Claim] AS [c]
		
		UNION
	
		SELECT	[c].[ClaimID],[ClaimNumber],[PersonCode],[DateOfInjury],[RelationCode],[PreviousClaimNumber],[TermDate],[PatientID],[PayorID],[AdjusterID]
		FROM	[dbo].[Claim] AS [c]
		EXCEPT
		SELECT	[ClaimID],[8],[9],[19],[22],[25],[125],[PatientID],[PayorID],[AdjustorID]
		FROM	#ClaimUpdate c
	)
	UPDATE	[c] SET [c].[ClaimNumber] = [ct].[8]
				, [c].[PersonCode] = [ct].[9]
				, [c].[DateOfInjury] = [ct].[19]
				, [c].[RelationCode] = [ct].[22]
				, [c].[PreviousClaimNumber] = [ct].[25]
				, [c].[TermDate] = [ct].[125]
				, [c].[PatientID] = [ct].[PatientID]
				, [c].[PayorID] = [ct].[PayorID]
				, [c].[AdjusterID] = [ct].[AdjustorID]
				, [c].[UpdatedOnUTC] = @UTCNow
	FROM	[UpdatedClaimsCTE] AS [ct] INNER JOIN [dbo].[Claim] AS [c] ON [ct].[ClaimID] = [c].[ClaimID]
	SET @UpdatedRowCount = @@ROWCOUNT

	IF @UpdatedRowCount != (SELECT COUNT(*) FROM [dbo].[Claim] AS [c] WHERE [c].[UpdatedOnUTC] = @UTCNow)
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The QA check for the count of rows updated, and the count of Updated Patient Records didn''t match', 16, 1) WITH NOWAIT
			RETURN
		END

	/********************************************************************************************
	Import New Invoices
	********************************************************************************************/
	WAITFOR DELAY '00:00:00.050' -- wait 50 milliseconds to get a new UTCNow
	SELECT @UTCNow = SYSUTCDATETIME();
	DECLARE @SQL NVARCHAR(4000)
	SET @SQL = N'ALTER TABLE dbo.Invoice ADD PrescriptionID INT NULL';
	EXECUTE sys.sp_executesql @SQL;

	SET @SQL =
	N'WITH MissingInvoicesCTE ( InvoiceNumber, InvoiceDate, RowID, PrescriptionID )
	AS ( SELECT slf.[4]
				, slf.[5]
				, slf.RowID
				, slf.PrescriptionID
			FROM  etl.StagedLakerFile AS slf
			WHERE slf.[4] IS NOT NULL
				AND slf.[5] IS NOT NULL
				AND slf.PrescriptionID IS NOT NULL
		)
	INSERT dbo.Invoice (InvoiceNumber, InvoiceDate, Amount, ETLRowID, PrescriptionID)
	SELECT c.InvoiceNumber
			, c.InvoiceDate
			, 0
			, c.RowID
			, c.PrescriptionID
	FROM   MissingInvoicesCTE AS c
			LEFT JOIN dbo.Invoice AS i ON i.InvoiceNumber = c.InvoiceNumber AND i.InvoiceDate = c.InvoiceDate
			LEFT JOIN dbo.Prescription AS p ON p.InvoiceID = i.InvoiceID
	WHERE  i.InvoiceID IS NULL
			AND p.PrescriptionID IS NULL'
	EXECUTE sys.sp_executesql @SQL

	SET @SQL= N'UPDATE op SET op.InvoiceID = iip.InvoiceID
					FROM dbo.Prescription AS op
					INNER JOIN
						(SELECT i.PrescriptionID, i.InvoiceID
						 FROM dbo.Invoice AS i
							LEFT JOIN dbo.Prescription AS p ON p.InvoiceID=i.InvoiceID
								AND p.PrescriptionID=i.PrescriptionID
						 WHERE i.PrescriptionID IS NOT NULL AND p.PrescriptionID IS NULL) AS iip 
							ON iip.PrescriptionID=op.PrescriptionID';
	EXECUTE sys.sp_executesql @SQL;

	SET @SQL = N'ALTER TABLE dbo.Invoice DROP COLUMN PrescriptionID';
	EXECUTE sys.sp_executesql @SQL

	/********************************************************************************************
	Begin Pharmacy Section
	********************************************************************************************/
	WAITFOR DELAY '00:00:00.050' -- wait 50 milliseconds to get a new UTCNow
	SELECT @UTCNow = SYSUTCDATETIME();

	CREATE TABLE #Pharmacy
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
	INSERT INTO [#Pharmacy] ([89],[90],[91],[92],[93],[94],[StateID],[96],[97],[RowNumber],[DenseRank],ETLRowID)
	SELECT [s].[89],[s].[90],[s].[91],[s].[92],[s].[93],[s].[94],[us].[StateID],[s].[96],[s].[97]
		,ROW_NUMBER() OVER (PARTITION BY [s].[89],[s].[90],[s].[91],[s].[92],[s].[93],[s].[94],[us].[StateID],[s].[96],[s].[97] ORDER BY [s].[89]) RowID
		,DENSE_RANK() OVER (ORDER BY [s].[89],[s].[90],[s].[91],[s].[92],[s].[93],[s].[94],[us].[StateID],[s].[96],[s].[97]) DenseRank, 
		[s].[RowID]
	FROM [etl].[#New] AS [s]
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

	INSERT INTO [dbo].[Pharmacy] ([NABP],[NPI],[PharmacyName],[Address1],[Address2],[City],[StateID],[PostalCode],[DispType], [ETLRowID], [CreatedOnUTC], [UpdatedOnUTC])
	SELECT	[p].[89],[p].[90],[p].[91],[p].[92],[p].[93],[p].[94],[p].[StateID],[p].[96],util.udfTrimLeadingZeros([p].[97]),[p].[ETLRowID], @UTCNow, @UTCNow
	FROM	[#Pharmacy] AS [p] LEFT JOIN [dbo].[Pharmacy] AS ph ON [ph].[NABP] = [p].[89]
	WHERE	[p].[RowNumber] = 1 AND ph.[NABP] IS NULL
	SET @RowCountCheck = @@ROWCOUNT

	IF (SELECT COUNT(*) FROM [dbo].[Pharmacy] AS [p] WHERE [p].[CreatedOnUTC] = @UTCNow) != @RowCountCheck
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. Could not import the same amount of Pharmacies that we should have', 16, 1) WITH NOWAIT
			RETURN
		END
		
	UPDATE n SET n.[NABP] = p.[NABP]
	FROM   [#New] AS [n]
		   INNER JOIN [dbo].[Pharmacy] AS [p] ON [p].[NABP] = [n].[89]
	SET @RowCountCheck = @@ROWCOUNT

	UPDATE [slf] SET [slf].[NABP] = [n].[NABP]
	FROM   [etl].[StagedLakerFile] AS [slf]
		   INNER JOIN [#New] AS [n] ON [n].[RowID] = [slf].[RowID]
	IF @@ROWCOUNT != @RowCountCheck
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The same number of #New Pharmacy NABP''s updated does not match the number of StagedLakerFile Pharmacy NABP''s Updated.', 16, 1) WITH NOWAIT
			RETURN
		END
	
	CREATE TABLE #PharmacyUpdate
	(
	[NABP] [varchar] (7) NOT NULL,
	[NPI] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PharmacyName] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Address1] [varchar] (55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Address2] [varchar] (55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[City] [varchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[StateID] [int] NULL,
	[PostalCode] [varchar] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DispType] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	RowID VARCHAR(50) NOT NULL)
	INSERT [#PharmacyUpdate] ([NABP],[NPI],[PharmacyName],[Address1],[Address2],[City],[StateID],[PostalCode],[DispType],[RowID])
	SELECT  [p].[89],[p].[90],[p].[91],[p].[92],[p].[93],[p].[94],[us].[StateID],[p].[96],util.udfTrimLeadingZeros([p].[97]),[p].[RowID]
	FROM	[etl].[StagedLakerFile] AS [p] LEFT JOIN [dbo].[UsState] AS [us] ON [us].[StateCode] = [p].[95]
	WHERE	[p].NABP IS NOT NULL

	WAITFOR DELAY '00:00:00.050' -- wait 50 milliseconds to get a new UTCNow
	SELECT @UTCNow = SYSUTCDATETIME();
	
	WITH PharmaciesUpdateCTE AS
	(
		SELECT	[p].[NABP],[p].[NPI],[p].[PharmacyName],[p].[Address1],[p].[Address2],[p].[City],[p].[StateID],[p].[PostalCode],p.[DispType]
		FROM	[dbo].[Pharmacy] AS [p]
		EXCEPT
		SELECT	[p].[NABP],[p].[NPI],[p].[PharmacyName],[p].[Address1],[p].[Address2],[p].[City],[p].[StateID],[p].[PostalCode],[p].[DispType]
		FROM	[#PharmacyUpdate] AS [p]

		UNION
    
		SELECT	[p].[NABP],[p].[NPI],[p].[PharmacyName],[p].[Address1],[p].[Address2],[p].[City],[p].[StateID],[p].[PostalCode],[p].[DispType]
		FROM	[#PharmacyUpdate] AS [p]
		EXCEPT
		SELECT	[p].[NABP],[p].[NPI],[p].[PharmacyName],[p].[Address1],[p].[Address2],[p].[City],[p].[StateID],[p].[PostalCode],p.[DispType]
		FROM	[dbo].[Pharmacy] AS [p]
	)
	UPDATE [p]
	SET    [p].[NABP] = [c].[NABP]
		 , [p].[NPI] = [c].[NPI]
		 , [p].[PharmacyName] = [c].[PharmacyName]
		 , [p].[Address1] = [c].[Address1]
		 , [p].[Address2] = [c].[Address2]
		 , [p].[City] = [c].[City]
		 , [p].[StateID] = [c].[StateID]
		 , [p].[PostalCode] = [c].[PostalCode]
		 , [p].[DispType] = [c].[DispType]
		 , [p].[UpdatedOnUTC] = @UTCNow
	FROM   [dbo].[Pharmacy] AS [p]
		   INNER JOIN [PharmaciesUpdateCTE] AS [c] ON [p].NABP = [c].NABP
	SET @RowCountCheck = @@ROWCOUNT
	
	IF @RowCountCheck != (SELECT COUNT(*) FROM [dbo].[Pharmacy] AS [p] WHERE [p].[UpdatedOnUTC] = @UTCNow)
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The Updated Pharmacies does not match the actual number of records in the database where Pharmacies were updated', 16, 1) WITH NOWAIT
			RETURN
		END


	/********************************************************************************************
	Import New Prescriptions
	********************************************************************************************/
	WAITFOR DELAY '00:00:00.050' -- wait 50 milliseconds to get a new UTCNow
	SELECT @UTCNow = SYSUTCDATETIME();

	-- Actual Prescription Import
	INSERT INTO [dbo].[Prescription] ([ClaimID],[DateSubmitted],[RxNumber],[DateFilled],[RefillDate],[RefillNumber],[MONY],
				[DAW],[Quantity],[DaySupply],[NDC],[LabelName],[GPI],[BillIngrCost],[BillDispFee],[BilledTax],[BilledCopay],
				[BilledAmount],[PayIngrCost],[PayDispFee],[PayTax],[PayableAmount],[DEA],[PrescriberNPI],[AWPUnit],[Usual],
				[Compound],[Strength],[GPIGenName],[TheraClass],[Generic],[PharmacyNABP],[Prescriber],[TransactionType],[TranID],
				[InvoiceID],[ETLRowID],[CreatedOnUTC],[UpdatedOnUTC])
	SELECT	[n].[ClaimID],[n].[3],[n].[60],[n].[61],[n].[62],[n].[63]
			,[n].[64],[n].[65],[n].[66],[n].[67],[n].[68],[n].[69],[n].[70],[n].[71],[n].[72],[n].[73]
			,[n].[74],[n].[75],[n].[76],[n].[77],[n].[78],[n].[79],[n].[80],[n].[81],[n].[105],[n].[122],
			CAST(ISNULL([n].[123],'') AS VARCHAR(8000)) -- TODO: Remove at some point, this column is non-nullable.
			,[n].[137],[n].[143],[n].[146]
			,'Y',[89],[82],CAST('' AS VARCHAR(8000)),CAST('' AS VARCHAR(8000)),[n].[InvoiceID],[n].[RowID],@UTCNow,@UTCNow -- Question for Adam: this isn't in the mapping file. [Generic],[PharmacyNABP]
	FROM	[#New] AS [n]
	SET @RowCountCheck = @@ROWCOUNT;

	IF (SELECT COUNT(*) FROM [dbo].[Prescription] AS [p] WHERE [p].[CreatedOnUTC] = @UTCNow) != @RowCountCheck
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The new Prescription records imported count does not match the rows affected for inserted Prescriptions', 16, 1) WITH NOWAIT
			RETURN
		END

	UPDATE s SET [s].PrescriptionID = p.PrescriptionID
	FROM   #New AS s WITH ( TABLOCKX )
		   INNER JOIN [dbo].Prescription AS p ON p.ETLRowID = s.[RowID]
	IF @RowCountCheck != @@ROWCOUNT
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. Something went wrong in updating the #New file, PrescriptionID''s.', 16, 1) WITH NOWAIT
			RETURN
		END

	UPDATE	[slf] SET [slf].[PrescriptionID] = [n].[PrescriptionID]
	FROM	[etl].[StagedLakerFile] AS [slf]
			INNER JOIN [#New] AS [n] ON [n].[RowID] = [slf].[RowID]
	IF @RowCountCheck != @@ROWCOUNT
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The Updated Prescription ID''s in the StagedLakerFile does not match the count of updated Prescription ID''s in #New', 16, 1) WITH NOWAIT
			RETURN
		END

	-- QA Checks
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN [PrescriptionID] INTEGER NOT NULL')
	-- Set it back
	EXEC(N'ALTER TABLE [etl].[StagedLakerFile] ALTER COLUMN [PrescriptionID] INTEGER NULL')

	WAITFOR DELAY '00:00:00.050' -- wait 50 milliseconds to get a new UTCNow
	SELECT @UTCNow = SYSUTCDATETIME();

	CREATE TABLE #PrescriptionUpdate (
		[PrescriptionID] [int] NOT NULL,
		[ClaimID] [int] NOT NULL,
		[RxNumber] [varchar](100) NOT NULL,
		[DateSubmitted] [datetime2](7) NOT NULL,
		[DateFilled] [datetime2](7) NOT NULL,
		[LabelName] [varchar](25) NULL,
		[NDC] [varchar](11) NOT NULL,
		[Quantity] [float] NOT NULL,
		[DaySupply] [float] NOT NULL,
		[Generic] [char](1) NOT NULL,
		[PharmacyNABP] [varchar](7) NOT NULL,
		[AWPUnit] [float] NULL,
		[Usual] [decimal](18, 0) NULL,
		[Prescriber] [varchar](100) NULL,
		[PayableAmount] [money] NOT NULL,
		[BilledAmount] [money] NOT NULL,
		[TransactionType] [char](1) NOT NULL,
		[Compound] [char](1) NOT NULL,
		[TranID] [varchar](14) NOT NULL,
		[RefillDate] [date] NULL,
		[RefillNumber] [smallint] NULL,
		[MONY] [char](1) NULL,
		[DAW] [smallint] NULL,
		[GPI] [varchar](14) NULL,
		[BillIngrCost] [float] NULL,
		[BillDispFee] [float] NULL,
		[BilledTax] [float] NULL,
		[BilledCopay] [float] NULL,
		[PayIngrCost] [float] NULL,
		[PayDispFee] [float] NULL,
		[PayTax] [float] NULL,
		[DEA] [varchar](12) NULL,
		[PrescriberNPI] [varchar](12) NULL,
		[Strength] [varchar](255) NULL,
		[GPIGenName] [varchar](255) NULL,
		[TheraClass] [varchar](255) NULL,
		[InvoiceID] [int] NULL,
		[CreatedOnUTC] [datetime2](7) NOT NULL,
		[UpdatedOnUTC] [datetime2](7) NOT NULL,
		[DataVersion] [timestamp] NOT NULL,
		[ETLRowID] [varchar](50) NULL,
		[AWP]  AS ([Quantity]*[AWPUnit]),
	 CONSTRAINT [pkPrescription] PRIMARY KEY CLUSTERED 
	(
		[PrescriptionID] ASC
	))
	INSERT #PrescriptionUpdate ([PrescriptionID],[ClaimID],[DateSubmitted],[RxNumber],[DateFilled],[RefillDate],[RefillNumber],[MONY],
				[DAW],[Quantity],[DaySupply],[NDC],[LabelName],[GPI],[BillIngrCost],[BillDispFee],[BilledTax],[BilledCopay],
				[BilledAmount],[PayIngrCost],[PayDispFee],[PayTax],[PayableAmount],[DEA],[PrescriberNPI],[AWPUnit],[Usual],
				[Compound],[Strength],[GPIGenName],[TheraClass],[Generic],[PharmacyNABP],[Prescriber],[TransactionType],[TranID],
				[InvoiceID],[ETLRowID],[CreatedOnUTC],[UpdatedOnUTC])
	SELECT	[n].[PrescriptionID], [n].[ClaimID],[n].[3],[n].[60],[n].[61],[n].[62],[n].[63]
			,[n].[64],[n].[65],[n].[66],[n].[67],[n].[68],[n].[69],[n].[70],[n].[71],[n].[72],[n].[73]
			,[n].[74],[n].[75],[n].[76],[n].[77],[n].[78],[n].[79],[n].[80],[n].[81],[n].[105],[n].[122],
			CAST(ISNULL([n].[123],'') AS VARCHAR(8000)) -- TODO: Remove at some point, this column is non-nullable.
			,[n].[137],[n].[143],[n].[146]
			,'Y',[89],[82],CAST('' AS VARCHAR(8000)),CAST('' AS VARCHAR(8000)),[n].[InvoiceID],[n].[RowID],@UTCNow,@UTCNow -- Question for Adam: this isn't in the mapping file. [Generic],[PharmacyNABP]
	FROM	[etl].[StagedLakerFile] AS [n];

	WITH UpdatedPrescriptionsCTE AS
	(
		SELECT	[PrescriptionID] ,[ClaimID] ,[RxNumber] ,[DateSubmitted] ,[DateFilled] ,[LabelName] ,[NDC] ,[Quantity] ,[DaySupply] ,[Generic] ,[PharmacyNABP] ,[AWPUnit] ,[Usual] ,[Prescriber] ,[PayableAmount] ,[BilledAmount] ,[TransactionType] ,[Compound] ,[TranID] ,[RefillDate],[RefillNumber],[MONY],[DAW],[GPI],[BillIngrCost],[BillDispFee],[BilledTax],[BilledCopay],[PayIngrCost],[PayDispFee],[PayTax],[DEA],[PrescriberNPI],[Strength],[GPIGenName],[TheraClass],[InvoiceID],[ETLRowID],[AWP]
		FROM	#PrescriptionUpdate
		EXCEPT
		SELECT	[PrescriptionID] ,[ClaimID] ,[RxNumber] ,[DateSubmitted] ,[DateFilled] ,[LabelName] ,[NDC] ,[Quantity] ,[DaySupply] ,[Generic] ,[PharmacyNABP] ,[AWPUnit] ,[Usual] ,[Prescriber] ,[PayableAmount] ,[BilledAmount] ,[TransactionType] ,[Compound] ,[TranID] ,[RefillDate],[RefillNumber],[MONY],[DAW],[GPI],[BillIngrCost],[BillDispFee],[BilledTax],[BilledCopay],[PayIngrCost],[PayDispFee],[PayTax],[DEA],[PrescriberNPI],[Strength],[GPIGenName],[TheraClass],[InvoiceID],[ETLRowID],[AWP]
		FROM	[dbo].[Prescription]
	
		UNION
    
		SELECT	[PrescriptionID] ,[ClaimID] ,[RxNumber] ,[DateSubmitted] ,[DateFilled] ,[LabelName] ,[NDC] ,[Quantity] ,[DaySupply] ,[Generic] ,[PharmacyNABP] ,[AWPUnit] ,[Usual] ,[Prescriber] ,[PayableAmount] ,[BilledAmount] ,[TransactionType] ,[Compound] ,[TranID] ,[RefillDate],[RefillNumber],[MONY],[DAW],[GPI],[BillIngrCost],[BillDispFee],[BilledTax],[BilledCopay],[PayIngrCost],[PayDispFee],[PayTax],[DEA],[PrescriberNPI],[Strength],[GPIGenName],[TheraClass],[InvoiceID],[ETLRowID],[AWP]
		FROM	[dbo].[Prescription]
		EXCEPT
		SELECT	[PrescriptionID] ,[ClaimID] ,[RxNumber] ,[DateSubmitted] ,[DateFilled] ,[LabelName] ,[NDC] ,[Quantity] ,[DaySupply] ,[Generic] ,[PharmacyNABP] ,[AWPUnit] ,[Usual] ,[Prescriber] ,[PayableAmount] ,[BilledAmount] ,[TransactionType] ,[Compound] ,[TranID] ,[RefillDate],[RefillNumber],[MONY],[DAW],[GPI],[BillIngrCost],[BillDispFee],[BilledTax],[BilledCopay],[PayIngrCost],[PayDispFee],[PayTax],[DEA],[PrescriberNPI],[Strength],[GPIGenName],[TheraClass],[InvoiceID],[ETLRowID],[AWP]
		FROM	#PrescriptionUpdate
	)
	UPDATE [p] SET [p].[Prescriber] = [c].[Prescriber], 
				   [p].[PrescriberNPI] = [c].[PrescriberNPI],
				   [p].[ClaimID] = [c].[ClaimID],
				   [p].[RxNumber] = [c].[RxNumber],
				   [p].[DateSubmitted] = [c].[DateSubmitted],
				   [p].[DateFilled] = [c].[DateFilled],
				   [p].[LabelName] = c.[LabelName],
				   p.[NDC] = c.[NDC],
				   p.[Quantity] = c.[Quantity],
				   p.[DaySupply] = c.[DaySupply],
				   p.[Generic] = c.[Generic],
				   p.[PharmacyNABP] = c.[PharmacyNABP],
				   p.[AWPUnit] = c.[AWPUnit],
				   p.[Usual] = c.[Usual],
				   p.[PayableAmount] = c.[PayableAmount],
				   p.[BilledAmount] = c.[BilledAmount],
				   p.[TransactionType] = c.[TransactionType],
				   p.[Compound] = c.[Compound],
				   p.[TranID] = c.[TranID],
				   p.[RefillDate] = c.[RefillDate],
				   p.[RefillNumber] = c.[RefillNumber],
				   p.[MONY] = c.[MONY],
				   p.[DAW] = c.[DAW],
				   p.[BillIngrCost] = c.[BillIngrCost],
				   p.[PayDispFee] = [c].[PayDispFee],
				   p.[BilledTax] = c.[BilledTax],
				   p.[PayTax] = c.[PayTax],
				   p.[DEA] = c.[DEA],
				   p.[BillDispFee] = c.[BillDispFee],
				   p.[Strength] = c.[Strength],
				   p.[GPI] = c.[GPI],
				   p.[GPIGenName] = c.[GPIGenName],
				   p.[UpdatedOnUTC] = @UTCNow
	FROM   UpdatedPrescriptionsCTE AS [c]
		   INNER JOIN [dbo].[Prescription] AS [p] ON [p].[PrescriptionID] = [c].[PrescriptionID]

	SET @UpdatedRowCount = @@ROWCOUNT

	IF @UpdatedRowCount != (SELECT COUNT(*) FROM [dbo].[Prescription] AS [p] WHERE [p].[UpdatedOnUTC] = @UTCNow)
		BEGIN
			IF @@TRANCOUNT > 0
				ROLLBACK
			RAISERROR(N'Error. The QA check for the count of rows updated, and the count of Updated Prescription Records didn''t match', 16, 1) WITH NOWAIT
			RETURN
		END

	/********************************************************************************************
	Begin AcctPayable Section
	********************************************************************************************/
	--  Stop this insanity
	/*INSERT INTO [dbo].[AcctPayable] ([CheckNumber],[CheckDate],[AmountPaid],[ClaimID],[InvoiceID],[CreatedOnUTC],[UpdatedOnUTC],[ETLRowID])
	SELECT [s].[6]
		 , [s].[7]
		 , 0
		 , [s].[ClaimID]
		 , [s].[InvoiceID]
		 , @UTCNow
		 , @UTCNow
		 , [s].[RowID]
	FROM   [etl].[StagedLakerFile] AS [s]
	WHERE  1 = 1
		   AND [s].[InvoiceID] IS NOT NULL
		   AND [s].[6] IS NOT NULL*/
	   
	DECLARE @Success BIT = 0
	IF @@TRANCOUNT > 0
		BEGIN
			IF @@ERROR = 0
				BEGIN
					COMMIT
					SET @Success = 1
				END
			ELSE
				ROLLBACK
		END
	
	IF @@TRANCOUNT > 0
		RAISERROR(N'A transaction is still open', 16, 1) WITH NOWAIT
END
GO

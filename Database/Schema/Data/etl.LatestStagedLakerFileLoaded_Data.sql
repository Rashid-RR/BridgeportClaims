SET IDENTITY_INSERT [etl].[LatestStagedLakerFileLoaded] ON
INSERT INTO [etl].[LatestStagedLakerFileLoaded] ([ID], [LastFileNameLoaded]) VALUES (1, 'Billing_Claim_File_20180403.csv')
SET IDENTITY_INSERT [etl].[LatestStagedLakerFileLoaded] OFF

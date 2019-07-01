SET IDENTITY_INSERT [etl].[ImportType] ON
INSERT INTO [etl].[ImportType] ([ImportTypeID], [TypeName], [TypeCode], [Description], [CreatedOnUTC], [UpdatedOnUTC]) VALUES (1, 'Laker', 'LAKER', 'The Laker Import File', '2019-04-01 23:32:32.6994552', '2019-04-01 23:32:32.6994552')
INSERT INTO [etl].[ImportType] ([ImportTypeID], [TypeName], [TypeCode], [Description], [CreatedOnUTC], [UpdatedOnUTC]) VALUES (2, 'Envision', 'ENVISION', 'The Envision Import File', '2019-04-01 23:32:32.6994552', '2019-04-01 23:32:32.6994552')
SET IDENTITY_INSERT [etl].[ImportType] OFF

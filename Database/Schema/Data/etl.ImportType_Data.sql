SET IDENTITY_INSERT [etl].[ImportType] ON
INSERT INTO [etl].[ImportType] ([ImportTypeID], [TypeName], [TypeCode], [Description], [CreatedOnUTC], [UpdatedOnUTC]) VALUES (1, 'Laker', 'LAKER', 'The Laker Import File', '2019-04-06 22:52:02.2090207', '2019-04-06 22:52:02.2090207')
INSERT INTO [etl].[ImportType] ([ImportTypeID], [TypeName], [TypeCode], [Description], [CreatedOnUTC], [UpdatedOnUTC]) VALUES (2, 'Envision', 'ENVISION', 'The Envision Import File', '2019-04-06 22:52:02.2090207', '2019-04-06 22:52:02.2090207')
SET IDENTITY_INSERT [etl].[ImportType] OFF

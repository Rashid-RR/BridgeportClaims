SET IDENTITY_INSERT [dbo].[NotificationType] ON
INSERT INTO [dbo].[NotificationType] ([NotificationTypeID], [TypeName], [Code], [NotificationConfigDescription], [CreatedOnUTC], [UpdatedOnUTC]) VALUES (3, 'Envision Information', 'ENVISIONINFO', 'Stores the latest, imported Envision Imported data', '2019-07-10 08:20:26.8087342', '2019-07-10 08:20:26.8087342')
SET IDENTITY_INSERT [dbo].[NotificationType] OFF
SET IDENTITY_INSERT [dbo].[NotificationType] ON
INSERT INTO [dbo].[NotificationType] ([NotificationTypeID], [TypeName], [Code], [NotificationConfigDescription], [CreatedOnUTC], [UpdatedOnUTC]) VALUES (1, 'Payor Letter Name', 'PAYORLETTER', 'Stores the MAX(PayorID) from the Payor table prior to the data import', '2018-03-14 07:44:59.8142593', '2018-03-14 07:44:59.8142593')
INSERT INTO [dbo].[NotificationType] ([NotificationTypeID], [TypeName], [Code], [NotificationConfigDescription], [CreatedOnUTC], [UpdatedOnUTC]) VALUES (2, 'New Client Referral', 'NEWREFERRAL', 'Stores the MAX(ReferralID) from the client.Referral table after the last referrals were pushed as notifications', '2019-03-15 06:45:35.5581975', '2019-03-15 06:45:35.5581975')
SET IDENTITY_INSERT [dbo].[NotificationType] OFF

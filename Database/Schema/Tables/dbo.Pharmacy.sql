CREATE TABLE [dbo].[Pharmacy]
(
[phaNABP] [nvarchar] (7) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[phaNPI] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaPharmacyName] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaAddr1] [nvarchar] (55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaAddr2] [nvarchar] (55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaCity] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaState] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaZip] [nvarchar] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaPhone] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaAltPhone] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaFax] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaContact] [nvarchar] (55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaContactPhone] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaContactEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaFedTIN] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phaDispType] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO

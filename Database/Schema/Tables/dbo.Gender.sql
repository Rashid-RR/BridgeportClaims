CREATE TABLE [dbo].[Gender]
(
[GenderID] [int] NOT NULL IDENTITY(1, 1),
[GenderName] [varchar] (55) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GenderCode] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfGenderCreatedOnUTC] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfGenderUpdatedOnUTC] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Gender] ADD CONSTRAINT [pkGender] PRIMARY KEY CLUSTERED  ([GenderID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO

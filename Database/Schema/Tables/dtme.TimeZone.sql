CREATE TABLE [dtme].[TimeZone]
(
[TimeZoneID] [int] NOT NULL IDENTITY(1, 1),
[Display] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Bias] [smallint] NOT NULL,
[StdBias] [smallint] NOT NULL,
[DltBias] [smallint] NOT NULL,
[StdMonth] [smallint] NOT NULL,
[StdDayOfWeek] [smallint] NOT NULL,
[StdWeek] [smallint] NOT NULL,
[StdHour] [smallint] NOT NULL,
[DltMonth] [smallint] NOT NULL,
[DltDayOfWeek] [smallint] NOT NULL,
[DltWeek] [smallint] NOT NULL,
[DltHour] [smallint] NOT NULL,
[IsDefault] [bit] NOT NULL CONSTRAINT [dfTimeZoneInfoIsDefault] DEFAULT ((0))
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dtme].[TimeZone] ADD CONSTRAINT [pkTimeZoneInfo] PRIMARY KEY CLUSTERED  ([TimeZoneID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idxUqTimeZoneIsDefaultIncludeTimeZoneID] ON [dtme].[TimeZone] ([IsDefault]) INCLUDE ([TimeZoneID]) WHERE ([isDefault]=(1)) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO

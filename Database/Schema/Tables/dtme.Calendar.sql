CREATE TABLE [dtme].[Calendar]
(
[DateID] [date] NOT NULL,
[CalendarYear] [smallint] NOT NULL,
[CalendarQuarter] [tinyint] NOT NULL,
[CalendarQuarterDescription] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CalendarMonth] [tinyint] NOT NULL,
[CalendarMonthNameLong] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CalendarMonthNameShort] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CalendarWeekInYear] [tinyint] NOT NULL,
[CalendarWeekInMonth] [tinyint] NOT NULL,
[CalendarDayInYear] [smallint] NOT NULL,
[CalendarDayInWeek] [tinyint] NOT NULL,
[CalendarDayInMonth] [tinyint] NOT NULL,
[MonthDayYearNameLong] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MonthDayYearNameLongWithSuffix] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DayNameLong] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DayNameShort] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ContinuousYear] [tinyint] NOT NULL,
[ContinuousQuarter] [smallint] NOT NULL,
[ContinuousMonth] [smallint] NOT NULL,
[ContinuousWeek] [smallint] NOT NULL,
[ContinuousDay] [int] NOT NULL,
[DateDescription] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IsWeekend] [tinyint] NOT NULL,
[IsHoliday] [tinyint] NOT NULL,
[IsWorkDay] [tinyint] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dtme].[Calendar] ADD CONSTRAINT [pkCalendar] PRIMARY KEY CLUSTERED  ([DateID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO

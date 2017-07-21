CREATE TABLE [dtme].[Calendar]
(
[DateID] [date] NOT NULL,
[CalendarYear] [smallint] NULL,
[CalendarQuarter] [tinyint] NULL,
[CalendarQuarterDescription] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CalendarMonth] [tinyint] NULL,
[CalendarMonthNameLong] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CalendarMonthNameShort] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CalendarWeekInYear] [tinyint] NULL,
[CalendarWeekInMonth] [tinyint] NULL,
[CalendarDayInYear] [smallint] NULL,
[CalendarDayInWeek] [tinyint] NULL,
[CalendarDayInMonth] [tinyint] NULL,
[MonthDayYearNameLong] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MonthDayYearNameLongWithSuffix] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DayNameLong] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DayNameShort] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[continuous_year] [tinyint] NULL,
[continuous_quarter] [smallint] NULL,
[continuous_month] [smallint] NULL,
[continuous_week] [smallint] NULL,
[continuous_day] [int] NULL,
[description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[is_weekend] [tinyint] NULL,
[is_holiday] [tinyint] NULL,
[is_workday] [tinyint] NULL,
[is_event] [tinyint] NULL,
[is_meds_day] [bit] NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dtme].[Calendar] ADD CONSTRAINT [pkCalendar] PRIMARY KEY CLUSTERED  ([DateID]) WITH (FILLFACTOR=100, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO

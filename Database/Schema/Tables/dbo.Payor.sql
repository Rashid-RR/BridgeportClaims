CREATE TABLE [dbo].[Payor]
(
[PayorID] [int] NOT NULL IDENTITY(1, 1),
[GroupName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillToName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillToAddress1] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BillToAddress2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BillToCity] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BillToStateID] [int] NULL,
[BillToPostalCode] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AlternatePhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FaxNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Notes] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Contact] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LetterName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [dfPayorLetterNameBillToName] DEFAULT (''),
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ETLRowID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPayorCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPayorUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		3/11/2018
 Description:		User Trigger to change the insert behavior of an insert into dbo.Payor
					which uses the value of the BillToName column for the LetterName column.
 =============================================
*/
CREATE TRIGGER [dbo].[utPayorLetterNameBillToName] ON [dbo].[Payor]
INSTEAD OF INSERT
AS BEGIN
SET NOCOUNT ON;
INSERT INTO [dbo].[Payor]
(   [GroupName]
  , [BillToName]
  , [BillToAddress1]
  , [BillToAddress2]
  , [BillToCity]
  , [BillToStateID]
  , [BillToPostalCode]
  , [PhoneNumber]
  , [AlternatePhoneNumber]
  , [FaxNumber]
  , [Notes]
  , [Contact]
  , [LetterName]
  , [ETLRowID]
  , [CreatedOnUTC]
  , [UpdatedOnUTC]
  , [ModifiedByUserID])
SELECT [i].[GroupName]
     , [i].[BillToName]
     , [i].[BillToAddress1]
     , [i].[BillToAddress2]
     , [i].[BillToCity]
     , [i].[BillToStateID]
     , [i].[BillToPostalCode]
     , [i].[PhoneNumber]
     , [i].[AlternatePhoneNumber]
     , [i].[FaxNumber]
     , [i].[Notes]
     , [i].[Contact]
     , [i].[BillToName] -- This is the key to this trigger. Making the LetterName equal to the BillToName
     , [i].[ETLRowID]
     , [i].[CreatedOnUTC]
     , [i].[UpdatedOnUTC]
	 , [i].[ModifiedByUserID]
FROM Inserted i
END
GO
ALTER TABLE [dbo].[Payor] ADD CONSTRAINT [pkPayor] PRIMARY KEY CLUSTERED  ([PayorID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Payor] ADD CONSTRAINT [idxUqPayorGroupName] UNIQUE NONCLUSTERED  ([GroupName]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPayorModifiedByUserID] ON [dbo].[Payor] ([ModifiedByUserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Payor] ADD CONSTRAINT [fkPayorBillToStateIDUsStateStateID] FOREIGN KEY ([BillToStateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
ALTER TABLE [dbo].[Payor] ADD CONSTRAINT [fkPayorModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO

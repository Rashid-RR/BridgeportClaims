CREATE TABLE [client].[UserType]
(
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReferralTypeID] [tinyint] NOT NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfUserTypeCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfUserTypeUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [client].[UserType] ADD CONSTRAINT [pkUserType] PRIMARY KEY CLUSTERED  ([UserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxUserTypeReferralTypeIDIncludes] ON [client].[UserType] ([ReferralTypeID]) INCLUDE ([CreatedOnUTC], [ModifiedByUserID], [UpdatedOnUTC], [UserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [client].[UserType] ADD CONSTRAINT [fkUserTypeModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [client].[UserType] ADD CONSTRAINT [fkUserTypeReferralTypeIDReferralTypeReferralTypeID] FOREIGN KEY ([ReferralTypeID]) REFERENCES [client].[ReferralType] ([ReferralTypeID])
GO
ALTER TABLE [client].[UserType] ADD CONSTRAINT [fkUserTypeUserIDAspNetUsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO

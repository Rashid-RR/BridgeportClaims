CREATE TABLE [client].[ReferralType]
(
[ReferralTypeID] [tinyint] NOT NULL IDENTITY(1, 1),
[TypeName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfReferralTypeCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfReferralTypeUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [client].[ReferralType] ADD CONSTRAINT [pkReferralType] PRIMARY KEY CLUSTERED  ([ReferralTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [client].[ReferralType] ADD CONSTRAINT [idxUqReferralTypeCode] UNIQUE NONCLUSTERED  ([Code]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Claim]
(
[ClaimID] [int] NOT NULL IDENTITY(1, 1),
[PolicyNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DOI] [datetime2] NULL,
[IsFirstParty] [bit] NOT NULL,
[ClaimNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PreviousClaimNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PersonCode] [int] NULL,
[PayorID] [int] NOT NULL,
[AdjusterID] [int] NULL,
[Jurisdiction] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RelCode] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TermDate] [datetime2] NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfClaimCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfClaimUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [pkClaim] PRIMARY KEY CLUSTERED  ([ClaimID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimAdjusterID] ON [dbo].[Claim] ([AdjusterID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimPayorID] ON [dbo].[Claim] ([PayorID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimAdjusterIDAdjustorAdjustorID] FOREIGN KEY ([AdjusterID]) REFERENCES [dbo].[Adjustor] ([AdjustorID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimPayorIDPayorPayorID] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payor] ([PayorID])
GO

CREATE TABLE [dbo].[DuplicateClaim]
(
[DuplicateClaimID] [int] NOT NULL,
[ReplacementClaimID] [int] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[DuplicateClaim] ADD CONSTRAINT [pkDuplicateClaim] PRIMARY KEY CLUSTERED  ([DuplicateClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDuplicateClaimReplacementClaimID] ON [dbo].[DuplicateClaim] ([ReplacementClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DuplicateClaim] ADD CONSTRAINT [FK__Duplicate__Repla__0169315C] FOREIGN KEY ([ReplacementClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwPrescriptionPharmacy]
WITH SCHEMABINDING
AS
SELECT          [ph].[PharmacyName], [p].[PrescriptionID], [p].[PharmacyNABP]
FROM            [dbo].[Prescription] AS [p]
    INNER JOIN  [dbo].[Pharmacy]     AS [ph] ON [ph].[NABP] = [p].[PharmacyNABP]
GO
CREATE UNIQUE CLUSTERED INDEX [idxUqClusPrescriptionPrescriptionID] ON [dbo].[vwPrescriptionPharmacy] ([PrescriptionID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO

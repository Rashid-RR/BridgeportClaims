SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwSkippedPaymentPrescription]
AS
SELECT  p.PrescriptionID
       ,p.ClaimID
       ,p.RxNumber
       ,p.DateSubmitted
       ,p.DateFilled
       ,p.LabelName
       ,p.NDC
       ,p.Quantity
       ,p.DaySupply
       ,p.Generic
       ,p.PharmacyNABP
       ,p.AWPUnit
       ,p.Usual
       ,p.Prescriber
       ,p.PayableAmount
       ,p.BilledAmount
       ,p.TransactionType
       ,p.Compound
       ,p.TranID
       ,p.RefillDate
       ,p.RefillNumber
       ,p.MONY
       ,p.DAW
       ,p.GPI
       ,p.BillIngrCost
       ,p.BillDispFee
       ,p.BilledTax
       ,p.BilledCopay
       ,p.PayIngrCost
       ,p.PayDispFee
       ,p.PayTax
       ,p.DEA
       ,p.PrescriberNPI
       ,p.Strength
       ,p.GPIGenName
       ,p.TheraClass
       ,p.InvoiceID
       ,p.ETLRowID
       ,p.AWP
       ,p.ReversedDate
       ,p.IsReversed
       ,p.PrescriptionStatusID
       ,p.Adjudicated
       ,p.CreatedOnUTC
       ,p.UpdatedOnUTC
       ,p.DataVersion
       ,p.BilledAmountOriginal
       ,p.PayableAmountOriginal
FROM    dbo.Prescription AS p
        LEFT JOIN dbo.SkippedPaymentExclusion s ON p.PrescriptionID = s.PrescriptionID
WHERE	s.SkippedPaymentExclusionID IS NULL
GO

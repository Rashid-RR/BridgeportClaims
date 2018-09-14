SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[uspPrescriptionPaymentDelete]
    @PrescriptionPaymentID int
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DELETE
	FROM   [dbo].[PrescriptionPayment]
	WHERE  [PrescriptionPaymentID] = @PrescriptionPaymentID
END
GO

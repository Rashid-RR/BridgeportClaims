SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [client].[uspReferralUpdate] 
    @ReferralID int,
    @ClaimNumber varchar(255),
    @JurisdictionStateID int,
    @LastName varchar(155),
    @FirstName varchar(155),
    @DateOfBirth date,
    @InjuryDate date,
    @Notes varchar(8000),
    @ReferredBy nvarchar(128),
    @ReferralDate datetime2(7),
    @ReferralTypeID tinyint,
    @EligibilityStart datetime2(7),
    @EligibilityEnd datetime2(7),
    @Address1 varchar(255),
    @Address2 varchar(255),
    @City varchar(155),
    @StateID int,
    @PostalCode varchar(100),
    @PatientPhone varchar(30),
    @AdjustorName varchar(255),
    @AdjustorPhone varchar(30)
AS 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @UtcNow DATETIME2 = dtme.udfGetDate();
	UPDATE [client].[Referral]
	SET    [ClaimNumber] = @ClaimNumber, [JurisdictionStateID] = @JurisdictionStateID, [LastName] = @LastName, [FirstName] = @FirstName,
		   [DateOfBirth] = @DateOfBirth, [InjuryDate] = @InjuryDate, [Notes] = @Notes, [ReferredBy] = @ReferredBy, [ReferralDate] = @ReferralDate,
		   [ReferralTypeID] = @ReferralTypeID, [EligibilityStart] = @EligibilityStart, [EligibilityEnd] = @EligibilityEnd, [Address1] = @Address1,
		   [Address2] = @Address2, [City] = @City, [StateID] = @StateID, PostalCode = @PostalCode, [PatientPhone] = @PatientPhone, [AdjustorName] = @AdjustorName,
		   [AdjustorPhone] = @AdjustorPhone, [UpdatedOnUTC] = @UtcNow
	WHERE  [ReferralID] = @ReferralID;
GO

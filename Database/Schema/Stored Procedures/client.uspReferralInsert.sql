SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [client].[uspReferralInsert] 
    @ClaimNumber varchar(255),
    @JurisdictionStateID int,
    @LastName varchar(155),
    @FirstName varchar(155),
    @DateOfBirth date,
    @InjuryDate date,
    @Notes varchar(8000),
    @ReferredBy nvarchar(128),
    @ReferralDate datetime2(7),
    @EligibilityStart datetime2(7),
    @EligibilityEnd datetime2(7),
    @Address1 varchar(255),
    @Address2 varchar(255),
    @City varchar(155),
    @StateID int,
    @PostalCode varchar(100),
    @PatientPhone varchar(30),
    @AdjustorName varchar(255),
    @AdjustorPhone varchar(30),
	@PersonCode CHAR(2),
	@GenderID INT,
	@GroupName VARCHAR(255)
AS BEGIN 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	DECLARE @UtcNow DATETIME2 = dtme.udfGetDate();
	INSERT INTO [client].[Referral] ([ClaimNumber], [JurisdictionStateID], [LastName], [FirstName], [DateOfBirth], [InjuryDate], [Notes], [ReferredBy], [ReferralDate], [EligibilityStart], [EligibilityEnd], [Address1], [Address2], [City], [StateID], PostalCode, [PatientPhone], [AdjustorName], [AdjustorPhone], [PersonCode], [GenderID], [GroupName], [CreatedOnUTC], [UpdatedOnUTC])
	SELECT @ClaimNumber, @JurisdictionStateID, @LastName, @FirstName, @DateOfBirth, @InjuryDate, @Notes, @ReferredBy, @ReferralDate, @EligibilityStart, @EligibilityEnd, @Address1, @Address2, @City, @StateID, @PostalCode, @PatientPhone, @AdjustorName, @AdjustorPhone, @PersonCode, @GenderID, @GroupName, @UtcNow, @UtcNow;
END
GO

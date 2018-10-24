SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [client].[uspUserTypeUpsert]
    @UserID NVARCHAR(128),
    @ReferralTypeID TINYINT,
	@ModifiedByUserID NVARCHAR(128)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @UtcNow DATETIME2 = dtme.udfGetDate();

	MERGE client.UserType AS tgt
		USING (SELECT @UserID UserID, @ReferralTypeID ReferralTypeID, @ModifiedByUserID ModifiedByUserID,
			   @UtcNow CreatedOnUTC, @UtcNow UpdatedOnUTC) AS src
		ON tgt.UserID = src.UserID
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (UserID, ReferralTypeID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC)
		VALUES (src.UserID, src.ReferralTypeID, src.ModifiedByUserID, src.CreatedOnUTC, src.UpdatedOnUTC)
	WHEN MATCHED THEN
		UPDATE SET tgt.ReferralTypeID = src.ReferralTypeID,
				   tgt.ModifiedByUserID = src.ModifiedByUserID,
				   tgt.UpdatedOnUTC = src.UpdatedOnUTC;
	SELECT rt.TypeName
	FROM client.ReferralType AS rt
	WHERE rt.ReferralTypeID = @ReferralTypeID;
END
GO

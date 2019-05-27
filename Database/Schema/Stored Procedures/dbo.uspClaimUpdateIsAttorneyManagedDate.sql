SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       5/26/2019
 Description:       Updates the Claim table, IsAttorneyManagedDate column.
 Example Execute:
					DECLARE @ID NVARCHAR(128) = util.udfGetRandomAspNetUserID();
                    EXECUTE [dbo].[uspClaimUpdateIsAttorneyManagedDate] 7, @ID;
 =============================================
*/
CREATE PROCEDURE [dbo].[uspClaimUpdateIsAttorneyManagedDate]
(
	@ClaimID INT,
	@IsAttorneyManaged BIT,
	@ModifiedByUserID NVARCHAR(128)
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Today DATE = CAST(dtme.udfGetLocalDate() AS DATE),
			@UtcNow DATETIME2 = dtme.udfGetDate();

    UPDATE c
    SET c.IsAttorneyManagedDate = CASE @IsAttorneyManaged WHEN 1 THEN @Today ELSE NULL END, c.UpdatedOnUTC = @UtcNow, c.ModifiedByUserID = @ModifiedByUserID
    FROM dbo.Claim AS c
    WHERE c.ClaimID = @ClaimID;

END;
GO

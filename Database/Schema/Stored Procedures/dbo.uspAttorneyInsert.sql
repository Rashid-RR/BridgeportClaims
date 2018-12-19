SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[uspAttorneyInsert] 
    @AttorneyName varchar(255),
    @Address1 varchar(255),
    @Address2 varchar(255),
    @City varchar(255),
    @StateID int,
    @PostalCode varchar(255),
    @PhoneNumber varchar(30),
    @FaxNumber varchar(30),
    @ModifiedByUserID nvarchar(128)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();
	INSERT INTO [dbo].[Attorney] ([AttorneyName], [Address1], [Address2], [City], [StateID], [PostalCode], [PhoneNumber], [FaxNumber], [ModifiedByUserID], [CreatedOnUTC], [UpdatedOnUTC])
	SELECT @AttorneyName, @Address1, @Address2, @City, @StateID, @PostalCode, @PhoneNumber, @FaxNumber, @ModifiedByUserID, @UtcNow, @UtcNow;
END
GO

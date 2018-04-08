SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/29/2017
	Description:	CRUD Proc inserting into [dbo].[InvoiceIndex]
	Sample Execute:
			EXEC [dbo].[uspInvoiceIndexInsert]
*/
CREATE PROC [dbo].[uspInvoiceIndexInsert]
    @DocumentID int,
    @InvoiceNumber varchar(100),
    @ModifiedByUserID nvarchar(128),
	@AlreadyExists BIT OUTPUT
AS 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
		DECLARE @PrntMsg NVARCHAR(100)

		IF EXISTS (SELECT * FROM [dbo].[InvoiceIndex] AS [ii] WHERE  [ii].[InvoiceNumber] = @InvoiceNumber)
			SET @AlreadyExists = 1;
		ELSE
			SET @AlreadyExists = 0;

		IF (@AlreadyExists = 1)
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
 			 	SET @PrntMsg = N'An InvoiceIndex record already exists.';
				RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
				RETURN -1;
			END

		DECLARE @UtcNow DATETIME2 = SYSUTCDATETIME();
		INSERT INTO [dbo].[InvoiceIndex] ([DocumentID], [InvoiceNumber], [ModifiedByUserID], [CreatedOnUTC], [UpdatedOnUTC])
		SELECT @DocumentID, @InvoiceNumber, @ModifiedByUserID, @UtcNow, @UtcNow

		IF (@@TRANCOUNT > 0)
			COMMIT
	END TRY
	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			ROLLBACK;
				
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

        RAISERROR(N'%s (line %d): %s',	-- Message text w formatting
			@ErrSeverity,		-- Severity
			@ErrState,			-- State
			@ErrProc,			-- First argument (string)
			@ErrLine,			-- Second argument (int)
			@ErrMsg);			-- First argument (string)
	END CATCH
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       11/4/2018
 Description:       Merges users to collection assignments. 
 Example Execute:
                    DECLARE @Base [dbo].[udtID];
					INSERT @Base (ID) VALUES (1),(2),(3);
					EXECUTE [dbo].[uspAssignCollections] N'e9c4cd49-251f-4782-97be-2e8f6d90c328', N'77abd5bb-075f-40d0-a97d-2784a320d66d', @Base;
 =============================================
*/
CREATE PROC [dbo].[uspMergeCollectionAssignments]
(
	@UserID NVARCHAR(128),
	@ModifiedByUserID NVARCHAR(128),
	@Payors [dbo].[udtID] READONLY
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
            
		DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();

        MERGE INTO [dbo].[CollectionAssignment] AS [tgt]
		USING @Payors AS src
		ON src.[ID] = [tgt].[PayorID] AND [tgt].[UserID] = @UserID
		WHEN NOT MATCHED BY TARGET
		THEN INSERT ([UserID], [PayorID], [ModifiedByUserID], [CreatedOnUTC], [UpdatedOnUTC])
			 VALUES (@UserID, [src].[ID], @ModifiedByUserID, @UtcNow, @UtcNow)
		WHEN MATCHED THEN UPDATE SET [tgt].[ModifiedByUserID] = @ModifiedByUserID, [tgt].[UpdatedOnUTC] = @UtcNow
		WHEN NOT MATCHED BY SOURCE AND [tgt].[UserID] = @UserID
			THEN DELETE;
            
        IF (@@TRANCOUNT > 0)
            COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
            ROLLBACK;
                
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(4000) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();

        RAISERROR(N'%s (line %d): %s', @ErrSeverity, @ErrState, @ErrProc, @ErrLine, @ErrMsg);
    END CATCH
END
GO

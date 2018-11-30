SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       11/29/2018
 Description:       Cleans up all of the Adjustors who have a blank Adjustor name
					and no other fields populated.
 Example Execute:
                    EXECUTE [util].[uspCleanupEmptyAdjustors]
 =============================================
*/
CREATE PROC [util].[uspCleanupEmptyAdjustors]
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;            
		-- Testing...
		-- DROP TABLE IF EXISTS [#Adjustors];

        -- First, identify all of the empty Adjustors.
		CREATE TABLE [#Adjustors] (AdjustorID INT NOT NULL PRIMARY KEY);

		INSERT INTO [#Adjustors] ([AdjustorID])
		SELECT	[a].[AdjustorID]
		FROM	[dbo].[Adjustor] AS [a]
		WHERE	LTRIM(RTRIM([a].[AdjustorName])) = ''
				AND ISNULL([a].[PhoneNumber], '') = ''
				AND ISNULL([a].[FaxNumber], '') = ''
				AND ISNULL([a].[EmailAddress], '') = ''
				AND ISNULL([a].[Extension], '') = ''
				AND ISNULL([a].[ModifiedByUserID], '') = '';

		UPDATE  [c]
		SET		[c].[AdjustorID] = NULL
		FROM    [dbo].[Claim] AS [c]
				INNER JOIN [#Adjustors] AS [a] ON [c].[AdjustorID] = [a].[AdjustorID];

		-- Delete all of the Empty Adjustors.
        DELETE	[a]
		FROM    [dbo].[Adjustor] AS [a]
				INNER JOIN [#Adjustors] AS [a2] ON [a].[AdjustorID] = [a2].[AdjustorID];

        IF (@@TRANCOUNT > 0)
            COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
            ROLLBACK;
                
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

        RAISERROR(N'%s (line %d): %s', @ErrSeverity, @ErrState, @ErrProc, @ErrLine, @ErrMsg);
    END CATCH
END
GO

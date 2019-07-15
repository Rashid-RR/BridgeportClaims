SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/12/2019
 Description:       Gets the Payors by autocomplete search
 Example Execute:
                    EXECUTE [claims].[uspGetPayorsSearch] 'AA'
 =============================================
*/
CREATE PROCEDURE [claims].[uspGetPayorsSearch] (@SearchTerm VARCHAR(100))
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        DECLARE @WildCard CHAR(1) = '%';
        SELECT [p].[PayorID] AS [PayorId]
              ,[p].[GroupName] AS [Carrier]
        FROM [dbo].[Payor] AS [p]
        WHERE [p].[GroupName] LIKE CONCAT(@SearchTerm, @WildCard);
    END;
GO

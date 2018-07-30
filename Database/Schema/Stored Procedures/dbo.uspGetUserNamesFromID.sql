SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE   PROC [dbo].[uspGetUserNamesFromID]
(
    @UserName NVARCHAR(128)
)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        SELECT  u.ID Id, u.FirstName, u.LastName
        FROM    dbo.AspNetUsers AS u
        WHERE   u.ID = @UserName
    END
GO

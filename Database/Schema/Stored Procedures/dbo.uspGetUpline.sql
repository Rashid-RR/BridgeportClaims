SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[uspGetUpline] @LeafTreeID INT
AS
    BEGIN
        SELECT [x].[TreePath]
             , [x].[TreeLevel]
             , [x].[TreeID] AS [TreeId]
             , [x].[NodeName]
             , [x].[ParentTreeID] AS [ParentTreeId]
        FROM   [dbo].[udfGetUpline](73) AS [x];
    END;
GO

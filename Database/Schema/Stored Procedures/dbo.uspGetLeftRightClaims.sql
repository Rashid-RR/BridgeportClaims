SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       5/27/2018
 Description:       Gets the side-by-side Claims for comparison.
 Example Execute:
                    EXECUTE [dbo].[uspGetLeftRightClaims] 2392, 508
 =============================================
*/
CREATE PROC [dbo].[uspGetLeftRightClaims]
(
    @LeftSideClaimID INT,
    @RightSideClaimID INT
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SELECT  [LeftClaimID] = [l].[ClaimID]
           ,[LeftClaimNumber] = [l].[ClaimNumber]
           ,[LeftPatientID] = [lpat].[PatientID]
           ,[LeftPatientName] = ISNULL([lpat].[LastName], '') + ', ' + ISNULL([lpat].[FirstName], '')
           ,[LeftDateOfBirth] = [lpat].[DateOfBirth]
           ,[LeftInjuryDate] = [l].[DateOfInjury]
           ,[LeftAdjustorID] = [l].[AdjustorID]
           ,[LeftAdjustorName] = [la].[AdjustorName]
           ,[LeftPayorID] = [l].[PayorID]
           ,[LeftCarrier] = [lca].[GroupName]
           ,[LeftClaimFlex2] = [lcf].[Flex2]
           ,[RightClaimID] = [r].[ClaimID]
           ,[RightClaimNumber] = [r].[ClaimNumber]
           ,[RightPatientID] = [rpat].[PatientID]
           ,[RightPatientName] = ISNULL([rpat].[LastName], '') + ', ' + ISNULL([rpat].[FirstName], '')
           ,[RightDateOfBirth] = [rpat].[DateOfBirth]
           ,[RightInjuryDate] = [r].[DateOfInjury]
           ,[RightAdjustorID] = [r].[AdjustorID]
           ,[RightAdjustorName] = [ra].[AdjustorName]
           ,[RightPayorID] = [r].[PayorID]
           ,[RightCarrier] = [rca].[GroupName]
           ,[RightClaimFlex2] = [rcf].[Flex2]
    FROM    [dbo].[Claim] AS [l]
            INNER JOIN [dbo].[Patient] AS [lpat] ON [lpat].[PatientID] = [l].[PatientID]
            INNER JOIN [dbo].[Payor] AS [lca] ON [lca].[PayorID] = [l].[PayorID]
            LEFT JOIN [dbo].[Adjustor] AS [la] ON [la].[AdjustorID] = [l].[AdjustorID]
            LEFT JOIN [dbo].[ClaimFlex2] AS [lcf] ON [lcf].[ClaimFlex2ID] = [l].[ClaimFlex2ID]
            INNER JOIN [dbo].[Claim] AS [r] ON [r].[ClaimID] = @RightSideClaimID
            INNER JOIN [dbo].[Patient] AS [rpat] ON [rpat].[PatientID] = [r].[PatientID]
            INNER JOIN [dbo].[Payor] AS [rca] ON [rca].[PayorID] = [r].[PayorID]
            LEFT JOIN [dbo].[Adjustor] AS [ra] ON [ra].[AdjustorID] = [r].[AdjustorID]
            LEFT JOIN [dbo].[ClaimFlex2] AS [rcf] ON [rcf].[ClaimFlex2ID] = [r].[ClaimFlex2ID]
    WHERE   [l].[ClaimID] = @LeftSideClaimID;
END
GO

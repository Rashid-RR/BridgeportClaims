SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* =============================================
 Author:      Jordan Gurney
 Create date: 1/25/2018
 Description: Creates INSERT records in the audit tables.
 =============================================
EXECUTE dbo.uspInsertInitialAuditRecords
*/
CREATE PROC [dbo].[uspInsertInitialAuditRecords]
AS
    BEGIN
        BEGIN TRY
            BEGIN TRAN
			SET NOCOUNT OFF;
            DECLARE @UtcNow DATETIME2 = SYSUTCDATETIME();
            INSERT INTO dbo.ClaimAudit ( ClaimID
                                       , PolicyNumber
                                       , DateOfInjury
                                       , IsFirstParty
                                       , ClaimNumber
                                       , PreviousClaimNumber
                                       , PersonCode
                                       , PayorID
                                       , AdjusterID
                                       , JurisdictionStateID
                                       , RelationCode
                                       , TermDate
                                       , PatientID
                                       , ETLRowID
                                       , UniqueClaimNumber
                                       , ClaimFlex2ID
                                       , ModifiedByUserID
                                       , CreatedOnUTC
                                       , UpdatedOnUTC
                                       , Operation
                                       , SystemUser
                                       , AuditDateUTC )
            SELECT c.ClaimID
                 , c.PolicyNumber
                 , c.DateOfInjury
                 , c.IsFirstParty
                 , c.ClaimNumber
                 , c.PreviousClaimNumber
                 , c.PersonCode
                 , c.PayorID
                 , c.AdjusterID
                 , c.JurisdictionStateID
                 , c.RelationCode
                 , c.TermDate
                 , c.PatientID
                 , c.ETLRowID
                 , c.UniqueClaimNumber
                 , c.ClaimFlex2ID
                 , c.ModifiedByUserID
                 , c.CreatedOnUTC
                 , c.UpdatedOnUTC
                 , 'INSERT'
                 , SUSER_SNAME()
                 , @UtcNow
            FROM   dbo.Claim AS c
            WHERE  NOT EXISTS (   SELECT *
                                  FROM   dbo.ClaimAudit AS ca
                                  WHERE  ca.ClaimID = c.ClaimID )

            INSERT INTO dbo.AdjustorAudit ( AdjustorID
                                          , PayorID
                                          , AdjustorName
                                          , PhoneNumber
                                          , FaxNumber
                                          , EmailAddress
                                          , Extension
                                          , ModifiedByUserID
                                          , CreatedOnUTC
                                          , UpdatedOnUTC
                                          , ETLRowID
                                          , Operation
                                          , SystemUser
                                          , AuditDateUTC )
            SELECT a.AdjustorID
                 , a.PayorID
                 , a.AdjustorName
                 , a.PhoneNumber
                 , a.FaxNumber
                 , a.EmailAddress
                 , a.Extension
                 , a.ModifiedByUserID
                 , a.CreatedOnUTC
                 , a.UpdatedOnUTC
                 , a.ETLRowID
                 , 'INSERT'
                 , SUSER_SNAME()
                 , @UtcNow
            FROM   dbo.Adjustor AS a
            WHERE  NOT EXISTS (   SELECT *
                                  FROM   dbo.AdjustorAudit AS aa
                                  WHERE  aa.AdjustorID = a.AdjustorID )

            INSERT INTO dbo.PatientAudit ( PatientID
                                         , LastName
                                         , FirstName
                                         , Address1
                                         , Address2
                                         , City
                                         , PostalCode
                                         , StateID
                                         , PhoneNumber
                                         , AlternatePhoneNumber
                                         , EmailAddress
                                         , DateOfBirth
                                         , GenderID
                                         , ModifiedByUserID
                                         , CreatedOnUTC
                                         , UpdatedOnUTC
                                         , ETLRowID
                                         , Operation
                                         , SystemUser
                                         , AuditDateUTC )
            SELECT p.PatientID
                 , p.LastName
                 , p.FirstName
                 , p.Address1
                 , p.Address2
                 , p.City
                 , p.PostalCode
                 , p.StateID
                 , p.PhoneNumber
                 , p.AlternatePhoneNumber
                 , p.EmailAddress
                 , p.DateOfBirth
                 , p.GenderID
                 , p.ModifiedByUserID
                 , p.CreatedOnUTC
                 , p.UpdatedOnUTC
                 , p.ETLRowID
                 , 'INSERT'
                 , SUSER_SNAME()
                 , @UtcNow
            FROM   dbo.Patient AS p
            WHERE  NOT EXISTS (   SELECT *
                                  FROM   dbo.PatientAudit AS pa
                                  WHERE  pa.PatientID = p.PatientID )
            IF @@TRANCOUNT > 0
                COMMIT /* If we made it this far without an error, commit */
        END TRY
        BEGIN CATCH
            IF ( @@TRANCOUNT > 0 )
                ROLLBACK;

            DECLARE @ErrSeverity INT = ERROR_SEVERITY()
                  , @ErrState INT = ERROR_STATE()
                  , @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
                  , @ErrLine INT = ERROR_LINE()
                  , @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

            RAISERROR(N'%s (line %d): %s' -- Message text w formatting
                    , @ErrSeverity        -- Severity
                    , @ErrState           -- State
                    , @ErrProc            -- First argument (string)
                    , @ErrLine            -- Second argument (int)
                    , @ErrMsg); -- First argument (string)
        END CATCH
    END

GO

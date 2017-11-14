SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwDiary] AS
SELECT          d.DiaryID
              , c.ClaimID
              , c.ClaimNumber
              , AssignedToUser = u.FirstName + ' ' + u.LastName
              , d.PrescriptionNoteID
              , d.FollowUpDate
              , d.DateResolved
              , d.CreatedDate
              , IsClosed       = CONVERT(BIT, CASE WHEN d.DateResolved IS NOT NULL THEN 1 ELSE 0 END)
              , CreatedOn      = dtme.udfGetLocalDateTime(d.CreatedOnUTC)
              , UpdatedOn      = dtme.udfGetLocalDateTime(d.UpdatedOnUTC)
              , RxDate         = p.DateFilled
              , p.RxNumber
FROM            dbo.Diary            AS d
    INNER JOIN  dbo.PrescriptionNote AS pn ON pn.PrescriptionNoteID = d.PrescriptionNoteID
    INNER JOIN  dbo.AspNetUsers      AS u ON u.ID = d.AssignedToUserID
    INNER JOIN  dbo.Prescription     AS p ON p.PrescriptionID =
                                          (   SELECT TOP (1)
                                                    pnm.PrescriptionID
                                              FROM  dbo.PrescriptionNoteMapping AS pnm
                                              WHERE pnm.PrescriptionNoteID = pn.PrescriptionNoteID)
    INNER JOIN  dbo.Claim            AS c ON c.ClaimID = p.ClaimID


GO

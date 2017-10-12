SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	10/8/2017
	Description:	Proc that returns the grid results for the Diary page.
	Sample Execute:
					EXEC [dbo].[uspGetDiaries] 0, NULL, NULL, 'FollowUpDate', 'DESC', 1, 5000
*/
CREATE PROC [dbo].[uspGetDiaries]
(
	@IsDefaultSort BIT,
	@StartDate DATE,
	@EndDate DATE,
	@SortColumn VARCHAR(50),
	@SortDirection VARCHAR(5),
	@PageNumber INTEGER,
	@PageSize INTEGER
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @One INT = 1;
	CREATE TABLE #Diaries
		([DiaryId] [int] NOT NULL PRIMARY KEY,
		[ClaimId] [int] NOT NULL,
		[PrescriptionNoteId] [int] NOT NULL,
		[Owner] [nvarchar](500) NOT NULL,
		[Created] [date] NOT NULL,
		[FollowUpDate] [date] NOT NULL,
		[PatientName] [varchar](500) NOT NULL,
		[ClaimNumber] [varchar](255) NOT NULL,
		[Type] [varchar](255) NOT NULL,
		[RxNumber] [varchar](100) NOT NULL,
		[RxDate] [datetime2](7) NOT NULL,
		[InsuranceCarrier] [varchar](255) NOT NULL,
		[DiaryNote] [varchar](8000) NOT NULL)
	INSERT #Diaries
		(DiaryId,ClaimId,PrescriptionNoteId,[Owner],Created,FollowUpDate,PatientName
		,ClaimNumber,[Type],RxNumber,RxDate,InsuranceCarrier,DiaryNote)
	SELECT          DiaryId            = d.DiaryID
				  , ClaimId            = c.ClaimID
				  , PrescriptionNoteId = pn.PrescriptionNoteID
				  , [Owner]            = u.FirstName + ' ' + u.LastName
				  , [Created]          = d.CreatedDate
				  , d.FollowUpDate
				  , [PatientName]      = pat.LastName + ', ' + pat.FirstName
				  , c.ClaimNumber
				  , [Type]             = pnt.TypeName
				  , p.RxNumber
				  , RxDate             = p.DateFilled
				  , InsuranceCarrier   = pay.GroupName
				  , DiaryNote          = pn.NoteText
	FROM            dbo.Diary                                                   AS d
		INNER JOIN  dbo.AspNetUsers                                             AS u ON u.ID = d.AssignedToUserID
		INNER JOIN  dbo.PrescriptionNote                                        AS pn ON pn.PrescriptionNoteID = d.PrescriptionNoteID
		INNER JOIN  dbo.PrescriptionNoteType                                    AS pnt ON pnt.PrescriptionNoteTypeID = pn.PrescriptionNoteTypeID
		CROSS APPLY
					(   SELECT TOP (@One)
								pnm.PrescriptionID
						FROM    dbo.PrescriptionNoteMapping AS pnm
						WHERE   pnm.PrescriptionNoteID = pn.PrescriptionNoteID) AS pnm
		INNER JOIN  dbo.Prescription AS p ON p.PrescriptionID = pnm.PrescriptionID
		INNER JOIN  dbo.Claim        AS c ON c.ClaimID = p.ClaimID
		INNER JOIN  dbo.Patient      AS pat ON pat.PatientID = c.PatientID
		INNER JOIN  dbo.Payor        AS pay ON pay.PayorID = c.PayorID
	WHERE (d.FollowUpDate >= @StartDate OR @StartDate IS NULL) 
		AND (d.FollowUpDate <= @EndDate OR @EndDate IS NULL)
	ORDER BY CASE WHEN @SortColumn = 'DiaryId' AND @SortDirection = 'ASC'
				THEN d.DiaryID END ASC,
			CASE WHEN @SortColumn = 'DiaryId' AND @SortDirection = 'DESC'
				THEN d.DiaryID END DESC,
			CASE WHEN @SortColumn = 'Owner' AND @SortDirection = 'ASC'
				THEN u.FirstName + ' ' + u.LastName END ASC,
			CASE WHEN @SortColumn = 'Owner' AND @SortDirection = 'DESC'
				THEN u.FirstName + ' ' + u.LastName END DESC,
			CASE WHEN @SortColumn = 'Created' AND @SortDirection = 'ASC'
				THEN d.CreatedDate END ASC,
			CASE WHEN @SortColumn = 'Created' AND @SortDirection = 'DESC'
				THEN d.CreatedDate END DESC,
			CASE WHEN @SortColumn = 'FollowUpDate' AND @SortDirection = 'ASC'
				THEN d.FollowUpDate END ASC,
			CASE WHEN @SortColumn = 'FollowUpDate' AND @SortDirection = 'DESC'
				THEN d.FollowUpDate END DESC,
			CASE WHEN @SortColumn = 'PatientName' AND @SortDirection = 'ASC'
				THEN pat.LastName + ', ' + pat.FirstName END ASC,
			CASE WHEN @SortColumn = 'PatientName' AND @SortDirection = 'DESC'
				THEN pat.LastName + ', ' + pat.FirstName END DESC,
			CASE WHEN @SortColumn = 'ClaimNumber' AND @SortDirection = 'ASC'
				THEN c.ClaimNumber END ASC,
			CASE WHEN @SortColumn = 'ClaimNumber' AND @SortDirection = 'DESC'
				THEN c.ClaimNumber END DESC,
			CASE WHEN @SortColumn = 'Type' AND @SortDirection = 'ASC'
				THEN pnt.TypeName END ASC,
			CASE WHEN @SortColumn = 'Type' AND @SortDirection = 'DESC'
				THEN pnt.TypeName END DESC,
			CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'ASC'
				THEN p.RxNumber END ASC,
			CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'DESC'
				THEN p.RxNumber END DESC,
			CASE WHEN @SortColumn = 'RxDate' AND @SortDirection = 'ASC'
				THEN p.DateFilled END ASC,
			CASE WHEN @SortColumn = 'RxDate' AND @SortDirection = 'DESC'
				THEN p.DateFilled END DESC,
			CASE WHEN @SortColumn = 'InsuranceCarrier' AND @SortDirection = 'ASC'
				THEN pay.GroupName END ASC,
			CASE WHEN @SortColumn = 'InsuranceCarrier' AND @SortDirection = 'DESC'
				THEN pay.GroupName END DESC,
			CASE WHEN @SortColumn = 'DiaryNote' AND @SortDirection = 'ASC'
				THEN pn.NoteText END ASC,
			CASE WHEN @SortColumn = 'DiaryNote' AND @SortDirection = 'DESC'
				THEN pn.NoteText END DESC,
			CASE WHEN @IsDefaultSort = 1
				THEN d.FollowUpDate END ASC
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;
	SELECT d.DiaryId
         , d.ClaimId
         , d.PrescriptionNoteId
         , d.[Owner]
         , d.Created
         , d.FollowUpDate
         , d.PatientName
         , d.ClaimNumber
         , d.[Type]
         , d.RxNumber
         , d.RxDate
         , d.InsuranceCarrier
         , d.DiaryNote 
	FROM #Diaries AS d
END
GO

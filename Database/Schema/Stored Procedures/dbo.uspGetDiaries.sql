SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	10/8/2017
	Description:	Proc that returns the grid results for the Diary page.
	Sample Execute:
					EXEC [dbo].[uspGetDiaries] 1, NULL, NULL, 'DiaryId', 'ASC', 2, 50, 0
*/
CREATE PROC [dbo].[uspGetDiaries]
(
	@IsDefaultSort BIT,
	@StartDate DATE,
	@EndDate DATE,
	@SortColumn VARCHAR(50),
	@SortDirection VARCHAR(5),
	@PageNumber INTEGER,
	@PageSize INTEGER,
	@Closed BIT,
	@UserID NVARCHAR(128),
	@NoteText VARCHAR(8000),
	@TotalRows INTEGER OUTPUT
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @One SMALLINT = 1, 
			@Zero SMALLINT = 0,
			@True BIT = CONVERT(BIT, 1),
			@False BIT = CONVERT(BIT, 0);

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
				  , [Owner]			   = u.FirstName + ' ' + u.LastName
				  , Created			   = d.CreatedDate
				  , d.FollowUpDate
				  , PatientName        = pat.LastName + ', ' + pat.FirstName
				  , c.ClaimNumber
				  , [Type]             = pnt.TypeName
				  , p.RxNumber
				  , RxDate             = p.DateFilled
				  , InsuranceCarrier   = pay.GroupName
				  , DiaryNote          = pn.NoteText
	FROM            dbo.Diary					AS d
		INNER JOIN  dbo.AspNetUsers             AS u ON u.ID = d.AssignedToUserID
		INNER JOIN  dbo.PrescriptionNote        AS pn ON pn.PrescriptionNoteID = d.PrescriptionNoteID
		INNER JOIN  dbo.PrescriptionNoteType	AS pnt ON pnt.PrescriptionNoteTypeID = pn.PrescriptionNoteTypeID
		CROSS APPLY -- Must have at least one mapping to a prescription. Order doesn't matter, 
					-- all rows should lead back to the same script.
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
		AND (u.[ID] = @UserID OR @UserID IS NULL)
		AND (pn.[NoteText] LIKE CONCAT('%', @NoteText, '%') OR @NoteText IS NULL)
		AND @One = CASE WHEN @Closed = @False AND d.DateResolved IS NULL THEN @One
					 WHEN @Closed = @True AND d.DateResolved IS NOT NULL THEN @One
					 ELSE @Zero END

	SELECT @TotalRows = COUNT(*) FROM #Diaries

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
	ORDER BY CASE WHEN @SortColumn = 'DiaryId' AND @SortDirection = 'ASC'
				THEN d.DiaryID END ASC,
			CASE WHEN @SortColumn = 'DiaryId' AND @SortDirection = 'DESC'
				THEN d.DiaryID END DESC,
			CASE WHEN @SortColumn = 'Owner' AND @SortDirection = 'ASC'
				THEN d.[Owner] END ASC,
			CASE WHEN @SortColumn = 'Owner' AND @SortDirection = 'DESC'
				THEN d.[Owner] END DESC,
			CASE WHEN @SortColumn = 'Created' AND @SortDirection = 'ASC'
				THEN d.Created END ASC,
			CASE WHEN @SortColumn = 'Created' AND @SortDirection = 'DESC'
				THEN d.Created END DESC,
			CASE WHEN @SortColumn = 'FollowUpDate' AND @SortDirection = 'ASC'
				THEN d.FollowUpDate END ASC,
			CASE WHEN @SortColumn = 'FollowUpDate' AND @SortDirection = 'DESC'
				THEN d.FollowUpDate END DESC,
			CASE WHEN @SortColumn = 'PatientName' AND @SortDirection = 'ASC'
				THEN d.PatientName END ASC,
			CASE WHEN @SortColumn = 'PatientName' AND @SortDirection = 'DESC'
				THEN d.PatientName END DESC,
			CASE WHEN @SortColumn = 'ClaimNumber' AND @SortDirection = 'ASC'
				THEN d.ClaimNumber END ASC,
			CASE WHEN @SortColumn = 'ClaimNumber' AND @SortDirection = 'DESC'
				THEN d.ClaimNumber END DESC,
			CASE WHEN @SortColumn = 'Type' AND @SortDirection = 'ASC'
				THEN d.[Type] END ASC,
			CASE WHEN @SortColumn = 'Type' AND @SortDirection = 'DESC'
				THEN d.[Type] END DESC,
			CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'ASC'
				THEN d.RxNumber END ASC,
			CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'DESC'
				THEN d.RxNumber END DESC,
			CASE WHEN @SortColumn = 'RxDate' AND @SortDirection = 'ASC'
				THEN d.RxDate END ASC,
			CASE WHEN @SortColumn = 'RxDate' AND @SortDirection = 'DESC'
				THEN d.RxDate END DESC,
			CASE WHEN @SortColumn = 'InsuranceCarrier' AND @SortDirection = 'ASC'
				THEN d.InsuranceCarrier END ASC,
			CASE WHEN @SortColumn = 'InsuranceCarrier' AND @SortDirection = 'DESC'
				THEN d.InsuranceCarrier END DESC,
			CASE WHEN @SortColumn = 'DiaryNote' AND @SortDirection = 'ASC'
				THEN d.DiaryNote END ASC,
			CASE WHEN @SortColumn = 'DiaryNote' AND @SortDirection = 'DESC'
				THEN d.DiaryNote END DESC,
			CASE WHEN @IsDefaultSort = 1
				THEN d.FollowUpDate END ASC
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;
END
GO

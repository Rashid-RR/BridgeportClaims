SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	10/11/2017
	Description:	Returns the StateID associated to a StateCode. 
					Unique constraint on StateCode ensures only one scalar value is returned.
	Sample Execute:
					SELECT dbo.udfGetStateIDByCode('MI') StateID
*/
CREATE FUNCTION [dbo].[udfGetStateIDByCode](@StateCode CHAR(2))
RETURNS INT
AS BEGIN
	RETURN  (
				SELECT  us.StateID
				FROM    dbo.UsState AS us
				WHERE   us.StateCode = @StateCode
			)
END
GO

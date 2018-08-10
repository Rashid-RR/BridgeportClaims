SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       8/7/2018
 Description:       Utility proc that helps script out the adding of Dapper parameters
 Example Execute:
                    EXECUTE [util].[uspQueryStoredProcedureParams] 'claims.uspAddOrUpdatePrescriptionStatus', 'a'
 =============================================
*/
CREATE PROC [util].[uspQueryStoredProcedureParams]
(
    @ProcName SYSNAME,
    @ObjAlias VARCHAR(100)
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SELECT  Parameter_name = [name]
       ,[Type] = TYPE_NAME(user_type_id)
       ,[Length] = max_length
       ,Prec = CASE WHEN TYPE_NAME(system_type_id) = 'uniqueidentifier' THEN
                        precision
                    ELSE
                        OdbcPrec(system_type_id, max_length, precision)
               END
       ,Scale = OdbcScale(system_type_id, scale)
       ,Param_order = parameter_id
       ,Collation = CONVERT(
                                 sysname
                                ,CASE WHEN system_type_id IN (35, 99, 167, 175, 231, 239) THEN
                                      SERVERPROPERTY('collation')END
                             )
       ,Script = 'ps.Add("' + NAME + '", ' + @ObjAlias + '.' + REPLACE(NAME, '@', '') + ', DbType.'
                 + CASE TYPE_NAME(user_type_id)WHEN 'int' THEN
                                                   'Int32'
                                               WHEN 'varchar' THEN
                                                   'AnsiString'
                                               WHEN 'nvarchar' THEN
                                                   'String'
                                               WHEN 'datetime' THEN
                                                   'DateTime'
                                               WHEN 'datetime2' THEN
                                                   'DateTime2'
                                               WHEN 'bit' THEN
                                                    'Boolean'
                   END
                 + CASE WHEN is_output = 1 THEN ', ParameterDirection.Output' ELSE '' END
                 + CASE WHEN TYPE_NAME(user_type_id) IN ('varchar', 'nvarchar') THEN
                            ', size: '
                            + CONVERT(   VARCHAR(50)
                                        ,CASE WHEN TYPE_NAME(system_type_id) = 'uniqueidentifier' THEN
                                                  PRECISION
                                              ELSE
                                                  OdbcPrec(system_type_id, max_length, PRECISION)
                                         END
                                     )
                   ELSE ''
                   END + ');'
    FROM    [sys].[parameters]
    WHERE   [object_id] = OBJECT_ID(@ProcName);
END
GO

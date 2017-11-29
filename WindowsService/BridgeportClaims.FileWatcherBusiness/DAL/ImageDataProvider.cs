using System;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.FileWatcherBusiness.Disposable;
using cm = BridgeportClaims.FileWatcherBusiness.ConfigService.ConfigService;

namespace BridgeportClaims.FileWatcherBusiness.DAL
{
    internal class ImageDataProvider
    {
        private readonly string _dbConnStr = cm.GetDbConnStr();

        internal void DeleteImageFile(string fileName) =>
            DisposableService.Using(() => new SqlConnection(_dbConnStr), conn =>
            {
                DisposableService.Using(() => new SqlCommand("[dbo].[uspDocumentInsert]", conn), cmd =>
                {
                    var claimIdParam = cmd.CreateParameter();
                    claimIdParam.Direction = ParameterDirection.Input;
                    claimIdParam.DbType = DbType.Int32;
                    claimIdParam.SqlDbType = SqlDbType.Int;
                    claimIdParam.ParameterName = "@ClaimID";
                    claimIdParam.Value = DBNull.Value;
                    cmd.Parameters.Add(claimIdParam);
                    var documentTypeIdParam = cmd.CreateParameter();
                    documentTypeIdParam.Direction = ParameterDirection.Input;
                    documentTypeIdParam.DbType = DbType.Byte;
                    documentTypeIdParam.SqlDbType = SqlDbType.TinyInt;
                    documentTypeIdParam.ParameterName = "@DocumentTypeID";
                    documentTypeIdParam.Value = DBNull.Value;
                    cmd.Parameters.Add(documentTypeIdParam);
                    var rxDateParam = cmd.CreateParameter();
                    rxDateParam.Direction = ParameterDirection.Input;
                    rxDateParam.DbType = DbType.DateTime2;
                    rxDateParam.SqlDbType = SqlDbType.DateTime2;
                    rxDateParam.ParameterName = "@RxDate";
                    rxDateParam.Value = DBNull.Value;
                    cmd.Parameters.Add(rxDateParam);
                    var rxNumberParam = cmd.CreateParameter();
                    rxNumberParam.SqlDbType = SqlDbType.VarChar;
                    rxNumberParam.Size = 100;
                    rxNumberParam.ParameterName = "@RxNumber";
                    rxNumberParam.Value = DBNull.Value;
                    rxNumberParam.Direction = ParameterDirection.Input;
                    rxNumberParam.DbType = DbType.AnsiStringFixedLength;
                    cmd.Parameters.Add(rxNumberParam);
                    var invoiceNumberParam = cmd.CreateParameter();
                    invoiceNumberParam.Direction = ParameterDirection.Input;
                    invoiceNumberParam.SqlDbType = SqlDbType.VarChar;
                    invoiceNumberParam.Size = 100;
                    invoiceNumberParam.DbType = DbType.AnsiStringFixedLength;
                    invoiceNumberParam.ParameterName = "@InvoiceNumber";
                    invoiceNumberParam.Value = DBNull.Value;
                    cmd.Parameters.Add(invoiceNumberParam);
                    var injuryDateParam = cmd.CreateParameter();
                    injuryDateParam.Direction = ParameterDirection.Input;
                    injuryDateParam.DbType = DbType.DateTime2;
                    injuryDateParam.SqlDbType = SqlDbType.DateTime2;
                    injuryDateParam.ParameterName = "@InjuryDate";
                    injuryDateParam.Value = DBNull.Value;
                    cmd.Parameters.Add(injuryDateParam);
                    var attorneyNameParam = cmd.CreateParameter();
                    attorneyNameParam.Size = 255;
                    attorneyNameParam.SqlDbType = SqlDbType.VarChar;
                    attorneyNameParam.DbType = DbType.AnsiStringFixedLength;
                    attorneyNameParam.Direction = ParameterDirection.Input;
                    attorneyNameParam.ParameterName = "@AttorneyName";
                    attorneyNameParam.Value = DBNull.Value;
                    cmd.Parameters.Add(attorneyNameParam);
                });
            });
    }
}
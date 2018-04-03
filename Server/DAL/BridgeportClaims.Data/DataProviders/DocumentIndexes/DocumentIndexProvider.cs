using System;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.DocumentIndexes
{
    public class DocumentIndexProvider : IDocumentIndexProvider
    {
        public string InvoiceNumberExists(string invoiceNumber) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
                {
                    return DisposableService.Using(() => new SqlCommand("[dbo].[uspInvoiceNumberExists]", conn),
                        cmd =>
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            var invoiceNumberParam = cmd.CreateParameter();
                            invoiceNumberParam.Value = invoiceNumber ?? (object) DBNull.Value;
                            invoiceNumberParam.ParameterName = "@InvoiceNumber";
                            invoiceNumberParam.DbType = DbType.AnsiString;
                            invoiceNumberParam.Size = 100;
                            invoiceNumberParam.SqlDbType = SqlDbType.VarChar;
                            invoiceNumberParam.Direction = ParameterDirection.Input;
                            cmd.Parameters.Add(invoiceNumberParam);
                            var existsParam = cmd.CreateParameter();
                            existsParam.Value = invoiceNumber ?? (object)DBNull.Value;
                            existsParam.ParameterName = "@FileUrl";
                            existsParam.DbType = DbType.String;
                            existsParam.SqlDbType = SqlDbType.NVarChar;
                            existsParam.Size = 500;
                            existsParam.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(existsParam);
                            if (conn.State != ConnectionState.Open)
                                conn.Open();
                            cmd.ExecuteNonQuery();
                            if (conn.State != ConnectionState.Closed)
                                conn.Close();
                            return existsParam.Value as string;
                        });
                });

        public void DeleteDocumentIndex(int documentId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                DisposableService.Using(() => new SqlCommand("[dbo].[uspDocumentIndexDelete]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var documentIdParam = cmd.CreateParameter();
                    documentIdParam.Direction = ParameterDirection.Input;
                    documentIdParam.Value = documentId;
                    documentIdParam.ParameterName = "@DocumentID";
                    documentIdParam.DbType = DbType.Int32;
                    documentIdParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(documentIdParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                });
            });

        public bool UpsertDocumentIndex(int documentId, int claimId, int documentTypeId, DateTime? rxDate,
                string rxNumber, string invoiceNumber, DateTime? injuryDate, string attorneyName, string indexedByUserId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspDocumentIndexUpsert]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var documentIdParam = cmd.CreateParameter();
                    documentIdParam.Direction = ParameterDirection.Input;
                    documentIdParam.Value = documentId;
                    documentIdParam.ParameterName = "@DocumentID";
                    documentIdParam.DbType = DbType.Int32;
                    documentIdParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(documentIdParam);
                    var claimIdParam = cmd.CreateParameter();
                    claimIdParam.Value = claimId;
                    claimIdParam.ParameterName = "@ClaimID";
                    claimIdParam.DbType = DbType.Int32;
                    claimIdParam.SqlDbType = SqlDbType.Int;
                    claimIdParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(claimIdParam);
                    var documentTypeIdParam = cmd.CreateParameter();
                    documentTypeIdParam.Value = documentTypeId;
                    documentTypeIdParam.ParameterName = "@DocumentTypeID";
                    documentTypeIdParam.DbType = DbType.Int32;
                    documentTypeIdParam.SqlDbType = SqlDbType.Int;
                    documentTypeIdParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(documentTypeIdParam);
                    var rxDateParam = cmd.CreateParameter();
                    rxDateParam.Value = rxDate ?? (object) DBNull.Value;
                    rxDateParam.ParameterName = "@RxDate";
                    rxDateParam.DbType = DbType.DateTime2;
                    rxDateParam.SqlDbType = SqlDbType.DateTime2;
                    rxDateParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(rxDateParam);
                    var rxNumberParam = cmd.CreateParameter();
                    rxNumberParam.Value = rxNumber ?? (object) DBNull.Value;
                    rxNumberParam.ParameterName = "@RxNumber";
                    rxNumberParam.DbType = DbType.AnsiString;
                    rxNumberParam.SqlDbType = SqlDbType.VarChar;
                    rxNumberParam.Size = 100;
                    rxNumberParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(rxNumberParam);
                    var invoiceNumberParam = cmd.CreateParameter();
                    invoiceNumberParam.Value = invoiceNumber ?? (object) DBNull.Value;
                    invoiceNumberParam.Direction = ParameterDirection.Input;
                    invoiceNumberParam.DbType = DbType.AnsiString;
                    invoiceNumberParam.SqlDbType = SqlDbType.VarChar;
                    invoiceNumberParam.Size = 100;
                    invoiceNumberParam.ParameterName = "@InvoiceNumber";
                    cmd.Parameters.Add(invoiceNumberParam);
                    var injuryDateParam = cmd.CreateParameter();
                    injuryDateParam.Value = injuryDate ?? (object)DBNull.Value;
                    injuryDateParam.ParameterName = "@InjuryDate";
                    injuryDateParam.DbType = DbType.DateTime2;
                    injuryDateParam.SqlDbType = SqlDbType.DateTime2;
                    injuryDateParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(injuryDateParam);
                    var attorneyNameParam = cmd.CreateParameter();
                    attorneyNameParam.Value = attorneyName ?? (object) DBNull.Value;
                    attorneyNameParam.DbType = DbType.AnsiString;
                    attorneyNameParam.SqlDbType = SqlDbType.VarChar;
                    attorneyNameParam.Size = 255;
                    attorneyNameParam.ParameterName = "@AttorneyName";
                    attorneyNameParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(attorneyNameParam);
                    var indexedByUserIdParam = cmd.CreateParameter();
                    indexedByUserIdParam.DbType = DbType.String;
                    indexedByUserIdParam.SqlDbType = SqlDbType.NVarChar;
                    indexedByUserIdParam.Size = 128;
                    indexedByUserIdParam.Value = indexedByUserId ?? (object) DBNull.Value;
                    indexedByUserIdParam.ParameterName = "@IndexedByUserID";
                    indexedByUserIdParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(indexedByUserIdParam);
                    var existsParam = cmd.CreateParameter();
                    existsParam.DbType = DbType.Boolean;
                    existsParam.SqlDbType = SqlDbType.Bit;
                    existsParam.Direction = ParameterDirection.Output;
                    existsParam.ParameterName = "@Exists";
                    cmd.Parameters.Add(existsParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return existsParam.Value as bool? ?? default(bool);
                });
            });

        public void InsertInvoiceIndex(int documentId, string invoiceNumber, string userId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                DisposableService.Using(() => new SqlCommand("[dbo].[uspInvoiceIndexInsert]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var documentIdParam = cmd.CreateParameter();
                    documentIdParam.Value = documentId;
                    documentIdParam.ParameterName = "@DocumentID";
                    documentIdParam.DbType = DbType.Int32;
                    documentIdParam.SqlDbType = SqlDbType.Int;
                    documentIdParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(documentIdParam);
                    var invoiceNumberParam = cmd.CreateParameter();
                    invoiceNumberParam.Value = invoiceNumber ?? (object) DBNull.Value;
                    invoiceNumberParam.DbType = DbType.AnsiString;
                    invoiceNumberParam.SqlDbType = SqlDbType.VarChar;
                    invoiceNumberParam.Size = 100;
                    invoiceNumberParam.Direction = ParameterDirection.Input;
                    invoiceNumberParam.ParameterName = "@InvoiceNumber";
                    cmd.Parameters.Add(invoiceNumberParam);
                    var modifiedByUserIdParam = cmd.CreateParameter();
                    modifiedByUserIdParam.Value = userId ?? (object) DBNull.Value;
                    modifiedByUserIdParam.DbType = DbType.String;
                    modifiedByUserIdParam.SqlDbType = SqlDbType.NVarChar;
                    modifiedByUserIdParam.Size = 128;
                    modifiedByUserIdParam.Direction = ParameterDirection.Input;
                    modifiedByUserIdParam.ParameterName = "@ModifiedByUserID";
                    cmd.Parameters.Add(modifiedByUserIdParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                });
            });
    }
}
using System;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.DocumentIndexes
{
    public class DocumentIndexProvider : IDocumentIndexProvider
    {
        public IndexedInvoiceDto GetIndexedInvoiceData(string invoiceNumber) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
                {
                    return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetIndexedInvoiceData]", conn),
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
                            IndexedInvoiceDto indexedInvoiceDto = null;
                            if (conn.State != ConnectionState.Open)
                                conn.Open();
                            DisposableService.Using(cmd.ExecuteReader, reader =>
                            {
                                var documentIdOrdinal = reader.GetOrdinal("DocumentId");
                                var fileUrlOrdinal = reader.GetOrdinal("FileUrl");
                                var invoiceNumberIsAlreadyIndexedOrdinal = reader.GetOrdinal("InvoiceNumberIsAlreadyIndexed");
                                var fileNameOrdinal = reader.GetOrdinal("FileName");
                                while (reader.Read())
                                {
                                    indexedInvoiceDto = new IndexedInvoiceDto
                                    {
                                        DocumentId = !reader.IsDBNull(documentIdOrdinal) ? reader.GetInt32(documentIdOrdinal) : (int?) null,
                                        FileUrl = !reader.IsDBNull(fileUrlOrdinal) ? reader.GetString(fileUrlOrdinal) : null,
                                        FileName = !reader.IsDBNull(fileNameOrdinal) ? reader.GetString(fileNameOrdinal) : null,
                                        InvoiceNumberIsAlreadyIndexed = !reader.IsDBNull(invoiceNumberIsAlreadyIndexedOrdinal) && reader.GetBoolean(invoiceNumberIsAlreadyIndexedOrdinal)
                                    };
                                }
                            });
                            if (conn.State != ConnectionState.Closed)
                                conn.Close();
                            return indexedInvoiceDto;
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
                    return existsParam.Value as bool? ?? default;
                });
            });

        public bool InsertInvoiceIndex(int documentId, string invoiceNumber, string userId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspInvoiceIndexInsert]", conn), cmd =>
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
                    var alreadyExistsParam = cmd.CreateParameter();
                    alreadyExistsParam.DbType = DbType.Boolean;
                    alreadyExistsParam.SqlDbType = SqlDbType.Bit;
                    alreadyExistsParam.Direction = ParameterDirection.Output;
                    alreadyExistsParam.ParameterName = "@AlreadyExists";
                    cmd.Parameters.Add(alreadyExistsParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    if (!(alreadyExistsParam.Value is bool retVal))
                        throw new Exception("Error, could not retrieve the value of the output parameter");
                    return retVal as bool? ?? false;
                });
            });

        public bool InsertCheckIndex(int documentId, string checkNumber, string userId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string output = "@AlreadyExists";
                var ps = new DynamicParameters();
                ps.Add("@DocumentID", documentId, DbType.Int32);
                ps.Add("@ModifiedByUserID", userId, DbType.String, size: 128);
                ps.Add("@CheckNumber", checkNumber, DbType.AnsiString, size: 100);
                ps.Add(output, dbType: DbType.Boolean, direction: ParameterDirection.Output);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute("[dbo].[uspCheckIndexInsert]", ps, commandType: CommandType.StoredProcedure);
                var retVal = ps.Get<bool>(output);
                return retVal;
            });
    }
}
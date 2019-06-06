using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Patients
{
    public class PatientProvider : IPatientProvider
    {
        private const string DefaultString = "{NULL}";

        public IEnumerable<PatientAddressDto> GetPatientAddressReport() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "rpt.uspPatientAddressEdit";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<PatientAddressDto>(sp, commandType: CommandType.StoredProcedure);
            });

        public void UpdatePatientAddress(int patientId, string modifiedByUserId, string lastName, string firstName, string address1,
            string address2, string city, string postalCode, string stateName, string phoneNumber, string emailAddress) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "dbo.uspEditPatient";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@PatientID", patientId, DbType.Int32);
                ps.Add("@ModifiedByUserID", modifiedByUserId, DbType.String, size: 128);
                if (lastName != DefaultString)
                {
                    ps.Add("@LastName", lastName, DbType.AnsiString, size: 155);
                }
                if (firstName != DefaultString)
                {
                    ps.Add("@FirstName", firstName, DbType.AnsiString, size: 155);
                }
                if (address1 != DefaultString)
                {
                    ps.Add("@Address1", address1, DbType.AnsiString, size: 255);
                }
                if (address2 != DefaultString)
                {
                    ps.Add("@Address2", address2, DbType.AnsiString, size: 255);
                }
                if (city != DefaultString)
                {
                    ps.Add("@City", city, DbType.AnsiString, size: 155);
                }
                if (postalCode != DefaultString)
                {
                    ps.Add("@PostalCode", postalCode, DbType.AnsiString, size: 100);
                }
                if (stateName != DefaultString)
                {
                    ps.Add("@StateName", stateName, DbType.AnsiString, size: 64);
                }
                if (phoneNumber != DefaultString)
                {
                    ps.Add("@PhoneNumber", phoneNumber, DbType.AnsiString, size: 30);
                }
                if (emailAddress != DefaultString)
                {
                    ps.Add("@EmailAddress", emailAddress, DbType.AnsiString, size: 155);
                }
                conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
            });
    }
}
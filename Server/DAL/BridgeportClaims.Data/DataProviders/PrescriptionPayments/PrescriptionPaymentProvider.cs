using System;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.PrescriptionPayments
{
    public class PrescriptionPaymentProvider : IPrescriptionPaymentProvider
    {
        public void DeletePrescriptionPayment(int prescriptionPaymentId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                const string sp = "[dbo].[uspPrescriptionPaymentDelete]";
                conn.Execute(sp, new {PrescriptionPaymentID = prescriptionPaymentId},
                    commandType: CommandType.StoredProcedure);
            });

        public void UpdatePrescriptionPayment(int prescriptionPaymentId, string checkNumber, decimal amountPaid,
            DateTime? datePosted, int prescriptionId, string userId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspUpdatePrescriptionPayment]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute(sp, new
                {
                    PrescriptionPaymentID = prescriptionPaymentId,
                    CheckNumber = checkNumber,
                    AmountPaid = amountPaid,
                    DatePosted = datePosted,
                    UserID = userId
                }, commandType: CommandType.StoredProcedure);
            });
    }
}   
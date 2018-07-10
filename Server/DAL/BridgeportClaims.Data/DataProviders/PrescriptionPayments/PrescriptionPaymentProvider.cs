using System;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.PrescriptionPayments
{
    public class PrescriptionPaymentProvider : IPrescriptionPaymentProvider
    {
        private readonly Lazy<IRepository<PrescriptionPayment>> _prescriptionPaymentRepository;

        public PrescriptionPaymentProvider(Lazy<IRepository<PrescriptionPayment>> prescriptionPaymentRepository)
        {
            _prescriptionPaymentRepository = prescriptionPaymentRepository;
        }

        public void DeletePrescriptionPayment(int prescriptionPaymentId)
        {
            _prescriptionPaymentRepository.Value.Delete(_prescriptionPaymentRepository.Value.Get(prescriptionPaymentId));
        }

        public void UpdatePrescriptionPayment(int prescriptionPaymentId, string checkNumber, decimal amountPaid,
            DateTime? datePosted, int prescriptionId, string userId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspUpdatePrescriptionPayment]";
                conn.Open();
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
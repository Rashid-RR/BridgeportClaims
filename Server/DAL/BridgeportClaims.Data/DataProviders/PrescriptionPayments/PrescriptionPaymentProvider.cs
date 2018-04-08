using System;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Data.DataProviders.PrescriptionPayments
{
    public class PrescriptionPaymentProvider : IPrescriptionPaymentProvider
    {
        private readonly Lazy<IRepository<PrescriptionPayment>> _prescriptionPaymentRepository;
        private readonly Lazy<IRepository<Prescription>> _prescriptionRepository;
        private readonly Lazy<IRepository<AspNetUsers>> _usersRepository;

        public PrescriptionPaymentProvider(Lazy<IRepository<PrescriptionPayment>> prescriptionPaymentRepository, 
            Lazy<IRepository<Prescription>> prescriptionRepository, Lazy<IRepository<AspNetUsers>> usersRepository)
        {
            _prescriptionPaymentRepository = prescriptionPaymentRepository;
            _prescriptionRepository = prescriptionRepository;
            _usersRepository = usersRepository;
        }

        public void DeletePrescriptionPayment(int prescriptionPaymentId)
        {
            _prescriptionPaymentRepository.Value.Delete(_prescriptionPaymentRepository.Value.Get(prescriptionPaymentId));
        }

        public void UpdatePrescriptionPayment(int prescriptionPaymentId, string checkNumber, decimal amountPaid,
            DateTime? datePosted, int prescriptionId, string userId)
        {
            var p = _prescriptionPaymentRepository.Value.Get(prescriptionPaymentId);
            if (null == p)
                throw new Exception($"Error, cannot find a Prescription Payment record with an ID of {prescriptionPaymentId}");
            var now = DateTime.UtcNow;
            p.CheckNumber = checkNumber;
            p.AmountPaid = amountPaid;
            p.DatePosted = datePosted;
            p.Prescription = _prescriptionRepository.Value.Get(prescriptionId);
            p.CreatedOnUtc = now;
            p.UpdatedOnUtc = now;
            p.UserId = _usersRepository.Value.Get(userId);
            _prescriptionPaymentRepository.Value.Save(p);
        }
    }
}   
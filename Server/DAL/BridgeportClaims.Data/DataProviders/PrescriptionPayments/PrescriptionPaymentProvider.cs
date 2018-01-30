using System;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Data.DataProviders.PrescriptionPayments
{
    public class PrescriptionPaymentProvider : IPrescriptionPaymentProvider
    {
        private readonly IRepository<PrescriptionPayment> _prescriptionPaymentRepository;
        private readonly IRepository<Prescription> _prescriptionRepository;
        private readonly IRepository<AspNetUsers> _usersRepository;

        public PrescriptionPaymentProvider(IRepository<PrescriptionPayment> prescriptionPaymentRepository, 
            IRepository<Prescription> prescriptionRepository, IRepository<AspNetUsers> usersRepository)
        {
            _prescriptionPaymentRepository = prescriptionPaymentRepository;
            _prescriptionRepository = prescriptionRepository;
            _usersRepository = usersRepository;
        }

        public void DeletePrescriptionPayment(int prescriptionPaymentId)
        {
            _prescriptionPaymentRepository.Delete(_prescriptionPaymentRepository.Get(prescriptionPaymentId));
        }

        public void UpdatePrescriptionPayment(int prescriptionPaymentId, string checkNumber, decimal amountPaid,
            DateTime? datePosted, int prescriptionId, string userId)
        {
            var p = _prescriptionPaymentRepository.Get(prescriptionPaymentId);
            if (null == p)
                throw new Exception($"Error, cannot find a Prescription Payment record with an ID of {prescriptionPaymentId}");
            var now = DateTime.UtcNow;
            p.CheckNumber = checkNumber;
            p.AmountPaid = amountPaid;
            p.DatePosted = datePosted;
            p.Prescription = _prescriptionRepository.Get(prescriptionId);
            p.CreatedOnUtc = now;
            p.UpdatedOnUtc = now;
            p.UserId = _usersRepository.Get(userId);
            _prescriptionPaymentRepository.Save(p);
        }
    }
}   
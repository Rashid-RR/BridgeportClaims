using System;
using BridgeportClaims.Data.Enums;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Data.DataProviders.Prescriptions
{
    public class PrescriptionsProvider : IPrescriptionsProvider
    {
        private readonly IRepository<Prescription> _prescriptionRepository;
        private readonly IRepository<PrescriptionStatus> _prescriptionStatusRepository;

        public PrescriptionsProvider(IRepository<Prescription> prescriptionRepository, IRepository<PrescriptionStatus> prescriptionStatusRepository)
        {
            _prescriptionRepository = prescriptionRepository;
            _prescriptionStatusRepository = prescriptionStatusRepository;
        }

        public EntityOperation AddOrUpdatePrescriptionStatus(int prescriptionId, int prescriptionStatusId)
        {
            var prescription = _prescriptionRepository.Get(prescriptionId);
            if (null == prescription)
                throw new ArgumentNullException(nameof(prescription));
            var prescriptionStatus = _prescriptionStatusRepository.Get(prescriptionStatusId);
            var op = null == prescription.PrescriptionStatus ? EntityOperation.Add : EntityOperation.Update;
            prescription.PrescriptionStatus = prescriptionStatus ??
                                              throw new ArgumentNullException(nameof(prescriptionStatus));
            prescription.UpdatedOnUtc = DateTime.UtcNow;
            _prescriptionRepository.Update(prescription);
            return op;
        }
    }
}
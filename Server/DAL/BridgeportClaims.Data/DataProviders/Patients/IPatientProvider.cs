using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Patients
{
    public interface IPatientProvider
    {
        IEnumerable<PatientAddressDto> GetPatientAddressReport();
        void UpdatePatientAddress(int patientId, string modifiedByUserId, string lastName, string firstName, string address1,
            string address2, string city, string postalCode, string stateName, string phoneNumber, string emailAddress);
    }
}
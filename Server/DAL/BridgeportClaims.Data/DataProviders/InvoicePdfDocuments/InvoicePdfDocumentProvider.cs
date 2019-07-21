using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Models;
using BridgeportClaims.Data.Dtos;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.InvoicePdfDocuments
{
    public class InvoicePdfDocumentProvider : IInvoicePdfDocumentProvider
    {
        public IEnumerable<InvoicePdfDto> GetInvoicePdfDocument() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetInvoicingPdfData]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<InvoicePdfDto>(sp, commandType: CommandType.StoredProcedure);
            });

        public InvoicePdfModel GetInvoicePdfModel(IList<InvoicePdfDto> data)
        {
            var model = data.GroupBy(d => new
                {
                    d.ClaimId,
                    d.BillToName,
                    d.BillToAddress1,
                    d.BillToAddress2,
                    d.BillToCity,
                    d.BillToStateCode,
                    d.BillToPostalCode,
                    d.InvoiceNumber,
                    d.InvoiceDate,
                    d.ClaimNumber,
                    d.PatientLastName,
                    d.PatientFirstName,
                    d.PatientAddress1,
                    d.PatientAddress2,
                    d.PatientCity,
                    d.PatientStateCode,
                    d.PatientPostalCode,
                    d.PatientPhoneNumber,
                    d.DateOfBirthDay,
                    d.DateOfBirthMonth,
                    d.DateOfBirthYear,
                    d.IsMale,
                    d.IsFemale,
                    d.DateOfInjuryDay,
                    d.DateOfInjuryMonth,
                    d.DateOfInjuryYear
                })
                .Select(gcs => new InvoicePdfModel
                {
                    BillToName = gcs.Key.BillToName,
                    BillToAddress1 = gcs.Key.BillToAddress1,
                    BillToAddress2 = gcs.Key.BillToAddress2,
                    BillToCity = gcs.Key.BillToCity,
                    BillToStateCode = gcs.Key.BillToStateCode,
                    BillToPostalCode = gcs.Key.BillToPostalCode,
                    InvoiceNumber = gcs.Key.InvoiceNumber,
                    InvoiceDate = gcs.Key.InvoiceDate,
                    ClaimNumber = gcs.Key.ClaimNumber,
                    PatientLastName = gcs.Key.PatientLastName,
                    PatientFirstName = gcs.Key.PatientFirstName,
                    PatientAddress1 = gcs.Key.PatientAddress1,
                    PatientAddress2 = gcs.Key.PatientAddress2,
                    PatientCity = gcs.Key.PatientCity,
                    PatientStateCode = gcs.Key.PatientStateCode,
                    PatientPostalCode = gcs.Key.PatientPostalCode,
                    PatientPhoneNumber = gcs.Key.PatientPhoneNumber,
                    DateOfBirthDay = gcs.Key.DateOfBirthDay,
                    DateOfBirthMonth = gcs.Key.DateOfBirthMonth,
                    DateOfBirthYear = gcs.Key.DateOfBirthYear,
                    IsMale = gcs.Key.IsMale,
                    IsFemale = gcs.Key.IsFemale,
                    DateOfInjuryDay = gcs.Key.DateOfInjuryDay,
                    DateOfInjuryMonth = gcs.Key.DateOfInjuryMonth,
                    DateOfInjuryYear = gcs.Key.DateOfInjuryYear,
                    Scripts = GetInvoicePrescriptionPdfModels(gcs.ToList())
                });
            return model.SingleOrDefault();
        }

        /// <summary>
        /// I Hate Automapper
        /// </summary>
        /// <param name="invoicePdfDtos"></param>
        /// <returns></returns>
        private static IList<InvoicePrescriptionPdfModel> GetInvoicePrescriptionPdfModels(IEnumerable<InvoicePdfDto> invoicePdfDtos)
        {
            var query = invoicePdfDtos.Select(c => new InvoicePrescriptionPdfModel
            {
                PrescriptionId = c.PrescriptionId,
                Prescriber = c.Prescriber,
                PrescriberNpi = c.PrescriberNpi,
                DateFilledDay = c.DateFilledDay,
                DateFilledMonth = c.DateFilledMonth,
                DateFilledYear = c.DateFilledYear,
                Ndc = c.Ndc,
                LabelName = c.LabelName,
                RxNumber = c.RxNumber,
                BilledAmount = c.BilledAmount,
                BilledAmountDollars = c.BilledAmountDollars,
                BilledAmountCents = c.BilledAmountCents,
                Quantity = c.Quantity,
                PharmacyName = c.PharmacyName,
                Address1 = c.Address1,
                Address2 = c.Address2,
                City = c.City,
                PharmacyState = c.PharmacyState,
                PostalCode = c.PostalCode,
                FederalTin = c.FederalTin,
                Npi = c.Npi,
                Nabp = c.Nabp
            });
            return query.ToList();
        }
    }
}
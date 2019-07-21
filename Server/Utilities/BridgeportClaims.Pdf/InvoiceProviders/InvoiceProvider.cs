using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Common.Models;
using iTextSharp.text;
using NLog;
using iTextSharp.text.pdf;

namespace BridgeportClaims.Pdf.InvoiceProviders
{
    public class InvoiceProvider : IInvoiceProvider
    {
        private const int DefaultFontSize = 9;
        private const int AlternateFontSize = 7;
        private const int DefaultFontStyle = 0;
        private static readonly BaseFont BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        public bool ProcessInvoice(InvoicePdfModel data, string targetPath)
        {
            var success = true;
            DisposableService.Using(() => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("BridgeportClaims.Pdf.EmbeddedResources.Invoice.pdf"), resourceStream =>
                {
                    var reader = new PdfReader(resourceStream.ToBytes());
                    reader.SelectPages("1");
                    var stamper = new PdfStamper(reader, new FileStream(targetPath, FileMode.Create));
                    try
                    {
                        var contentByte = stamper.GetOverContent(1);
                        StampText(data.BillToName, 341, 770, contentByte);
                        StampText(data.BillToAddress1, 341, 759.75f, contentByte);
                        if (data.BillToAddress2.IsNotNullOrWhiteSpace())
                        {
                            StampText(data.BillToAddress2, 341, 749.5f, contentByte);
                            StampText($"{data.BillToCity}, {data.BillToStateCode} {data.BillToPostalCode}", 341, 739.25f, contentByte);
                        }
                        else
                        {
                            StampText($"{data.BillToCity}, {data.BillToStateCode} {data.BillToPostalCode}", 341, 749.5f, contentByte);
                        }
                        StampText($"Invoice#-1: {data.InvoiceNumber}", 240, 683, contentByte);
                        StampText($"Invoice Date: {data.InvoiceDate}", 340, 683, contentByte);
                        StampText("X", 331.7f, 656.7f, contentByte, 8);
                        StampText(data.ClaimNumber, 362, 655.8f, contentByte, AlternateFontSize);
                        const float leftAxis = 66.7f;
                        StampText($"{data.PatientLastName}, {data.PatientFirstName}", leftAxis, 636, contentByte, AlternateFontSize);
                        StampText(data.DateOfBirthMonth, 248, 636, contentByte, AlternateFontSize);
                        StampText(data.DateOfBirthDay, 268, 636, contentByte, AlternateFontSize);
                        StampText(data.DateOfBirthYear, 291, 636, contentByte, AlternateFontSize);
                        if (data.IsMale.HasValue && data.IsMale.Value)
                        {
                            // Check Male
                            StampText("X", 313.5f, 636.2f, contentByte, 8);
                        }
                        else
                        {
                            // Check Female
                            StampText("X", 343.8f, 636.2f, contentByte, 8);
                        }
                        // Address
                        var address = data.PatientAddress2.IsNotNullOrWhiteSpace() ? $"{data.PatientAddress1}, {data.PatientAddress2}" : data.PatientAddress1; 
                        StampText(address, leftAxis, 614.8f, contentByte, AlternateFontSize);
                        // Stamp Self
                        StampText("X", 259.3f, 614.8f, contentByte, 8);
                        StampText(data.PatientCity, leftAxis, 594.6f, contentByte, AlternateFontSize);
                        StampText(data.PatientStateCode, 218, 594.6f, contentByte, AlternateFontSize);
                        const float zipPhoneYAxis = 572.8f;
                        StampText(data.PatientPostalCode, leftAxis, zipPhoneYAxis, contentByte, AlternateFontSize);
                        if (data.PatientPhoneNumber.IsNotNullOrWhiteSpace())
                        {
                            if (data.PatientPhoneNumber.Length == 10)
                            {
                                var areaCode = data.PatientPhoneNumber.Substring(0, 3);
                                var phoneNumber = data.PatientPhoneNumber.Substring(3, 7);
                                var phonePrefix = phoneNumber.Substring(0, 3);
                                var phoneSuffix = phoneNumber.Substring(3, 4);
                                StampText($"{areaCode}    {phonePrefix}-{phoneSuffix}", 154.5f, zipPhoneYAxis, contentByte, AlternateFontSize);
                            }
                            else
                            {
                                StampText($"    {data.PatientPhoneNumber}", 154.5f, zipPhoneYAxis, contentByte, AlternateFontSize);
                            }
                        }
                        const float dobYAxis = 530;
                        StampText(data.DateOfBirthMonth, 383, dobYAxis, contentByte, AlternateFontSize);
                        StampText(data.DateOfBirthDay, 401, dobYAxis, contentByte, AlternateFontSize);
                        StampText(data.DateOfBirthYear, 425, dobYAxis, contentByte, AlternateFontSize);
                        const float billToNameYAxis = 490.5f;
                        StampText(data.BillToName, 361.5f, billToNameYAxis, contentByte, AlternateFontSize);
                        // Auto-Accident Yes
                        StampText("X", 271.7f, 510.8f, contentByte, 8);
                        const string availableUponRequest = "AVAILABLE UPON REQUEST";
                        const float availableUponRequestYAxis = 430;
                        StampText(availableUponRequest, 101, availableUponRequestYAxis, contentByte, AlternateFontSize);
                        StampText(availableUponRequest, 399, availableUponRequestYAxis,contentByte,7);
                        const float dateOfInjuryYAxis = 406.2f;
                        StampText(data.DateOfInjuryMonth, 75.3f, dateOfInjuryYAxis, contentByte, AlternateFontSize);
                        StampText(data.DateOfInjuryDay, 94, dateOfInjuryYAxis, contentByte, AlternateFontSize);
                        StampText(data.DateOfInjuryYear, 116.2f, dateOfInjuryYAxis, contentByte, AlternateFontSize);
                        const float prescriberLineYAxis = 386.4f;
                        StampText(data.Scripts[0].Prescriber, 85, prescriberLineYAxis, contentByte, AlternateFontSize);
                        StampText(data.Scripts[0].PrescriberNpi, 256, prescriberLineYAxis, contentByte, AlternateFontSize);
                        const string diagnosis = "V49.9XXA";
                        StampText(diagnosis, 78.7f, 345, contentByte, AlternateFontSize);
                        // Script 1
                        const float ndcLabelNameXAxis = 213.5f;
                        const float scriptOneNdcRxNumberYAxis = 292.5f;
                        StampText(data.Scripts[0].Ndc, ndcLabelNameXAxis, scriptOneNdcRxNumberYAxis, contentByte, AlternateFontSize);
                        const float scriptOneYAxis = 283.1f;
                        // Rx Date
                        StampText(data.Scripts[0].DateFilledMonth, leftAxis + 2, scriptOneYAxis, contentByte, AlternateFontSize);
                        StampText(data.Scripts[0].DateFilledDay, 87, scriptOneYAxis, contentByte, AlternateFontSize);
                        StampText(data.Scripts[0].DateFilledYear, 104, scriptOneYAxis, contentByte, AlternateFontSize);
                        // Rx Date again
                        StampText(data.Scripts[0].DateFilledMonth, 123, scriptOneYAxis, contentByte, AlternateFontSize);
                        StampText(data.Scripts[0].DateFilledDay, 140, scriptOneYAxis, contentByte, AlternateFontSize);
                        StampText(data.Scripts[0].DateFilledYear, 158.5f, scriptOneYAxis, contentByte, AlternateFontSize);
                        StampText(data.Scripts[0].LabelName, ndcLabelNameXAxis, scriptOneYAxis, contentByte, AlternateFontSize);
                        const float rxNumberXAxis = 331.0f;
                        StampText(data.Scripts[0].RxNumber, rxNumberXAxis, scriptOneNdcRxNumberYAxis, contentByte, AlternateFontSize);
                        const string a = "A";
                        StampText(a, rxNumberXAxis + 9, scriptOneYAxis, contentByte, AlternateFontSize);
                        var billedAmountDollars = data.Scripts[0].BilledAmountDollars;
                        var billedAmountCents = data.Scripts[0].BilledAmountCents;
                        const float billedAmountDollarsXAxis = 370.5f;
                        if (billedAmountDollars.HasValue && billedAmountDollars.Value > 0)
                        {
                            StampText(billedAmountDollars.Value.ToString(), billedAmountDollarsXAxis, scriptOneYAxis, contentByte, AlternateFontSize);
                        }
                        const float billedAmountCentsXAxis = 400.5f;
                        if (billedAmountCents.HasValue)
                        {
                            var cents = billedAmountCents.Value.ToString(CultureInfo.InvariantCulture);
                            StampText(cents, billedAmountCentsXAxis, scriptOneYAxis, contentByte, AlternateFontSize);
                        }
                        StampText(data.Scripts[0].Quantity.ToString(CultureInfo.InvariantCulture), 430.5f, scriptOneYAxis, contentByte, AlternateFontSize);
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        Logger.Value.Error(ex);
                    }
                    finally
                    {
                        stamper.Close();
                    }
                });
            return success;
        }

        private static void StampText(string text, float x, float y, PdfContentByte contentByte, int fontSize = DefaultFontSize)
        {
            ColumnText.ShowTextAligned(contentByte, Element.ALIGN_LEFT,
                new Phrase(text, new Font(BaseFont, fontSize, DefaultFontStyle, BaseColor.BLACK)), x, y, 0);
        }
    }
}

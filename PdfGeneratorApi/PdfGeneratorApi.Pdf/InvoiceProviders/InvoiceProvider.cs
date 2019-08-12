using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NLog;
using PdfGeneratorApi.Common.Disposable;
using PdfGeneratorApi.Common.Extensions;
using PdfGeneratorApi.Common.Models;

namespace PdfGeneratorApi.Pdf.InvoiceProviders
{
    public class InvoiceProvider : IInvoiceProvider
    {
        private const string X = "X";
        private const string ZeroOne = "01";
        private const int DefaultFontSize = 9;
        private const int AlternateFontSize = 7;
        private const float LetterLength = 3.8f;
        private const int DefaultFontStyle = 0;
        private const string Na = "NA";
        private const float BilledAmountCentsXAxis = 399.0f;
        private static readonly BaseFont BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        public bool ProcessInvoice(InvoicePdfModel data, string targetPath)
        {
            var success = true;
            DisposableService.Using(() => Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("PdfGeneratorApi.Pdf.EmbeddedResources.Invoice.pdf"), resourceStream =>
            {
                DisposableService.Using(() => File.Create(targetPath),
                    output => { CopyStream(resourceStream, output); });
                var reader = new PdfReader(resourceStream.ToBytes());
                reader.SelectPages("1");
                var stamper = new PdfStamper(reader, new FileStream(targetPath, FileMode.Create));
                try
                {
                    PdfContentByte contentByte = stamper.GetOverContent(1);
                    StampText(data.BillToName, 341, 770, contentByte);
                    StampText(data.BillToAddress1, 341, 759.75f, contentByte);
                    if (data.BillToAddress2.IsNotNullOrWhiteSpace())
                    {
                        StampText(data.BillToAddress2, 341, 749.5f, contentByte);
                        StampText($"{data.BillToCity}, {data.BillToStateCode} {data.BillToPostalCode}", 341, 739.25f,
                            contentByte);
                    }
                    else
                    {
                        StampText($"{data.BillToCity}, {data.BillToStateCode} {data.BillToPostalCode}", 341, 749.5f,
                            contentByte);
                    }

                    StampText($"Invoice #: {data.InvoiceNumber}", 240, 683, contentByte);
                    StampText($"Invoice Date: {data.InvoiceDate}", 340, 683, contentByte);
                    StampText(X, 331.7f, 656.7f, contentByte, 8);
                    StampText(data.ClaimNumber, 362, 655.8f, contentByte, AlternateFontSize);
                    const float leftAxis = 66.7f;
                    StampText($"{data.PatientLastName}, {data.PatientFirstName}", leftAxis, 636, contentByte,
                        AlternateFontSize);
                    StampText(data.DateOfBirthMonth, 248, 636, contentByte, AlternateFontSize);
                    StampText(data.DateOfBirthDay, 268, 636, contentByte, AlternateFontSize);
                    StampText(data.DateOfBirthYear, 291, 636, contentByte, AlternateFontSize);
                    if (data.IsMale.HasValue && data.IsMale.Value)
                    {
                        // Check Male
                        StampText(X, 313.5f, 636.2f, contentByte, 8);
                    }
                    else
                    {
                        // Check Female
                        StampText(X, 343.8f, 636.2f, contentByte, 8);
                    }

                    // Address
                    var address = data.PatientAddress2.IsNotNullOrWhiteSpace()
                        ? $"{data.PatientAddress1}, {data.PatientAddress2}"
                        : data.PatientAddress1;
                    StampText(address, leftAxis, 614.8f, contentByte, AlternateFontSize);
                    // Stamp Self
                    StampText(X, 259.3f, 614.8f, contentByte, 8);
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
                            StampText($"{areaCode}    {phonePrefix}-{phoneSuffix}", 154.5f, zipPhoneYAxis, contentByte,
                                AlternateFontSize);
                        }
                        else
                        {
                            StampText($"    {data.PatientPhoneNumber}", 154.5f, zipPhoneYAxis, contentByte,
                                AlternateFontSize);
                        }
                    }

                    const float dobYAxis = 530;
                    StampText(data.DateOfBirthMonth, 383, dobYAxis, contentByte, AlternateFontSize);
                    StampText(data.DateOfBirthDay, 401, dobYAxis, contentByte, AlternateFontSize);
                    StampText(data.DateOfBirthYear, 425, dobYAxis, contentByte, AlternateFontSize);
                    // Optionally check Male or Female
                    if (data.IsMale.HasValue && data.IsMale.Value)
                    {
                        StampText(X, 470.4f, dobYAxis + 2, contentByte, 8);
                    }
                    else
                    {
                        StampText(X, 512.9f, dobYAxis + 2, contentByte, 8);
                    }
                    const float billToNameYAxis = 490.5f;
                    StampText(data.BillToName, 361.5f, billToNameYAxis, contentByte, AlternateFontSize);
                    // Auto-Accident Yes
                    StampText(X, 271.7f, 510.8f, contentByte, 8);
                    const string availableUponRequest = "AVAILABLE UPON REQUEST";
                    const float availableUponRequestYAxis = 430;
                    StampText(availableUponRequest, 101, availableUponRequestYAxis, contentByte, AlternateFontSize);
                    StampText(availableUponRequest, 399, availableUponRequestYAxis, contentByte, 7);
                    const float dateOfInjuryYAxis = 406.2f;
                    StampText(data.DateOfInjuryMonth, 75.3f, dateOfInjuryYAxis, contentByte, AlternateFontSize);
                    StampText(data.DateOfInjuryDay, 94, dateOfInjuryYAxis, contentByte, AlternateFontSize);
                    StampText(data.DateOfInjuryYear, 116.2f, dateOfInjuryYAxis, contentByte, AlternateFontSize);
                    const float prescriberLineYAxis = 386.4f;
                    StampText(data.Scripts[0].Prescriber, 85, prescriberLineYAxis, contentByte, AlternateFontSize);
                    StampText(data.Scripts[0].PrescriberNpi, 256, prescriberLineYAxis, contentByte, AlternateFontSize);
                    const string diagnosis = "V49.9XXA";
                    StampText(diagnosis, 78.7f, 345, contentByte, AlternateFontSize);
                    var index = 0;
                    // Script 1
                    if (data.Scripts.Any() && null != data.Scripts[index])
                    {
                        StampScript(index, data, contentByte);
                    }

                    // Script 2
                    index = 1;
                    var scriptCount = data.Scripts.Count;
                    if (data.Scripts.Any() && index < scriptCount)
                    {
                        StampScript(index, data, contentByte);
                    }

                    // Script 3
                    index = 2;
                    if (data.Scripts.Any() && index < scriptCount)
                    {
                        StampScript(index, data, contentByte);
                    }

                    // Script 4
                    index = 3;
                    if (data.Scripts.Any() && index < scriptCount)
                    {
                        StampScript(index, data, contentByte);
                    }

                    // Script 5
                    index = 4;
                    if (data.Scripts.Any() && index < scriptCount)
                    {
                        StampScript(index, data, contentByte);
                    }

                    // Script 6
                    index = 5;
                    if (data.Scripts.Any() && index < scriptCount)
                    {
                        StampScript(index, data, contentByte);
                    }

                    // Script 7
                    index = 6;
                    if (data.Scripts.Any() && index < scriptCount)
                    {
                        throw new Exception("Error, we have more than 6 prescriptions");
                    }
                    const string federalTaxIdNumber = "81-4105673";
                    const float line25XAxis = 158.5f;
                    StampText(federalTaxIdNumber, 70.8f, line25XAxis, contentByte, AlternateFontSize);
                    StampText(X, 175.6f, line25XAxis, contentByte, 8);
                    // Yes Accept Assignment
                    StampText(X, 290.2f, line25XAxis, contentByte, 8);
                    var totalBilled = data.Scripts.Sum(x => x.BilledAmount);
                    var totalBilledStr = totalBilled.ToString(CultureInfo.InvariantCulture);
                    var totalBilledDollars = totalBilledStr.Left(totalBilledStr.Length - 3);
                    var totalBilledCents = totalBilledStr.Right(2);
                    var totalBilledDollarsLength = totalBilledDollars.ToString().Length;
                    var subtractTotalBilledDollarsXAxis = (totalBilledDollarsLength - 1) * LetterLength;
                    StampText(totalBilledDollars.Length == 0 ? "0" : totalBilledDollars,
                        404.4f - subtractTotalBilledDollarsXAxis, line25XAxis, contentByte,
                        AlternateFontSize);
                    StampText(totalBilledCents, 412.1f, line25XAxis, contentByte,
                        AlternateFontSize);
                    StampText("0", 464.8f, line25XAxis, contentByte, AlternateFontSize);
                    StampText("00", 472.1f, line25XAxis, contentByte, AlternateFontSize);
                    // Box 30
                    StampText(totalBilledDollars, 518.3f - subtractTotalBilledDollarsXAxis, line25XAxis, contentByte,
                        AlternateFontSize);
                    StampText(totalBilledCents.Right(2), 526.5f, line25XAxis, contentByte,
                        AlternateFontSize);
                    const string bridgeportPharmacyServices = "BRIDGEPORT PHARMACY SERVICES";
                    StampText(bridgeportPharmacyServices, leftAxis + 1.5f, 121.5f, contentByte, AlternateFontSize);
                    const float line32XAxis = 199.6f;
                    StampText(data.Scripts[0].PharmacyName, line32XAxis, 142.2f, contentByte, AlternateFontSize);
                    StampText(
                        data.Scripts[0].Address1 + (data.Scripts[0].Address2.IsNotNullOrWhiteSpace()
                            ? $", {data.Scripts[0].Address2}"
                            : string.Empty), line32XAxis, 134.6f, contentByte, AlternateFontSize);
                    StampText(
                        (data.Scripts[0].City ?? string.Empty) +
                        (data.Scripts[0].PharmacyState.IsNotNullOrWhiteSpace() &&
                         !string.Equals(data.Scripts[0].PharmacyState, Na, StringComparison.InvariantCultureIgnoreCase)
                            ? ", " + data.Scripts[0].PharmacyState
                            : string.Empty) + (data.Scripts[0].PostalCode.IsNotNullOrWhiteSpace()
                            ? " " + (data.Scripts[0].PostalCode.Length == 9 && !data.Scripts[0].PostalCode.Contains("-")
                                  ? data.Scripts[0].PostalCode.Left(5)
                                    + "-" + data.Scripts[0].PostalCode.Right(4)
                                  : data.Scripts[0].PostalCode)
                            : string.Empty), line32XAxis, 127.1f, contentByte, AlternateFontSize);
                    StampText(
                        data.Scripts[0].FederalTin.IsNotNullOrWhiteSpace()
                            ? "Tax ID: " + data.Scripts[0].FederalTin
                            : string.Empty, line32XAxis, 119, contentByte, AlternateFontSize);
                    // Line 33
                    const string bridgeportAddress = "PO BOX 249";
                    const string bridgeportCityStateZip = "SANDY, UT 84091-0249";
                    const string bridgeportEmployeeId = "81-4105673";
                    const string bridgeportAreaCode = "844";
                    const string bridgeportPhoneNumber = "480-5630";
                    const float line33XAxis = 365.6f;
                    StampText(bridgeportPharmacyServices, line33XAxis, 139.2f, contentByte, AlternateFontSize);
                    StampText(bridgeportAddress, line33XAxis, 131.6f, contentByte, AlternateFontSize);
                    StampText(bridgeportCityStateZip, line33XAxis, 124.1f, contentByte, AlternateFontSize);
                    StampText(bridgeportEmployeeId, line33XAxis, 116.9f, contentByte, AlternateFontSize);
                    const float billingPhoneNumberYAxis = 147.5f;
                    StampText(bridgeportAreaCode, 461.5f, billingPhoneNumberYAxis, contentByte, AlternateFontSize);
                    StampText(bridgeportPhoneNumber, 482, billingPhoneNumberYAxis, contentByte, AlternateFontSize);
                }
                catch (Exception ex)
                {
                    success = false;
                    Logger.Value.Error(ex);
                }
                finally
                {
                    stamper.Dispose();
                }
            });
            return success;
        }

        private static void StampScript(int scriptIndex, InvoicePdfModel data, PdfContentByte contentByte)
        {
            // Script 1
            const float dateFilledMonthXAxis = 68.7f;
            const float dateFilledDayXAxis = 87;
            const float dateFilledYearXAxis = 104;
            const float secondDateFilledMonthXAxis = 123;
            const float secondDateFilledDayXAxis = 140;
            const float secondDateFilledYearXAxis = 158.5f;
            const float ndcLabelNameXAxis = 213.5f;
            float ndcRxNumberYAxis;
            const float scriptOneNdcRxNumberYAxis = 292.8f;
            const float scriptTwoNdcRxNumberYAxis = 271.7f;
            const float scriptThreeNdcRxNumberYAxis = 251.3f;
            const float scriptFourNdcRxNumberYAxis = 230.1f;
            const float scriptFiveNdcRxNumberYAxis = 209.8f;
            const float scriptSixNdcRxNumberYAxis = 189.0f;
            const float scriptOneYAxis = 283.1f;
            const float scriptTwoYAxis = 261.8f;
            const float scriptThreeYAxis = 241.3f;
            const float scriptFourYAxis = 220;
            const float scriptFiveYAxis = 199.8f;
            const float scriptSixYAxis = 179.0f;
            float yAxis;
            switch (scriptIndex)
            {
                case 0:
                    ndcRxNumberYAxis = scriptOneNdcRxNumberYAxis;
                    yAxis = scriptOneYAxis;
                    break;
                case 1:
                    ndcRxNumberYAxis = scriptTwoNdcRxNumberYAxis;
                    yAxis = scriptTwoYAxis;
                    break;
                case 2:
                    ndcRxNumberYAxis = scriptThreeNdcRxNumberYAxis;
                    yAxis = scriptThreeYAxis;
                    break;
                case 3:
                    ndcRxNumberYAxis = scriptFourNdcRxNumberYAxis;
                    yAxis = scriptFourYAxis;
                    break;
                case 4:
                    ndcRxNumberYAxis = scriptFiveNdcRxNumberYAxis;
                    yAxis = scriptFiveYAxis;
                    break;
                case 5:
                    ndcRxNumberYAxis = scriptSixNdcRxNumberYAxis;
                    yAxis = scriptSixYAxis;
                    break;
                default:
                    throw new Exception($"Error, the {nameof(scriptIndex)} argument must be an integer of zero through 5.");
            }
            StampText(data.Scripts[scriptIndex].Ndc, ndcLabelNameXAxis, ndcRxNumberYAxis, contentByte, AlternateFontSize);
            // Rx Date
            StampText(data.Scripts[scriptIndex].DateFilledMonth, dateFilledMonthXAxis, yAxis, contentByte, AlternateFontSize);
            StampText(data.Scripts[scriptIndex].DateFilledDay, dateFilledDayXAxis, yAxis, contentByte, AlternateFontSize);
            StampText(data.Scripts[scriptIndex].DateFilledYear, dateFilledYearXAxis, yAxis, contentByte, AlternateFontSize);
            // Rx Date again
            StampText(data.Scripts[scriptIndex].DateFilledMonth, secondDateFilledMonthXAxis, yAxis, contentByte, AlternateFontSize);
            StampText(data.Scripts[scriptIndex].DateFilledDay, secondDateFilledDayXAxis, yAxis, contentByte, AlternateFontSize);
            StampText(data.Scripts[scriptIndex].DateFilledYear, secondDateFilledYearXAxis, yAxis, contentByte, AlternateFontSize);
            StampText(data.Scripts[scriptIndex].LabelName, ndcLabelNameXAxis, yAxis, contentByte, AlternateFontSize);
            StampText(ZeroOne, 176.5f, yAxis, contentByte, AlternateFontSize);
            const float rxNumberXAxis = 331.0f;
            StampText(data.Scripts[scriptIndex].RxNumber, rxNumberXAxis, ndcRxNumberYAxis, contentByte, AlternateFontSize);
            const string a = "A";
            StampText(a, rxNumberXAxis + 9, yAxis, contentByte, AlternateFontSize);
            var billedAmountDollars = data.Scripts[scriptIndex].BilledAmountDollars;
            var billedAmountLength = data.Scripts[scriptIndex]?.BilledAmountDollars?.ToString().Length;
            if (null == billedAmountLength)
            {
                throw new Exception("Something went wrong. The billed amount is null.");
            }
            var billedAmountXAxis = 392.0f - (billedAmountLength.Value -1) * LetterLength;
            var billedAmountCents = data.Scripts[scriptIndex].BilledAmountCents;
            if (billedAmountDollars.HasValue)
            {
                var amount = billedAmountDollars.Value.ToString();
                StampText(0 == amount.Length ? "0" : amount, billedAmountXAxis, yAxis, contentByte, AlternateFontSize);
            }
            if (billedAmountCents.IsNotNullOrWhiteSpace())
            {
                var cents = billedAmountCents?.Right(2);
                StampText(cents, BilledAmountCentsXAxis, yAxis, contentByte, AlternateFontSize);
            }
            StampText(data.Scripts[scriptIndex].Quantity.ToString(CultureInfo.InvariantCulture), 421.5f, yAxis, contentByte, AlternateFontSize);
        }

        private static void StampText(string text, float x, float y, PdfContentByte contentByte, int fontSize = DefaultFontSize)
        {
            ColumnText.ShowTextAligned(contentByte, Element.ALIGN_LEFT,
                new Phrase(text, new Font(BaseFont, fontSize, DefaultFontStyle, BaseColor.BLACK)), x, y, 0);
        }

        private static void CopyStream(Stream input, Stream output)
        {
            // Insert null checking here for production
            if (null == input)
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (null == output)
            {
                throw new ArgumentNullException(nameof(output));
            }
            var buffer = new byte[8192];

            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }
    }
}

using NLog;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Common.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BridgeportClaims.Pdf.InvoiceProviders
{
    public class InvoiceProvider : IInvoiceProvider
    {
        private const int DefaultFontSize = 9;
        private const int AlternateFontSize = 7;
        private const int DefaultFontStyle = 0;
        private const float BilledAmountCentsXAxis = 399.0f;
        private const float BilledAmountDollarsXAxis = 372.0f;
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
                        StampText("X", 175.6f, line25XAxis, contentByte, 8);
                        // Yes Accept Assignment
                        StampText("X", 290.2f, line25XAxis, contentByte, 8);
                        var totalBilled = data.Scripts.Sum(x => x.BilledAmount);
                        var totalBilledStr = totalBilled.ToString(CultureInfo.InvariantCulture);
                        var totalBilledDollars = totalBilledStr.Left(totalBilledStr.Length - 5);
                        var totalBilledCents = totalBilledStr.Right(5).Left(3);
                        StampText(totalBilledDollars, BilledAmountDollarsXAxis + 10, line25XAxis, contentByte, AlternateFontSize);
                        StampText(totalBilledCents, BilledAmountDollarsXAxis + 42.1f, line25XAxis, contentByte, AlternateFontSize);
                        StampText("0", BilledAmountDollarsXAxis + 82.1f, line25XAxis, contentByte, AlternateFontSize);
                        StampText("00", BilledAmountDollarsXAxis + 101.1f, line25XAxis, contentByte, AlternateFontSize);
                        StampText(totalBilledDollars, BilledAmountDollarsXAxis + 120, line25XAxis, contentByte, AlternateFontSize);
                        StampText(totalBilledCents.Right(2), BilledAmountDollarsXAxis + 160, line25XAxis, contentByte, AlternateFontSize);
                        const string bridgeportPharmacyServices = "BRIDGEPORT PHARMACY SERVICES";
                        StampText(bridgeportPharmacyServices, leftAxis + 1.5f, 121.5f, contentByte, AlternateFontSize);
                        const float line32XAxis = 199.6f;
                        StampText(data.Scripts[0].PharmacyName, line32XAxis, 142.2f, contentByte, AlternateFontSize);
                        StampText(
                            data.Scripts[0].Address1 + (data.Scripts[0].Address2.IsNotNullOrWhiteSpace()
                                ? $", {data.Scripts[0].Address2}"
                                : string.Empty), line32XAxis, 133.9f, contentByte, AlternateFontSize);
                        StampText(
                            (data.Scripts[0].City ?? string.Empty) +
                            (data.Scripts[0].PharmacyState.IsNotNullOrWhiteSpace()
                                ? "," + data.Scripts[0].PharmacyState
                                : string.Empty) + (data.Scripts[0].PostalCode.IsNotNullOrWhiteSpace()
                                ? " " + data.Scripts[0].PostalCode
                                : string.Empty), line32XAxis, 127.1f, contentByte, AlternateFontSize);
                        StampText(
                            data.Scripts[0].FederalTin.IsNotNullOrWhiteSpace()
                                ? "Tax ID: " + data.Scripts[0].FederalTin
                                : string.Empty, line32XAxis, 120, contentByte, AlternateFontSize);
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
            var ndcRxNumberYAxis = 0.0f;
            const float scriptOneNdcRxNumberYAxis = 292.5f;
            const float scriptTwoNdcRxNumberYAxis = 271.5f;
            const float scriptThreeNdcRxNumberYAxis = 251.1f;
            const float scriptOneYAxis = 283.1f;
            const float scriptTwoYAxis = 261.8f;
            const float scriptThreeYAxis = 241.3f;
            var yAxis = 0.0f;
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
            const float rxNumberXAxis = 331.0f;
            StampText(data.Scripts[scriptIndex].RxNumber, rxNumberXAxis, ndcRxNumberYAxis, contentByte, AlternateFontSize);
            const string a = "A";
            StampText(a, rxNumberXAxis + 9, yAxis, contentByte, AlternateFontSize);
            var billedAmountDollars = data.Scripts[scriptIndex].BilledAmountDollars;
            var billedAmountCents = data.Scripts[scriptIndex].BilledAmountCents;
            if (billedAmountDollars.HasValue && billedAmountDollars.Value > 0)
            {
                StampText(billedAmountDollars.Value.ToString(), BilledAmountDollarsXAxis, yAxis, contentByte, AlternateFontSize);
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
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Enums;
using DataAccess;
using LumenWorks.Framework.IO.Csv;
using DataTable = System.Data.DataTable;
using NLog;

namespace BridgeportClaims.CsvReader.CsvReaders
{
    public class CsvReaderProvider : ICsvReaderProvider
    {
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private readonly ICsvToolsProvider _csvToolsProvider;

        public CsvReaderProvider(ICsvToolsProvider csvToolsProvider)
        {
            _csvToolsProvider = csvToolsProvider;
        }

        public DataTable ReadLakerCsvFile(string fullFilePath, bool useCsvTools, FileSource fileSource)
        {
            try
            {
                if (useCsvTools)
                {
                    var dt = _csvToolsProvider.ReadCsvFile(fullFilePath);
                    var retVal = fileSource == FileSource.Laker
                        ? ConvertToLakerDataTable(dt.Rows)
                        : ConvertToEnvisionDataTable(dt.Rows);
                    return retVal;
                }
                return DisposableService.Using(
                    () => new CachedCsvReader(new StreamReader(fullFilePath), fileSource == FileSource.Envision,
                        fileSource == FileSource.Laker ? '\t' : ','), csv =>
                    {
                        var table = new DataTable();
                        table.Load(csv);
                        return table;
                    });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }

        private static DataTable ConvertToEnvisionDataTable(IEnumerable<Row> rows)
        {
            var dt = new DataTable();
            dt.Columns.Add("CarrierID", typeof(string));
            dt.Columns.Add("GroupID", typeof(string));
            dt.Columns.Add("LocationCode", typeof(string));
            dt.Columns.Add("MemberID", typeof(string));
            dt.Columns.Add("PersonCode", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("RelCode", typeof(string));
            dt.Columns.Add("DOB", typeof(string));
            dt.Columns.Add("Gender", typeof(string));
            dt.Columns.Add("SSN", typeof(string));
            dt.Columns.Add("HICN", typeof(string));
            dt.Columns.Add("Subgroup", typeof(string));
            dt.Columns.Add("FillDate", typeof(string));
            dt.Columns.Add("WrittenDate", typeof(string));
            dt.Columns.Add("ProcessDate", typeof(string));
            dt.Columns.Add("ProcessTime", typeof(string));
            dt.Columns.Add("PharmacyNPI", typeof(string));
            dt.Columns.Add("PharmacyName", typeof(string));
            dt.Columns.Add("SubmittedPrescriberID", typeof(string));
            dt.Columns.Add("AltPrescriberID", typeof(string));
            dt.Columns.Add("RxNumber", typeof(string));
            dt.Columns.Add("QTY", typeof(string));
            dt.Columns.Add("DS", typeof(string));
            dt.Columns.Add("FillNumber", typeof(string));
            dt.Columns.Add("TranType", typeof(string));
            dt.Columns.Add("DAW", typeof(string));
            dt.Columns.Add("Compound", typeof(string));
            dt.Columns.Add("OtherCoverageCode", typeof(string));
            dt.Columns.Add("RxOrigin", typeof(string));
            dt.Columns.Add("MPANumber", typeof(string));
            dt.Columns.Add("MarkScriptAs", typeof(string));
            dt.Columns.Add("AuthNumber", typeof(string));
            dt.Columns.Add("ReversedAuth", typeof(string));
            dt.Columns.Add("NDC", typeof(string));
            dt.Columns.Add("GPI", typeof(string));
            dt.Columns.Add("DrugName", typeof(string));
            dt.Columns.Add("MONY", typeof(string));
            dt.Columns.Add("DEAClass", typeof(string));
            dt.Columns.Add("Tier", typeof(string));
            dt.Columns.Add("IngredCost", typeof(string));
            dt.Columns.Add("DispFee", typeof(string));
            dt.Columns.Add("SalesTax", typeof(string));
            dt.Columns.Add("VaccineAdminFee", typeof(string));
            dt.Columns.Add("MemberCostShareCopay", typeof(string));
            dt.Columns.Add("GroupBillAmount", typeof(string));
            dt.Columns.Add("DeductibleAmount", typeof(string));
            dt.Columns.Add("MemberOOP", typeof(string));
            dt.Columns.Add("BenefitAmount", typeof(string));
            dt.Columns.Add("SDCIND1", typeof(string));
            dt.Columns.Add("SDCVALUE1", typeof(string));
            dt.Columns.Add("SDCIND2", typeof(string));
            dt.Columns.Add("SDCVALUE2", typeof(string));
            dt.Columns.Add("SDCIND3", typeof(string));
            dt.Columns.Add("SDCVALUE3", typeof(string));
            dt.Columns.Add("SDCIND4", typeof(string));
            dt.Columns.Add("SDCVALUE4", typeof(string));
            dt.Columns.Add("SDCIND5", typeof(string));
            dt.Columns.Add("SDCVALUE5", typeof(string));
            foreach (var item in rows)
            {
                var row = dt.NewRow();
                var count = item.Values.Count;
                for (var i = 1; i <= count; i++)
                {
                    row[EnvisionColumns[i]] = item.Values[i - 1];
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private static DataTable ConvertToLakerDataTable(IEnumerable<Row> rows)
        {
            var dt = new DataTable();
            dt.Columns.Add("RowID", typeof(string));
            dt.Columns.Add("2", typeof(string));
            dt.Columns.Add("3", typeof(string));
            dt.Columns.Add("4", typeof(string));
            dt.Columns.Add("5", typeof(string));
            dt.Columns.Add("6", typeof(string));
            dt.Columns.Add("7", typeof(string));
            dt.Columns.Add("8", typeof(string));
            dt.Columns.Add("9", typeof(string));
            dt.Columns.Add("10", typeof(string));
            dt.Columns.Add("11", typeof(string));
            dt.Columns.Add("12", typeof(string));
            dt.Columns.Add("13", typeof(string));
            dt.Columns.Add("14", typeof(string));
            dt.Columns.Add("15", typeof(string));
            dt.Columns.Add("16", typeof(string));
            dt.Columns.Add("17", typeof(string));
            dt.Columns.Add("18", typeof(string));
            dt.Columns.Add("19", typeof(string));
            dt.Columns.Add("20", typeof(string));
            dt.Columns.Add("21", typeof(string));
            dt.Columns.Add("22", typeof(string));
            dt.Columns.Add("23", typeof(string));
            dt.Columns.Add("24", typeof(string));
            dt.Columns.Add("25", typeof(string));
            dt.Columns.Add("26", typeof(string));
            dt.Columns.Add("27", typeof(string));
            dt.Columns.Add("28", typeof(string));
            dt.Columns.Add("29", typeof(string));
            dt.Columns.Add("30", typeof(string));
            dt.Columns.Add("31", typeof(string));
            dt.Columns.Add("32", typeof(string));
            dt.Columns.Add("33", typeof(string));
            dt.Columns.Add("34", typeof(string));
            dt.Columns.Add("35", typeof(string));
            dt.Columns.Add("36", typeof(string));
            dt.Columns.Add("37", typeof(string));
            dt.Columns.Add("38", typeof(string));
            dt.Columns.Add("39", typeof(string));
            dt.Columns.Add("40", typeof(string));
            dt.Columns.Add("41", typeof(string));
            dt.Columns.Add("42", typeof(string));
            dt.Columns.Add("43", typeof(string));
            dt.Columns.Add("44", typeof(string));
            dt.Columns.Add("45", typeof(string));
            dt.Columns.Add("46", typeof(string));
            dt.Columns.Add("47", typeof(string));
            dt.Columns.Add("48", typeof(string));
            dt.Columns.Add("49", typeof(string));
            dt.Columns.Add("50", typeof(string));
            dt.Columns.Add("51", typeof(string));
            dt.Columns.Add("52", typeof(string));
            dt.Columns.Add("53", typeof(string));
            dt.Columns.Add("54", typeof(string));
            dt.Columns.Add("55", typeof(string));
            dt.Columns.Add("56", typeof(string));
            dt.Columns.Add("57", typeof(string));
            dt.Columns.Add("58", typeof(string));
            dt.Columns.Add("59", typeof(string));
            dt.Columns.Add("60", typeof(string));
            dt.Columns.Add("61", typeof(string));
            dt.Columns.Add("62", typeof(string));
            dt.Columns.Add("63", typeof(string));
            dt.Columns.Add("64", typeof(string));
            dt.Columns.Add("65", typeof(string));
            dt.Columns.Add("66", typeof(string));
            dt.Columns.Add("67", typeof(string));
            dt.Columns.Add("68", typeof(string));
            dt.Columns.Add("69", typeof(string));
            dt.Columns.Add("70", typeof(string));
            dt.Columns.Add("71", typeof(string));
            dt.Columns.Add("72", typeof(string));
            dt.Columns.Add("73", typeof(string));
            dt.Columns.Add("74", typeof(string));
            dt.Columns.Add("75", typeof(string));
            dt.Columns.Add("76", typeof(string));
            dt.Columns.Add("77", typeof(string));
            dt.Columns.Add("78", typeof(string));
            dt.Columns.Add("79", typeof(string));
            dt.Columns.Add("80", typeof(string));
            dt.Columns.Add("81", typeof(string));
            dt.Columns.Add("82", typeof(string));
            dt.Columns.Add("83", typeof(string));
            dt.Columns.Add("84", typeof(string));
            dt.Columns.Add("85", typeof(string));
            dt.Columns.Add("86", typeof(string));
            dt.Columns.Add("87", typeof(string));
            dt.Columns.Add("88", typeof(string));
            dt.Columns.Add("89", typeof(string));
            dt.Columns.Add("90", typeof(string));
            dt.Columns.Add("91", typeof(string));
            dt.Columns.Add("92", typeof(string));
            dt.Columns.Add("93", typeof(string));
            dt.Columns.Add("94", typeof(string));
            dt.Columns.Add("95", typeof(string));
            dt.Columns.Add("96", typeof(string));
            dt.Columns.Add("97", typeof(string));
            dt.Columns.Add("98", typeof(string));
            dt.Columns.Add("99", typeof(string));
            dt.Columns.Add("100", typeof(string));
            dt.Columns.Add("101", typeof(string));
            dt.Columns.Add("102", typeof(string));
            dt.Columns.Add("103", typeof(string));
            dt.Columns.Add("104", typeof(string));
            dt.Columns.Add("105", typeof(string));
            dt.Columns.Add("106", typeof(string));
            dt.Columns.Add("107", typeof(string));
            dt.Columns.Add("108", typeof(string));
            dt.Columns.Add("109", typeof(string));
            dt.Columns.Add("110", typeof(string));
            dt.Columns.Add("111", typeof(string));
            dt.Columns.Add("112", typeof(string));
            dt.Columns.Add("113", typeof(string));
            dt.Columns.Add("114", typeof(string));
            dt.Columns.Add("115", typeof(string));
            dt.Columns.Add("116", typeof(string));
            dt.Columns.Add("117", typeof(string));
            dt.Columns.Add("118", typeof(string));
            dt.Columns.Add("119", typeof(string));
            dt.Columns.Add("120", typeof(string));
            dt.Columns.Add("121", typeof(string));
            dt.Columns.Add("122", typeof(string));
            dt.Columns.Add("123", typeof(string));
            dt.Columns.Add("124", typeof(string));
            dt.Columns.Add("125", typeof(string));
            dt.Columns.Add("126", typeof(string));
            dt.Columns.Add("127", typeof(string));
            dt.Columns.Add("128", typeof(string));
            dt.Columns.Add("129", typeof(string));
            dt.Columns.Add("130", typeof(string));
            dt.Columns.Add("131", typeof(string));
            dt.Columns.Add("132", typeof(string));
            dt.Columns.Add("133", typeof(string));
            dt.Columns.Add("134", typeof(string));
            dt.Columns.Add("135", typeof(string));
            dt.Columns.Add("136", typeof(string));
            dt.Columns.Add("137", typeof(string));
            dt.Columns.Add("138", typeof(string));
            dt.Columns.Add("139", typeof(string));
            dt.Columns.Add("140", typeof(string));
            dt.Columns.Add("141", typeof(string));
            dt.Columns.Add("142", typeof(string));
            dt.Columns.Add("143", typeof(string));
            dt.Columns.Add("144", typeof(string));
            dt.Columns.Add("145", typeof(string));
            dt.Columns.Add("146", typeof(string));
            dt.Columns.Add("147", typeof(string));
            dt.Columns.Add("148", typeof(string));
            dt.Columns.Add("149", typeof(string));
            dt.Columns.Add("150", typeof(string));
            dt.Columns.Add("151", typeof(string));
            dt.Columns.Add("152", typeof(string));
            dt.Columns.Add("153", typeof(string));
            dt.Columns.Add("154", typeof(string));
            // Populate data table.
            foreach (var item in rows)
            {
                var row = dt.NewRow();
                var count = item.Values.Count;
                for (var i = 1; i <= count; i++)
                {
                    row[i == 1 ? "RowID" : i.ToString()] = item.Values[i - 1];
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private static Dictionary<int, string> EnvisionColumns
        {
            get
            {
                var dictionary = new Dictionary<int, string>
                {
                    {1, "CarrierID"},
                    {2, "GroupID"},
                    {3, "LocationCode"},
                    {4, "MemberID"},
                    {5, "PersonCode"},
                    {6, "LastName"},
                    {7, "FirstName"},
                    {8, "RelCode"},
                    {9, "DOB"},
                    {10, "Gender"},
                    {11, "SSN"},
                    {12, "HICN"},
                    {13, "Subgroup"},
                    {14, "FillDate"},
                    {15, "WrittenDate"},
                    {16, "ProcessDate"},
                    {17, "ProcessTime"},
                    {18, "PharmacyNPI"},
                    {19, "PharmacyName"},
                    {20, "SubmittedPrescriberID"},
                    {21, "AltPrescriberID"},
                    {22, "RxNumber"},
                    {23, "QTY"},
                    {24, "DS"},
                    {25, "FillNumber"},
                    {26, "TranType"},
                    {27, "DAW"},
                    {28, "Compound"},
                    {29, "OtherCoverageCode"},
                    {30, "RxOrigin"},
                    {31, "MPANumber"},
                    {32, "MarkScriptAs"},
                    {33, "AuthNumber"},
                    {34, "ReversedAuth"},
                    {35, "NDC"},
                    {36, "GPI"},
                    {37, "DrugName"},
                    {38, "MONY"},
                    {39, "DEAClass"},
                    {40, "Tier"},
                    {41, "IngredCost"},
                    {42, "DispFee"},
                    {43, "SalesTax"},
                    {44, "VaccineAdminFee"},
                    {45, "MemberCostShareCopay"},
                    {46, "GroupBillAmount"},
                    {47, "DeductibleAmount"},
                    {48, "MemberOOP"},
                    {49, "BenefitAmount"},
                    {50, "SDCIND1"},
                    {51, "SDCVALUE1"},
                    {52, "SDCIND2"},
                    {53, "SDCVALUE2"},
                    {54, "SDCIND3"},
                    {55, "SDCVALUE3"},
                    {56, "SDCIND4"},
                    {57, "SDCVALUE4"},
                    {58, "SDCIND5"},
                    {59, "SDCVALUE5"}
                };
                return dictionary;
            }
        }
    }
}
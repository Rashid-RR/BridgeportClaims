using System.IO;
using BridgeportClaims.ExcelDataReader.Exceptions;
using BridgeportClaims.ExcelDataReader.Exceptions.Core.BinaryFormat;
using BridgeportClaims.ExcelDataReader.Exceptions.Core.CompoundFormat;

namespace BridgeportClaims.ExcelDataReader
{
    /// <summary>
    /// The ExcelReader Factory
    /// </summary>
    public static class ExcelReaderFactory
    {
        private const string DirectoryEntryWorkbook = "Workbook";
        private const string DirectoryEntryBook = "Book";
        private const string DirectoryEntryEncryptedPackage = "EncryptedPackage";
        private const string DirectoryEntryEncryptionInfo = "EncryptionInfo";

        /// <summary>
        /// Creates an instance of <see cref="ExcelBinaryReader"/> or <see>
        ///         <cref>ExcelOpenXmlReader</cref>
        ///     </see>
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="configuration">The configuration object.</param>
        /// <returns>The excel data reader.</returns>
        public static IExcelDataReader CreateReader(Stream fileStream, ExcelReaderConfiguration configuration = null)
        { 
            var probe = new byte[8];
            fileStream.Read(probe, 0, probe.Length);
            fileStream.Seek(0, SeekOrigin.Begin);

            if (CompoundDocument.IsCompoundDocument(probe))
            {
                // Can be BIFF5-8 or password protected OpenXml
                var document = new CompoundDocument(fileStream);
                if (TryGetWorkbook(fileStream, document, out var stream))
                {
                    return new ExcelBinaryReader(stream, configuration);
                }
                throw new ExcelReaderException(Errors.ErrorStreamWorkbookNotFound);
            }
            if (XlsWorkbook.IsRawBiffStream(probe))
            {
                return new ExcelBinaryReader(fileStream, configuration);
            }
            throw new HeaderException(Errors.ErrorHeaderSignature);
        }

        /// <summary>
        /// Creates an instance of <see cref="ExcelBinaryReader"/>
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="configuration">The configuration object.</param>
        /// <returns>The excel data reader.</returns>
        public static IExcelDataReader CreateBinaryReader(Stream fileStream, ExcelReaderConfiguration configuration = null)
        {
            var probe = new byte[8];
            fileStream.Read(probe, 0, probe.Length);
            fileStream.Seek(0, SeekOrigin.Begin);

            if (CompoundDocument.IsCompoundDocument(probe))
            {
                var document = new CompoundDocument(fileStream);
                if (TryGetWorkbook(fileStream, document, out var stream))
                {
                    return new ExcelBinaryReader(stream, configuration);
                }
                else
                {
                    throw new ExcelReaderException(Errors.ErrorStreamWorkbookNotFound);
                }
            }
            else if (XlsWorkbook.IsRawBiffStream(probe))
            {
                return new ExcelBinaryReader(fileStream, configuration);
            }
            else
            {
                throw new HeaderException(Errors.ErrorHeaderSignature);
            }
        }

        /// <summary>
        /// Creates an instance of <see>
        ///         <cref>ExcelOpenXmlReader</cref>
        ///     </see>
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="configuration">The reader configuration -or- <see langword="null"/> to use the default configuration.</param>
        /// <returns>The excel data reader.</returns>
        /*public static IExcelDataReader CreateOpenXmlReader(Stream fileStream, ExcelReaderConfiguration configuration = null)
        {
            var probe = new byte[8];
            fileStream.Read(probe, 0, probe.Length);
            fileStream.Seek(0, SeekOrigin.Begin);

            // Probe for password protected compound document or zip file
            if (CompoundDocument.IsCompoundDocument(probe))
            {
                var document = new CompoundDocument(fileStream);
                if (TryGetEncryptedPackage(fileStream, document, "password", out var stream))
                {
                    return new ExcelOpenXmlReader(stream, configuration);
                }
                else
                {
                    throw new ExcelReaderException(Errors.ErrorCompoundNoOpenXml);
                }
            }
            else if (probe[0] == 0x50 && probe[1] == 0x4B)
            {
                // Zip files start with 'PK'
                return new ExcelOpenXmlReader(fileStream, configuration);
            }
            else
            {
                throw new HeaderException(Errors.ErrorHeaderSignature);
            }
        }*/

        private static bool TryGetWorkbook(Stream fileStream, CompoundDocument document, out Stream stream)
        {
            var workbookEntry = document.FindEntry(DirectoryEntryWorkbook) ?? document.FindEntry(DirectoryEntryBook);
            if (workbookEntry != null)
            {
                if (workbookEntry.EntryType != Stgty.StgtyStream)
                {
                    throw new ExcelReaderException(Errors.ErrorWorkbookIsNotStream);
                }

                stream = document.CreateStream(fileStream, workbookEntry.StreamFirstSector, (int)workbookEntry.StreamSize, workbookEntry.IsEntryMiniStream);
                return true;
            }

            stream = null;
            return false;
        }

        private static bool TryGetEncryptedPackage(Stream fileStream, CompoundDocument document, string password, out Stream stream)
        {
            var encryptedPackage = document.FindEntry(DirectoryEntryEncryptedPackage);
            var encryptionInfo = document.FindEntry(DirectoryEntryEncryptionInfo);

            if (encryptedPackage == null || encryptionInfo == null)
            {
                stream = null;
                return false;
            }

            /*
            var packageBytes = document.ReadStream(fileStream, encryptedPackage.StreamFirstSector, (int)encryptedPackage.StreamSize, encryptedPackage.IsEntryMiniStream);
            var infoBytes = document.ReadStream(fileStream, encryptionInfo.StreamFirstSector, (int)encryptionInfo.StreamSize, encryptionInfo.IsEntryMiniStream);

            TODO ...

            */
            stream = null;
            return false;
        }
    }
}

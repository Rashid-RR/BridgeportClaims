namespace BridgeportClaims.ExcelDataReader.Exceptions.Core.CompoundFormat
{
    internal enum Stgty : byte
    {
        StgtyInvalid = 0,
        StgtyStorage = 1,
        StgtyStream = 2,
        StgtyLockbytes = 3,
        StgtyProperty = 4,
        StgtyRoot = 5
    }

    internal enum Decolor : byte
    {
        DeRed = 0,
        DeBlack = 1
    }

    internal enum Fatmarkers : uint
    {
        FatEndOfChain = 0xFFFFFFFE,
        FatFreeSpace = 0xFFFFFFFF,
        FatFatSector = 0xFFFFFFFD,
        FatDifSector = 0xFFFFFFFC
    }
}

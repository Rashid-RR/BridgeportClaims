using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.Type;

namespace BridgeportClaims.Data.NHibernate
{
    public sealed class TypeMapper
    {
        public static IType GetTypeFromDbType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Int16:
                    return new Int16Type();
                case DbType.Int32:
                    return new Int32Type();
                case DbType.Binary:
                    return new BinaryBlobType();
                case DbType.Byte:
                    return new ByteType();
                case DbType.Boolean:
                    return new BooleanType();
                case DbType.Date:
                    return new DateType();
                case DbType.DateTime:
                    return new DateTimeType();
                case DbType.Decimal:
                    return new DecimalType();
                case DbType.Double:
                    return new DoubleType();
                case DbType.Guid:
                    return new GuidType();
                case DbType.Int64:
                    return new Int64Type();
                case DbType.SByte:
                    return new SByteType();
                case DbType.Single:
                    return new SingleType();
                case DbType.String:
                    return NHibernateUtil.GuessType(typeof(string));
                case DbType.Time:
                    return new TimeType();
                case DbType.UInt16:
                    return new UInt16Type();
                case DbType.UInt32:
                    return new UInt32Type();
                case DbType.UInt64:
                    return new UInt64Type();
                case DbType.AnsiStringFixedLength:
                    return NHibernateUtil.GuessType(typeof(AnsiStringFixedLengthSqlType));
                case DbType.StringFixedLength:
                    return NHibernateUtil.GuessType(typeof(StringFixedLengthSqlType));
                case DbType.Xml:
                    return new XmlDocType();
                case DbType.DateTime2:
                    return new DateTimeType();
                case DbType.DateTimeOffset:
                    return new DateTimeOffsetType();
                case DbType.AnsiString:
                    return NHibernateUtil.GuessType(typeof(AnsiStringType));
                case DbType.Currency:
                    return NHibernateUtil.GuessType(typeof(CurrencyType));
                case DbType.Object:
                    return NHibernateUtil.GuessType(typeof(object));
                case DbType.VarNumeric:
                    return NHibernateUtil.GuessType(typeof(float));
                default:
                    throw new ArgumentOutOfRangeException(nameof(dbType), dbType, null);
            }
        }
    }
}
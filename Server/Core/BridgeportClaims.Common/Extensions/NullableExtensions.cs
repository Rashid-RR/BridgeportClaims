using System;

namespace BridgeportClaims.Common.Extensions
{
    public static class NullableExtensions
    {
        public static Type Nullify(this Type type)
        {
            if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
                return typeof(Nullable<>).MakeGenericType(type);
            return type;
        }

        public static Type UnNullify(this Type type) => Nullable.GetUnderlyingType(type) ?? type;
    }
}
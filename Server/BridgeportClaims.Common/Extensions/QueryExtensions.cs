using System;
using NHibernate;

namespace BridgeportClaims.Common.Extensions
{
    public static class QueryExtensions
    {
        public static IQuery SetInt32(this IQuery query, int position, int? val)
        {
            if (val.HasValue)
            {
                query.SetInt32(position, val.Value);
            }
            else
            {
                query.SetParameter(position, null, NHibernateUtil.Int32);
            }
            return query;
        }

        public static IQuery SetInt32(this IQuery query, string name, int? val)
        {
            if (val.HasValue)
            {
                query.SetInt32(name, val.Value);
            }
            else
            {
                query.SetParameter(name, null, NHibernateUtil.Int32);
            }
            return query;
        }

        public static IQuery SetDateTime(this IQuery query, int position, DateTime? val)
        {
            if (val.HasValue)
            {
                query.SetDateTime(position, val.Value);
            }
            else
            {
                query.SetParameter(position, null, NHibernateUtil.DateTime);
            }
            return query;
        }

        public static IQuery SetDateTime2(this IQuery query, string name, DateTime? val)
        {
            if (val.HasValue)
            {
                query.SetDateTime2(name, val.Value);
            }
            else
            {
                query.SetParameter(name, null, NHibernateUtil.DateTime2);
            }
            return query;
        }

    }
}
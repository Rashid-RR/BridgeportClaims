using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NHibernate;
using NHibernate.Transform;
using BridgeportClaims.Data.NHibernate;

namespace BridgeportClaims.Data.StoredProcedureExecutors
{
    public class StoredProcedureExecutor : IStoredProcedureExecutor
    {
        private readonly ISession _session;

        public StoredProcedureExecutor(ISession session)
        {
            _session = session;
        }

        public IEnumerable<T> ExecuteMultiResultStoredProcedure<T>(string procedureNameExecStatement, IList<SqlParameter> parameters)
        {
            using (var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    var query = _session.CreateSQLQuery(procedureNameExecStatement);
                    AddStoredProcedureParameters(query, parameters);
                    var result = query.SetResultTransformer(Transformers.AliasToBean(typeof(T))).List().Cast<T>();
                    if (transaction.IsActive)
                        transaction.Commit();
                    return result;
                }
                catch
                {
                    if (transaction.IsActive)
                        transaction.Rollback();
                    throw;
                }
            }
        }

        public T ExecuteSingleResultStoredProcedure<T>(string procedureNameExecStatement,
            IList<SqlParameter> parameters)
        {
            var query = _session.CreateSQLQuery(procedureNameExecStatement);
            AddStoredProcedureParameters(query, parameters);
            var result = query.SetResultTransformer(Transformers.AliasToBean(typeof(T))).UniqueResult<T>();
            return result;
        }

        public T ExecuteScalarStoredProcedure<T>(string procedureName, IList<SqlParameter> parameters)
        {
            var query = _session.GetNamedQuery(procedureName);
            AddStoredProcedureParameters(query, parameters);
            var result = query.SetResultTransformer(Transformers.AliasToBean(typeof(T))).UniqueResult<T>();

            return result;
        }

        public static IQuery AddStoredProcedureParameters(IQuery query, IEnumerable<SqlParameter> parameters)
        {
            foreach (var parameter in parameters)
                query.SetParameter(parameter.ParameterName, parameter.Value,
                    TypeMapper.GetTypeFromDbType(parameter.DbType));
            return query;
        }
    }
}

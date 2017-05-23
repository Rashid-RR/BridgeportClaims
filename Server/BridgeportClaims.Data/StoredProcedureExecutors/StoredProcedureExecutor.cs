using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NHibernate;
using NHibernate.Transform;

namespace BridgeportClaims.Data.StoredProcedureExecutors
{
    public class StoredProcedureExecutor : IStoredProcedureExecutor
    {
        private readonly ISessionFactory _sessionFactory;

        public StoredProcedureExecutor(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public IEnumerable<T> ExecuteMultiResultStoredProcedure<T>(string procedureNameExecStatement, IList<SqlParameter> parameters)
        {
            IEnumerable<T> result;
            using (var session = _sessionFactory.OpenSession())
            {
                var query = session.CreateSQLQuery(procedureNameExecStatement);
                AddStoredProcedureParameters(query, parameters);
                result = query.SetResultTransformer(Transformers.AliasToBean(typeof(T))).List().Cast<T>();
            }
            return result;
        }

        public T ExecuteSingleResultStoredProcedure<T>(string procedureNameExecStatement, IList<SqlParameter> parameters)
        {
            T result;
            using (var session = _sessionFactory.OpenSession())
            {
                var query = session.CreateSQLQuery(procedureNameExecStatement);
                AddStoredProcedureParameters(query, parameters);
                result = query.SetResultTransformer(Transformers.AliasToBean(typeof(T))).UniqueResult<T>();
            }
            return result;
        }

        public T ExecuteScalarStoredProcedure<T>(string procedureName, IList<SqlParameter> parameters)
        {
            T result;
            using (var session = _sessionFactory.OpenSession())
            {
                var query = session.GetNamedQuery(procedureName);
                AddStoredProcedureParameters(query, parameters);
                result = query.SetResultTransformer(Transformers.AliasToBean(typeof(T))).UniqueResult<T>();
            }
            return result;
        }

        public static IQuery AddStoredProcedureParameters(IQuery query, IEnumerable<SqlParameter> parameters)
        {
            foreach (var parameter in parameters)
                query.SetParameter(parameter.ParameterName, parameter.Value);
            return query;
        }
    }
}

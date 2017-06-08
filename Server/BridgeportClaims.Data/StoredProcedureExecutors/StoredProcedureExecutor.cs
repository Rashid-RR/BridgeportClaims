using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.ExpressionManagers;
using BridgeportClaims.Data.RepositoryUnitOfWork;
using NHibernate;
using NHibernate.Transform;

namespace BridgeportClaims.Data.StoredProcedureExecutors
{
    public class StoredProcedureExecutor : IStoredProcedureExecutor
    {
        private readonly UnitOfWork _unitOfWork;

        public StoredProcedureExecutor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
        }

        public IEnumerable<T> ExecuteMultiResultStoredProcedure<T>(string procedureNameExecStatement, IList<SqlParameter> parameters)
        {
            IEnumerable<T> result;
            using (var uow = _unitOfWork.Session)
            {
                var query = uow.CreateSQLQuery(procedureNameExecStatement);
                AddStoredProcedureParameters(query, parameters);
                result = query.SetResultTransformer(Transformers.AliasToBean(typeof(T))).List().Cast<T>();
            }
            return result;
        }

        public T ExecuteSingleResultStoredProcedure<T>(string procedureNameExecStatement,
            IList<SqlParameter> parameters)
        {
            return
                DisposableManager.Using(() => _unitOfWork.Session,
                    transaction =>
                    {
                        _unitOfWork.BeginTransaction();
                        var query = transaction.CreateSQLQuery(procedureNameExecStatement);
                        AddStoredProcedureParameters(query, parameters);
                        var result = query.SetResultTransformer(Transformers.AliasToBean(typeof(T))).UniqueResult<T>();
                        _unitOfWork.Commit();
                        return result;
                    });
        }

        public T ExecuteScalarStoredProcedure<T>(string procedureName, IList<SqlParameter> parameters)
        {
            T result;
            using (var uow = _unitOfWork.Session)
            {
                var query = uow.GetNamedQuery(procedureName);
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

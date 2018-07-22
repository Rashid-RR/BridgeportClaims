using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.NHibernate;
using NHibernate;
using NHibernate.Transform;
using NLog;

namespace BridgeportClaims.Data.SessionFactory.StoredProcedureExecutors
{
    public class StoredProcedureExecutor : IStoredProcedureExecutor
    {
        private readonly Lazy<ISession> _session;
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger); 

        public StoredProcedureExecutor(Lazy<ISession> session)
        {
            _session = session;
        }

        public IEnumerable<T> ExecuteMultiResultStoredProcedure<T>(string procedureNameExecStatement, 
            IList<SqlParameter> parameters) => DisposableService.Using(() 
                => _session.Value.BeginTransaction(IsolationLevel.ReadCommitted),
            transaction =>
            {
                try
                {
                    IQuery query = _session.Value.CreateSQLQuery(procedureNameExecStatement);
                    AddStoredProcedureParameters(query, parameters);
                    var result = query.SetResultTransformer(Transformers.AliasToBean(typeof(T))).List().Cast<T>();
                    if (transaction.IsActive)
                        transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    Logger.Value.Error(ex);
                    if (transaction.IsActive)
                        transaction.Rollback();
                    throw;
                }
            });

        public static IQuery AddStoredProcedureParameters(IQuery query, IEnumerable<SqlParameter> parameters)
        {
            try
            {
                foreach (var parameter in parameters)
                    query.SetParameter(parameter.ParameterName, parameter.Value,
                        TypeMapper.GetTypeFromDbType(parameter.DbType));
                return query;
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }
    }
}

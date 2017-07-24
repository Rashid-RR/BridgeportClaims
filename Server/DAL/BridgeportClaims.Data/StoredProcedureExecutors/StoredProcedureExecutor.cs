using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using NHibernate;
using NHibernate.Transform;
using BridgeportClaims.Data.NHibernate;
using NLog;

namespace BridgeportClaims.Data.StoredProcedureExecutors
{
    public class StoredProcedureExecutor : IStoredProcedureExecutor
    {
        private readonly ISession _session;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); 

        public StoredProcedureExecutor(ISession session)
        {
            _session = session;
        }

        public void ExecuteNoResultStoredProcedure(string procedureNameExecStatement,
            IList<SqlParameter> parameters)
            => DisposableService.Using(() => _session.BeginTransaction(IsolationLevel.ReadCommitted),
                    transaction =>
                    {
                        try
                        {
                            IQuery query = _session.CreateSQLQuery(procedureNameExecStatement);
                            AddStoredProcedureParameters(query, parameters);
                            query.UniqueResult();
                            if (transaction.IsActive)
                                transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            if (transaction.IsActive)
                                transaction.Rollback();
                            Logger.Error(ex);
                            throw;
                        }
                    });
        

        public IEnumerable<T> ExecuteMultiResultStoredProcedure<T>(string procedureNameExecStatement, 
            IList<SqlParameter> parameters) => DisposableService.Using(() 
                => _session.BeginTransaction(IsolationLevel.ReadCommitted),
            transaction =>
            {
                try
                {
                    IQuery query = _session.CreateSQLQuery(procedureNameExecStatement);
                    AddStoredProcedureParameters(query, parameters);
                    var result = query.SetResultTransformer(Transformers.AliasToBean(typeof(T))).List().Cast<T>();
                    if (transaction.IsActive)
                        transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    if (transaction.IsActive)
                        transaction.Rollback();
                    throw;
                }
            });

        public T ExecuteSingleResultStoredProcedure<T>(string procedureNameExecStatement,
            IList<SqlParameter> parameters) => DisposableService.Using(() 
                => _session.BeginTransaction(IsolationLevel.ReadCommitted),
            transaction =>
            {
                try
                {
                    IQuery query = _session.CreateSQLQuery(procedureNameExecStatement);
                    AddStoredProcedureParameters(query, parameters);
                    var result = query.SetResultTransformer(Transformers.AliasToBean(typeof(T))).UniqueResult<T>();
                    if (transaction.IsActive)
                        transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    if (transaction.IsActive)
                        transaction.Rollback();
                    throw;
                }
            });

        public T ExecuteScalarStoredProcedure<T>(string procedureName, IList<SqlParameter> parameters)
            => DisposableService.Using(() => _session.BeginTransaction(IsolationLevel.ReadCommitted),
                transaction =>
                {
                    try
                    {
                        var query = _session.GetNamedQuery(procedureName);
                        AddStoredProcedureParameters(query, parameters);
                        var result = query.SetResultTransformer(Transformers.AliasToBean(typeof(T))).UniqueResult<T>();
                        if (transaction.IsActive)
                            transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
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
                Logger.Error(ex);
                throw;
            }
        }
    }
}

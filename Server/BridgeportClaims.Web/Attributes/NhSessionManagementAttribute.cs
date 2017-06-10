using System;
using System.Data;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BridgeportClaims.Data.SessionFactory;
using NHibernate;
using NHibernate.Context;

namespace BridgeportClaims.Web.Attributes
{
    public class NhSessionManagementAttribute : ActionFilterAttribute
    {
        public NhSessionManagementAttribute()
        {
            SessionFactory = SessionFactoryBuilder.CreateSessionFactory();
        }

        private ISessionFactory SessionFactory { get; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (null == SessionFactory)
                throw new Exception("Somehow the SessionFactory object was not initialized during construction");
            var factory = SessionFactory;
            var session = factory.OpenSession();
            if (!CurrentSessionContext.HasBind(factory))
                CurrentSessionContext.Bind(session);
            session.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var sess = CurrentSessionContext.Unbind(SessionFactory);
            if (sess == null) return;
            var tran = sess.Transaction;
            try
            {
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                sess.Close();
                sess.Dispose();
            }
        }
    }
}
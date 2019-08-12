using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using PdfGeneratorApi.Data.DataProviders.InvoicePdfDocuments;
using PdfGeneratorApi.Pdf.InvoiceProviders;

namespace PdfGeneratorApi
{
    public class AutofacWebApiConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //Register your Web API controllers.  
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<InvoiceProvider>().As<IInvoiceProvider>().InstancePerRequest();
            builder.RegisterType<InvoicePdfDocumentProvider>().As<IInvoicePdfDocumentProvider>().InstancePerRequest();

            //Set the dependency resolver to be Autofac.  
            Container = builder.Build();

            return Container;
        }
    }
}
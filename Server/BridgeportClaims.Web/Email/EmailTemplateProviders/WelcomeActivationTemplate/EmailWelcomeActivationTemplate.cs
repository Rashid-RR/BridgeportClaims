using System;
using System.IO;
using BridgeportClaims.Web.Models;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace BridgeportClaims.Web.Email.EmailTemplateProviders.WelcomeActivationTemplate
{
    public class EmailWelcomeActivationTemplate : IEmailTemplateProvider
    {
        private const string TemplateKeyInternal = "WelcomeActivation";
        public string GetTemplatedEmailBody<TModel>(TModel model) where TModel : class, new()
        {               
            var config = new TemplateServiceConfiguration
            {
                Language = Language.CSharp,
                EncodedStringFactory = new HtmlEncodedStringFactory(),
                Debug = true
            };
            var service = RazorEngineService.Create(config);
            Engine.Razor = service;

            // Generate the email body from the template file.
            var fullTemplatePath = Path.Combine(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates"), "WelcomeActivation.cshtml");
            ITemplateSource loadedTemplateSource = new LoadedTemplateSource(File.ReadAllText(fullTemplatePath), fullTemplatePath);
            Engine.Razor.AddTemplate(TemplateKeyInternal, loadedTemplateSource);
            return Engine.Razor.RunCompile(loadedTemplateSource, TemplateKeyInternal, typeof(TModel), model);
        }
    }
}
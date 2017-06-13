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
        private const string EmailSubjectInternal = "Thank you for Registering to BridgeportClaims.com, Please Activate your Email Address";
        private const string TemplateKeyInternal = "WelcomeActivation";

        public string EmailSubject => EmailSubjectInternal;

        public string GetTemplatedEmailBody(string baseUrl)
        {
            var model = new AspNetUsersModel
            {
                FirstName = "Josephat",
                LastName = "Ogwayi",
                AccountActivationToken = "ABCD",
                HostName = baseUrl
            };
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
            return Engine.Razor.RunCompile(loadedTemplateSource, TemplateKeyInternal, typeof(AspNetUsersModel), model);
        }
    }
}
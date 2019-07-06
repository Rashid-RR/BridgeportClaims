using System;
using System.IO;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using BridgeportClaims.Web.Email.Models;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace BridgeportClaims.Web.Email.EmailTemplateProviders
{
    public class EmailTemplateProvider: IEmailTemplateProvider
    {
        public string GetTemplatedEmailBody<TModel>(TModel model) 
            where TModel : class, new()
        {
            var emailViewModel = model as EmailViewModel;
            if (null == emailViewModel)
                throw new ArgumentNullException(nameof(emailViewModel));
            var templateKeyInternal = emailViewModel.EmailModelEnum == EmailModelEnum.WelcomeActivation ? "WelcomeActivation" : "PasswordReset";
            string razorFile;
            switch (emailViewModel.EmailModelEnum)
            {
                case EmailModelEnum.WelcomeActivation:
                    razorFile = "WelcomeActivation.cshtml";
                    break;
                case EmailModelEnum.LakerImportStatus:
                    razorFile = "LakerImportStatus.cshtml";
                    break;
                case EmailModelEnum.PasswordReset:
                    razorFile = "PasswordReset.cshtml";
                    break;
                case EmailModelEnum.EnvisionImportStatus:
                    razorFile = "EnvisionImportStatus.cshtml";
                    break;
                case EmailModelEnum.Unknown:
                    razorFile = string.Empty;
                    break;
                default:
                    throw new Exception("Could not find valid email model");
            }
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
                AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates"), razorFile);
            ITemplateSource loadedTemplateSource = new LoadedTemplateSource(File.ReadAllText(fullTemplatePath), fullTemplatePath);
            Engine.Razor.AddTemplate(templateKeyInternal, loadedTemplateSource);
            return Engine.Razor.RunCompile(loadedTemplateSource, templateKeyInternal, typeof(TModel), model);
        }
    }
}
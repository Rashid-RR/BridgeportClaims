using System;
using System.Diagnostics.CodeAnalysis;
using BridgeportClaims.Data.DataProviders.Documents;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Web.Models;

namespace BridgeportClaims.Web.SignalR
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DocumentsService
    {
        private static readonly Lazy<DocumentsService> _instance = new Lazy<DocumentsService>(() => new DocumentsService());
        private static readonly object Semiphore = new object();
        private bool _gettingDocuments = false;
        private readonly IDocumentsProvider _documentsProvider;
        public static DocumentsService Instance => _instance.Value;

        private DocumentsService() => _documentsProvider = new DocumentsProvider();

        public DocumentsDto GetDocuments(DocumentViewModel model)
        {
            lock (Semiphore)
            {
                DocumentsDto result = null;
                if (!_gettingDocuments)
                {
                    _gettingDocuments = true;
                    result = _documentsProvider.GetDocuments(model.Date, model.Sort, model.SortDirection,
                        model.Page,
                        model.PageSize);
                    _gettingDocuments = false;
                }
                return result;
            }
        }
    }
}
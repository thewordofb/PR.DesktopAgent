using DesktopAgent.Scan.Shared;
using System.Collections.Generic;

namespace DesktopAgent.Scan.Services
{
    public class DocumentService : IDocumentService
    {
        private IDictionary<string, IDocument> Documents = new Dictionary<string, IDocument>();

        public IDocument GetDocumentById(string docId)
        {
            return Documents[docId] as IDocument;
        }

        public void AddDocument(IDocument document)
        {
            Documents.Add(document.DocumentId, document);
        }

        public void AddDocuments(IEnumerable<IDocument> documents)
        {
            foreach(IDocument document in documents)
            {
                Documents.Add(document.DocumentId, document);
            }
        }
    }
}

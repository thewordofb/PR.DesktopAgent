using DesktopAgent.Scan.Shared;
using System.Collections.Generic;

namespace DesktopAgent.Scan.Services
{
    public interface IDocumentService
    {
        IDocument GetDocumentById(string docId);
        void AddDocument(IDocument document);
        void AddDocuments(IEnumerable<IDocument> documents);
    }
}

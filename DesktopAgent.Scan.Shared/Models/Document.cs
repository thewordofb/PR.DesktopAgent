using System;

namespace DesktopAgent.Scan.Shared
{
    public sealed class Document : IDocument
    {
        public Document()
        {
            DocumentId = Guid.NewGuid().ToString();
        }

        public string DocumentId { get; set; }
        public byte[] Image { get; set; }
    }
}

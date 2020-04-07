namespace DesktopAgent.Scan.Shared
{
    public interface IDocument
    {
        string DocumentId { get; set; }
        byte[] Image { get; set; }
    }
}

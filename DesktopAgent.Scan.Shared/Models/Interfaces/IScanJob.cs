namespace DesktopAgent.Scan.Shared
{
    /*
    public enum ScanJobStatus
    {
        InProgress,
        Completed,
        Failed
    }
    */
    
    public interface IScanJob
    {
        string ScanJobId { get; set; }
        string Status { get; set; }
        IScanJobRequest Request { get; set; }
    }
}

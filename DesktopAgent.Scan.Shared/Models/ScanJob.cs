using System;

namespace DesktopAgent.Scan.Shared
{
    public sealed class ScanJob : IScanJob
    {
        public ScanJob()
        {
            ScanJobId = Guid.NewGuid().ToString();
            Status = "In Progress";
        }

        public string ScanJobId { get; set; }
        public string Status { get; set; }
        public IScanJobRequest Request { get; set; }
    }
}

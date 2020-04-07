using System.Collections.Generic;

namespace DesktopAgent.Scan.Shared
{
    public sealed class ScanJobStatus : IScanJobStatus
    {
        public string Status { get; set; }
        public string JobId { get; set; }
        public string DeviceId { get; set; }
        public IEnumerable<string> DocumentIds { get; set; } = new List<string>();
    }
}

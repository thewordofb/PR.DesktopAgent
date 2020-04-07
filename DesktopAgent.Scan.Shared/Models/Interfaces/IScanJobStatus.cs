using System.Collections.Generic;

namespace DesktopAgent.Scan.Shared
{
    public interface IScanJobStatus
    {
        string Status { get; set; }
        string JobId { get; set; }
        string DeviceId { get; set; }
        IEnumerable<string> DocumentIds { get; set; }
    }
}

using DesktopAgent.Scan.Shared;
using System.Collections.Generic;

namespace DesktopAgent.Scan.Services
{
    public interface IScannerService
    {
        IEnumerable<IDevice> GetAllScanners();
        IDevice GetScannerById(string deviceId);
    }
}

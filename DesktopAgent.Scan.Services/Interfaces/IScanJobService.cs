using DesktopAgent.Scan.Shared;
using System.Collections.Generic;

namespace DesktopAgent.Scan.Services
{
    public interface IScanJobService
    {
        IEnumerable<IScanJob> GetAllScanJobsByDeviceId(string deviceId);
        IScanJob CreateScanJobFromRequest(string deviceId, IScanJobRequest request);
        IScanJob CancelScanJobById(string deviceId, string jobId);
        IScanJob GetScanJobById(string deviceId, string jobId);
        IScanJobStatus GetJobStatusById(string deviceId, string jobId);
    }
}

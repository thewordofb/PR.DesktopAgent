using DesktopAgent.Scan.Shared;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace DesktopAgent.Scan.Services
{
    public class ScanJobService : IScanJobService
    {
        private IDictionary<(string deviceId, string jobId), IScanJob> ScanJobs = new Dictionary<(string deviceId, string jobId), IScanJob>();

        private IDocumentService documentService;
        public ScanJobService(IDocumentService docService)
        {
            documentService = docService;
        }

        public IScanJob CreateScanJobFromRequest(string deviceId, IScanJobRequest request)
        {
            IScanJob job = new ScanJob();
            job.Request = request;
            ScanJobs.Add((deviceId: deviceId, jobId: job.ScanJobId), job);

            Task.Run(() =>
           {
               List<Image> images = ScannerProvider.Scan(request.DeviceId);

               foreach (Image image in images)
               {
                   Document doc = new Document();

                   using (MemoryStream ms = new MemoryStream())
                   {
                       image.Save(ms, image.RawFormat);
                       doc.Image = ms.ToArray();
                   }
                   documentService.AddDocument(doc);
                   job.Status = "Completed";
               }
           });

            return job;
        }

        public IScanJob CancelScanJobById(string deviceId, string jobId)
        {
            //We really want to cancel here, we need to put a cancellation token of sorts
            //on the ScanJob so we can signal it.
            IScanJob scanJob = ScanJobs[(deviceId: deviceId, jobId: jobId)];
            scanJob.Status = "Cancelled";

            //cancel here!
            return scanJob;
        }

        public IEnumerable<IScanJob> GetAllScanJobsByDeviceId(string deviceId)
        {
            foreach( var key in ScanJobs.Keys)
            {
                if( key.deviceId == deviceId)
                {
                    yield return ScanJobs[key];
                }
            }
        }

        public IScanJobStatus GetJobStatusById(string deviceId, string jobId)
        {
            IScanJob scanJob = ScanJobs[(deviceId: deviceId, jobId: jobId)];

            IScanJobStatus jobStatus = new ScanJobStatus();
            jobStatus.DeviceId = deviceId;
            jobStatus.JobId = jobId;
            jobStatus.Status = scanJob.Status;
            //Todo we need to place the document ids somewhere
            jobStatus.DocumentIds = new List<string>();

            return jobStatus;
        }

        public IScanJob GetScanJobById(string deviceId, string jobId)
        {
            return ScanJobs[(deviceId: deviceId, jobId: jobId)];
        }
    }
}

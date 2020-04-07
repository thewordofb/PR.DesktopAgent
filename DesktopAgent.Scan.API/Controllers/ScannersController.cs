using DesktopAgent.Scan.Services;
using DesktopAgent.Scan.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace desktopAPI.Controllers
{
    [Route("scanners")]
    [ApiController]
    public class ScannersController : ControllerBase
    {
        private IScannerService scannerService;
        private IScanJobService scanJobService;
        private IDocumentService documentService;

        public ScannersController(IScannerService scanService, IScanJobService jobService, IDocumentService docService)
        {
            scannerService = scanService;
            scanJobService = jobService;
            documentService = docService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Device>> GetAllScanners()
        {
            try
            {
                IEnumerable<IDevice> devices = scannerService.GetAllScanners();

                if (devices != null)
                {
                    return new OkObjectResult(devices);
                }
                else
                {
                    return new NoContentResult();
                }
            }
            catch (Exception)
            {
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Title = "All Scanners",
                    Detail = "Unable to enumerate scanners",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = HttpContext.Request.Path
                };
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        [HttpGet("{deviceId}")]
        public ActionResult<IDevice> GetScannerById(string deviceId)
        {
            try
            {
                IDevice device = scannerService.GetScannerById(deviceId);

                if (device != null)
                {
                    return new OkObjectResult(device);
                }
                else
                {
                    return new NoContentResult();
                }
            }
            catch (Exception)
            {
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Title = "Scanner",
                    Detail = "Unable to enumerate scanners",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = HttpContext.Request.Path
                };
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        [HttpGet("{deviceId}/jobs")]
        public ActionResult<IEnumerable<IScanJob>> GetAllJobs(string deviceId)  //Query param to get active/completed/failed
        {
            try
            {
                IEnumerable<IScanJob> scanJobs = scanJobService.GetAllScanJobsByDeviceId(deviceId);
                return new OkObjectResult(scanJobs);
            }
            catch (Exception)
            {
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Title = "Scanner",
                    Detail = "Unable to enumerate scanners",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = HttpContext.Request.Path
                };
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        [HttpPost("{deviceId}/jobs/")]
        public ActionResult<IScanJob> CreateScanJob(string deviceId, [FromBody] ScanJobRequest scanJobRequest)
        {
            try
            {
                IScanJob scanJob = scanJobService.CreateScanJobFromRequest(deviceId, scanJobRequest);
                return new OkObjectResult(scanJob);
            }
            catch (Exception)
            {
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Title = "Scanner",
                    Detail = "Unable to enumerate scanners",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = HttpContext.Request.Path
                };
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        [HttpDelete("{deviceId}/jobs/{jobId}")]
        public ActionResult<IScanJob> DeleteScanJob(string deviceId, string jobId)
        {
            try
            {
                IScanJob scanJob = scanJobService.CancelScanJobById(deviceId, jobId);
                return new OkObjectResult(scanJob);
            }
            catch (Exception)
            {
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Title = "Scanner",
                    Detail = "Unable to enumerate scanners",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = HttpContext.Request.Path
                };
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        [HttpGet("{deviceId}/jobs/{jobId}")]
        public ActionResult<IScanJob> GetJobById(string deviceId, string jobId)
        {
            try
            {
                IScanJob scanJob = scanJobService.GetScanJobById(deviceId, jobId);
                return new OkObjectResult(scanJob);
            }
            catch (Exception)
            {
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Title = "Scanner",
                    Detail = "Unable to enumerate scanners",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = HttpContext.Request.Path
                };
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        [HttpGet("{deviceId}/jobs/{jobId}/status")]
        public ActionResult<IScanJobStatus> GetJobStatusById(string deviceId, string jobId)
        {
            try
            {
                IScanJobStatus jobStatus = scanJobService.GetJobStatusById(deviceId, jobId);

                return new OkObjectResult(jobStatus);
            }
            catch(Exception)
            {
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Title = "Scanner",
                    Detail = "Unable to enumerate scanners",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = HttpContext.Request.Path
                };
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

    }
}

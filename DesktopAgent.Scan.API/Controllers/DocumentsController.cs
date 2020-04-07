using DesktopAgent.Scan.Services;
using DesktopAgent.Scan.Shared;
using Microsoft.AspNetCore.Mvc;

namespace desktopAPI.Controllers
{
    [Route("documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private IDocumentService documentService;

        public DocumentsController(IDocumentService docService)
        {
            documentService = docService;
        }

        [HttpGet("{id}")]
        public ActionResult<Document> Get(string id)
        {
            return null;
        }
    }
}

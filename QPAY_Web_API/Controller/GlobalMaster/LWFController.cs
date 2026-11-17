using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.GlobalMaster;
using static QPay.UI.Models.GlobalMaster.LWFClass;

namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class LWFController : ControllerBase
    {
        private readonly ILWFRepository _IRepository;
        public LWFController(ILWFRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet]
        [Route("GetLWFSlabSearch")]
        public async Task<IActionResult> GetLWFSlabSearch()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.GetLWFSlabSearch(schema);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("GetLWFSlabExporttoExcel")]
        public async Task<IActionResult> GetLWFSlabExporttoExcel()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();

            var ds = await _IRepository.GetLWFSlabExporttoExcel(schema);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("CreateUpdateDeleteLWFSlab")]
        public async Task<IActionResult> CreateUpdateDeleteLWFSlab(LWFSlabRequest request)
        {
            var result = await _IRepository.CreateUpdateDeleteLWFSlab(request);
            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.GlobalMaster;
using static QPay.UI.Models.GlobalMaster.PFClass;

namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class PFController : ControllerBase
    {
        private readonly IPFRepository _IRepository;
        public PFController(IPFRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet, Route("PFPayCodes")]
        public async Task<IActionResult> PFPayCodes()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.PFPayCodes(schema);
            return Ok(response);
        }


        [HttpGet, Route("PFCapType")]
        public async Task<IActionResult> PFCapType()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.PFCapType(schema);
            return Ok(response);
        }

        [HttpGet, Route("PFSearch/{CapType}/{Paycode}")]
        public async Task<IActionResult> PFSearch(int CapType, int Paycode)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.PFSearch(CapType, Paycode, schema);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GetPFDetails/{providentFundId}")]
        public async Task<IActionResult> GetPFDetails(int providentFundId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.GetPFDetails(providentFundId, schema);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("PFExporttoExcel/{CapType}")]
        public async Task<IActionResult> PFExporttoExcel(string CapType)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _IRepository.PFExporttoExcel(CapType, schema);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("CreateUpdatePF")]
        public async Task<IActionResult> CreateUpdatePF(PFRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var result = await _IRepository.CreateUpdatePF(request, schema);
            return Ok(result);
        }

        [HttpPost]
        [Route("DeletePF")]
        public async Task<IActionResult> DeletePF(PFDeleteRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var result = await _IRepository.DeletePF(request, schema);
            return Ok(result);
        }
    }
}

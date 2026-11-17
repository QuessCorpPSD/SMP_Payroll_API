using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Invoice;
using QPay_Web_Domain.Models;
using static QPay.UI.Invoice.InvoiceInitiationAgainstProfoma;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceInitiationAgainstProfomaController : ControllerBase
    {
        private readonly IInvoiceInitiationAgainstProfomaRepository _IRepository;
        public InvoiceInitiationAgainstProfomaController(IInvoiceInitiationAgainstProfomaRepository IRepository)
        {
            this._IRepository = IRepository;
        }


        [HttpPost]
        [Route("ProfomaImport")]
        public async Task<IActionResult> ProfomaImport(IFormFile file, [FromForm] int createdBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.ProfomaImport(file, createdBy);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetProfoma/{CompanyId}/{PayperiodId}/{flag}")]
        public async Task<IActionResult> GetProfoma(int CompanyId, int PayperiodId, string flag)
        {
            var ds = await _IRepository.GetProfoma(CompanyId, PayperiodId, flag);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost("Initiate")]
        public async Task<IActionResult> Initiate([FromBody] InitiateRequestDto request)
        {
            var res = await this._IRepository.Initiate(request);
            return Ok(res);
        }
    }
}

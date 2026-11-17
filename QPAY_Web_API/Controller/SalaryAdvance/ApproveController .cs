using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.BAL.IRepository.PayrollInput;
using QPay.BAL.IRepository.SalaryAdvance;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPay_Web_Domain.Models;

namespace QPay.API.Controller.SalaryAdvance
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproveController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IApproveRepository _IRepository;
        public ApproveController(IApproveRepository IRepository,  IConfiguration configuration)
        {
            this._IRepository = IRepository;
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("Search/{CompanyId}/{PayPeriod_Id}")]
        public async Task<IActionResult> Search(int? CompanyId, int? PayPeriod_Id)
        {
            var response = await _IRepository.Search(CompanyId, PayPeriod_Id, 0);
            var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
            return Ok(_outputResponse);
        }


        [HttpGet]
        [Route("GetTemplate")]
        public async Task<IActionResult> GetTemplate()
        {
            var response = await _IRepository.GetTemplate();
            if (response != null)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string CreatedBy,
          [FromForm] string CompanyCode, [FromForm] int Payperiod_id,[FromForm] string Payperiod)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.Upload(file, CreatedBy, CompanyCode, Payperiod_id, Payperiod);
            return Ok(result);
        }


    }
}

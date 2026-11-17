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
    public class ReportController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IReportRepository _IRepository;
        public ReportController(IReportRepository IRepository,  IConfiguration configuration)
        {
            this._IRepository = IRepository;
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("Search/{CompanyId}/{PayPeriod_Id}")]
        public async Task<IActionResult> Search(int? CompanyId, int? PayPeriod_Id)
        {
            var response = await _IRepository.Search(CompanyId, PayPeriod_Id);
            var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
            return Ok(_outputResponse);
            
        }

    }
}

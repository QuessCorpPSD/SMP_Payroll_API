using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Master;
using static QPay.UI_Domain.Models.Master.LeaveOpening;

namespace QPay_Web_API.Controller.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveOpeningBalanceController : ControllerBase
    {
        private readonly ILeaveOpeningBalanceRepository _ileave;
        public LeaveOpeningBalanceController(
          ILeaveOpeningBalanceRepository ileave)
        {
            this._ileave = ileave;
        }

        [HttpGet]
        [Route("GetLeaveOpeingBalance/{CompanyId}/{Sitename}")]
        public async Task<IActionResult> GetLeaveOpeingBalance(string CompanyId, string Sitename)
        {
            var ds = await _ileave.GetLeaveOpeingBalance(CompanyId, Sitename);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("GetLeaveOpeingBalanceTemplate")]
        public async Task<IActionResult> GetLeaveOpeingBalanceTemplate(LeaveOpeningTemplateRequest leave)
        {
            var ds = await _ileave.GetLeaveOpeingBalanceTemplate(leave);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("ImportLeaveOpeingBalance")]
        public async Task<IActionResult> ImportLeaveOpeingBalance(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string Fromdate, [FromForm] string Todate, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _ileave.ImportLeaveOpeingBalance(file, CompanyId, Fromdate, Todate, CreatedBy);
            return Ok(result);
        }
    }
}

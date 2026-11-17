using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Master;
using static QPay.UI_Domain.Models.Master.LeaveRule;

namespace QPay_Web_API.Controller.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRuleController : ControllerBase
    {
        private readonly ILeaveRuleRepository _ileaverule;
        public LeaveRuleController(
          ILeaveRuleRepository ileaverule)
        {
            this._ileaverule = ileaverule;
        }

        [HttpGet]
        [Route("GetLeaveRuleCompanywise/{CompanyId}/{SiteId}/{State}/")]
        public async Task<IActionResult> GetLeaveRuleCompanywise(int CompanyId, int SiteId,string State)
        {
            var ds = await _ileaverule.GetLeaveRuleCompanywise(CompanyId, SiteId, State);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet]
        [Route("GetLeaveRuleTemplate")]
        public async Task<IActionResult> GetLeaveRuleTemplate()
        {
            var ds = await _ileaverule.GetLeaveRuleTemplate();
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("UploadLeaveRule")]
        public async Task<IActionResult> UploadLeaveRule(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _ileaverule.UploadLeaveRule(file, User);
            return Ok(result);
        }

        [HttpGet, Route("GetLeaveType/{CompanyId}")]
        public async Task<IActionResult> GetLeaveType(string CompanyId)
        {
            var response = await _ileaverule.GetLeaveType(CompanyId);

            return Ok(response);
        }

        [HttpPost("SaveUpdateDeleteLeaveRule")]
        public async Task<IActionResult> SaveUpdateDeleteLeaveRule([FromBody] LeaveRuleRequest request)
        {
            var res = await this._ileaverule.SaveUpdateDeleteLeaveRule(request);
            return Ok(res);
        }
    }
}

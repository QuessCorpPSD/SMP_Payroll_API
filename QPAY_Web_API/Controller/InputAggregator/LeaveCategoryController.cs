using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.InputAggregator;
using static QPay.UI.Models.Aggregator.LeaveCategory;


namespace QPay.API.Controllers.InputAggregator
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveCategoryController : ControllerBase
    {
        private readonly ILeaveCategoryRepository _iholiday;
        public LeaveCategoryController(
          ILeaveCategoryRepository iholiday)
        {
            this._iholiday = iholiday;
        }

        [HttpGet]
        [Route("GetLeaveCategory/{CompanyId}/{siteid}/{Flag}")]
        public async Task<IActionResult> GetLeaveCategory(int CompanyId, int siteid, string Flag)
        {
            var ds = await _iholiday.GetLeaveCategory(CompanyId, siteid, Flag);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost("SaveUpdateDeleteLeaveCategory")]
        public async Task<IActionResult> SaveUpdateDeleteLeaveCategory([FromBody] LeaveCategoryRequest request)
        {
            var res = await this._iholiday.SaveUpdateDeleteLeaveCategory(request);
            return Ok(res);
        }
    }
}

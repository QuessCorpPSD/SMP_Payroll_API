using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Master;
using static QPay.UI_Domain.Models.Master.Holiday;


namespace QPay_Web_API.Controller.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayMasterController : ControllerBase
    {
        private readonly IHolidayMasterRepository _iholiday;
        public HolidayMasterController(
          IHolidayMasterRepository iholiday)
        {
            this._iholiday = iholiday;
        }

        [HttpGet]
        [Route("GetHolidayCompanywise/{CompanyId}/{SiteId}")]
        public async Task<IActionResult> GetHolidayCompanywise(int CompanyId, int SiteId)
        {
            var ds = await _iholiday.GetHolidayCompanywise(CompanyId, SiteId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet]
        [Route("GetHolidayTemplate")]
        public async Task<IActionResult> GetHolidayTemplate()
        {
            var ds = await _iholiday.GetHolidayTemplate();
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("UploadHolidayMaster")]
        public async Task<IActionResult> UploadHolidayMaster(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _iholiday.UploadHolidayMaster(file, User);
            return Ok(result);
        }

        [HttpPost("SaveUpdateDeleteHolidayMaster")]
        public async Task<IActionResult> SaveUpdateDeleteHolidayMaster([FromBody] HolidayMasterRequest request)
        {
            var res = await this._iholiday.SaveUpdateDeleteHolidayMaster(request);
            return Ok(res);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Reports;
using QPay.UI.Models.Reports;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class SGPayRegisterController : ControllerBase
    {
        private readonly ISGPayRegisterRepository _poReport;
        private readonly IConfiguration _configuration;

        public SGPayRegisterController(
            ISGPayRegisterRepository iporeport, IConfiguration configuration)
        {
            this._poReport = iporeport;
            this._configuration = configuration;
        }

        [HttpPost, Route("ReportDownload")]
        public async Task<IActionResult> ReportDownload(SGPay request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _poReport.ReportDownload(request, schema);
            if (response.Tables[0].Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }
    }
}

using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Master;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPay.UI_Domain.Models.Master;
using System.Data;

namespace QPay_Web_API.Controller.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveMasterController : ControllerBase
    {
        private readonly ILeaveMasterRepository _LeaveMaster;
        private readonly IConfiguration _configuration;
        public LeaveMasterController(ILeaveMasterRepository leaveMaster, IConfiguration configuration)
        {
            this._LeaveMaster = leaveMaster;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetAllLeaveMaster/{companyId}/{siteId}")]
        public async Task<IActionResult> GetAllLeaveMaster(int? companyId, string? siteId)
        {
            var response = await _LeaveMaster.GetAllLeaveMaster(companyId, siteId);

            return Ok(response);
        }

        [HttpGet, Route("GetLeaveTypeDD")]
        public async Task<IActionResult> GetLeaveTypeDD()
        {
            var response = await _LeaveMaster.GetLeaveTypeDD();

            return Ok(response);
        }


        [HttpPost]
        [Route("PostAddLeaveMaster")]
        public async Task<IActionResult> PostAddLeaveMaster([FromBody] LeaveMasterAdd leavemasteradd)
        {
            if (!ModelState.IsValid)
                return Ok(ModelState);
            var response = await _LeaveMaster.PostAddLeaveMaster(leavemasteradd);
            return Ok(response);
        }

        [HttpGet]
        [Route("LeaveMasterExport/{companyId}/{siteCode}")]
        public IActionResult LeaveMasterExport(int companyId, int siteCode)
        {

            DataSet ds = _LeaveMaster.LeaveMasterExport(companyId, siteCode);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.AddWorksheet("LeaveMaster");
                ws.Cell(1, 1).InsertTable(ds.Tables[0], "NewDataSet", true);

                var table = ws.Table("NewDataSet");
                table.ShowAutoFilter = false;
                table.Theme = XLTableTheme.None;

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var bytes = Convert.ToBase64String(stream.ToArray());

                    FileResponse fileResponse = new FileResponse();
                    string fileName = DateTime.Now.ToString();
                    fileResponse.FileName = "LeaveaMaster_Export_" + fileName + ".xlsx";
                    fileResponse.File = bytes;
                    return Ok(fileResponse);
                }
            }
            else
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = "Failure",
                    Data = "",
                    Error = ""
                };
                return Ok(response);
            }

        }
        [HttpPost("PostDeleteLeave")]
        public async Task<IActionResult> PostDeleteLeave([FromBody] int leavemasterid)
        {

            var response = await _LeaveMaster.PostDeleteLeave(leavemasterid);
            return Ok(response);

        }

    }
}

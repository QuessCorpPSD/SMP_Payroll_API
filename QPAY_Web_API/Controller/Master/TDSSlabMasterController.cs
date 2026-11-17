using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Master;
using QPay.BAL.Repository.Master;
using QPay.UI.Master;

namespace QPay.API.Controller.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class TDSSlabMasterController : ControllerBase
    {
        private readonly ITDSSlabMasterRepository _IRepository;
        public TDSSlabMasterController(ITDSSlabMasterRepository _IRepository)
        {
            this._IRepository = _IRepository;
        }

        [HttpGet, Route("Category")]
        public async Task<IActionResult> Category()
        {
            var bloodgroups = new List<string>() { "Semi-monthly", "Monthly", "Consultant" };
            var payload = ResponseWrapManager.ResponseWrapper(bloodgroups, HttpContext);
            return Ok(payload);

        }


        [HttpGet]
        [Route("GetFinancialYear")]
        public async Task<IActionResult> GetFinancialYear()
        {
            var response = await _IRepository.GetFinancialYear();
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
        [HttpGet]
        [Route("Search/{SearchCategory}/{tdsSlabId}")]
        public async Task<IActionResult> Search(string SearchCategory, int? tdsSlabId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.Search( SearchCategory, tdsSlabId, schema);
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

        [HttpGet]
        [Route("GetAllTDSSlabDetail/{tdsSlabId}")]
        public async Task<IActionResult> GetAllTDSSlabDetail(int? tdsSlabId)
        {
            var response = await _IRepository.GetAllTDSSlabDetail(tdsSlabId);
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
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] TDSSlabRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.Create(request, schema);
            if (response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
                if (!(message.Contains("successfully")))
                {
                    return Ok(new { StatusCode = "400", Message = response.Tables[0].Rows[0]["Error_Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }
        [HttpGet]
        [Route("ExporttoExcel/{FinancialYearId}/{SearchCategory}")]
        public async Task<IActionResult> ExporttoExcel(int? FinancialYearId, string SearchCategory)
        {
            var response = await _IRepository.ExporttoExcel(FinancialYearId, SearchCategory);
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

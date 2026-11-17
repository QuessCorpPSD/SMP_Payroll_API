using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Master;
using QPay.UI.Master;

namespace QPay.API.Controller.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputationRuleController : ControllerBase
    {
        private readonly IComputationRuleRepository _IRepository;
        public ComputationRuleController(IComputationRuleRepository _IRepository)
        {
            this._IRepository = _IRepository;
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
        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> Search(ComputationSearch request)
        {
            var response = await _IRepository.Search(request.FinancialYearId, request.Category, request.TaxId);
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
        public async Task<IActionResult> Create([FromBody]  ComputationRuleXml items)
        {
            var response = await _IRepository.Create(items);
            if (response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
                if (!(message.Contains("Successfully")))
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
                return Ok(new { StatusCode = "400", Message = "Paycode Created Failed" });
            }
        }

        [HttpPost]
        [Route("ExporttoExcel")]
        public async Task<IActionResult> ExporttoExcel(ComputationSearch request)
        {
            var response = await _IRepository.ExporttoExcel(request.FinancialYearId, request.Category, request.TaxId);
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

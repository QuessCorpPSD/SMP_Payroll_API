using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.UI.GlobalMaster;

namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormulaController : ControllerBase
    {
        private readonly IFormulaRepository _IRepository;
        public FormulaController(IFormulaRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet]
        [Route("Search/{payCodeId}")]
        public async Task<IActionResult> Search(int? payCodeId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.Search(payCodeId, schema);
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
        [Route("GetPayCategory/{companyId}")]
        public async Task<IActionResult> GetPayCategory(int companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.GetPayCategory(companyId, schema);
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
        public async Task<IActionResult> Create([FromBody] FormulasRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.Create(request, schema);
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
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }

        [HttpGet]
        [Route("GetPayrollType")]
        public async Task<IActionResult> GetPayrollType()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.GetPayrollType(schema);
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
        [Route("CreateMC")]
        public async Task<IActionResult> CreateMC([FromBody] MCFormulasRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.CreateMC(request, schema);
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
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }

        [HttpGet]
        [Route("MCSearch")]
        public async Task<IActionResult> MCSearch()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.MCSearch(schema);
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

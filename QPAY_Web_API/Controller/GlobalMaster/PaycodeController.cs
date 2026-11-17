using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System.Net;

namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaycodeController : ControllerBase
    {
        private readonly IPaycodeRepository _IRepository;
        public PaycodeController(
            IPaycodeRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> Search(PaycodeSearchParams _params)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.Search(_params.paycode_Code, _params.PayTypeId, _params.IsTaxable, _params.PayId, schema);
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
        [Route("GetPageType")]
        public async Task<IActionResult> GetPageType()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.GetPageType(schema);
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
        [Route("GetPayType")]
        public async Task<IActionResult> GetPayType()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.GetPayType(schema);
            if (response!=null)
            {
                return Ok(response);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }



        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(PaycodeCreateParams _params)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.Create(_params, schema);
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




    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.InputAggregator;
using QPay.UI.Models.Aggregator;


namespace QPay.API.Controllers.InputAggregator
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IInputAggregatorAttendanceRepository _IRepository;
        public AttendanceController (IConfiguration configuration, IInputAggregatorAttendanceRepository Repository)
        {
            _configuration = configuration;
            _IRepository = Repository;
        }


        #region for mapping 

        [HttpGet]
        [Route("QuessLeaveMaster")]
        public async Task<IActionResult> QuessLeaveMaster()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.QuessLeaveMaster(schema);
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
        [Route("leaveTypeMaster")]
        public async Task<IActionResult> leaveTypeMaster()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.leaveTypeMaster(schema);
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
        [Route("Createleavetype")]
        public async Task<IActionResult> Createleavetype([FromBody] leaveTypeMasterRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.Createleavetype(request, schema);
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

        [HttpPost]
        [Route("Createleavemapping")]
        public async Task<IActionResult> Createleavemapping([FromBody] AttendanceAggregatorRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.Createleavemapping(request,schema);
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
        [Route("SearchLeaveTypeMapping/{companyId}/{siteid}/{Flag}")]
        public async Task<IActionResult> SearchLeaveTypeMapping(int? companyId, int? siteid, string? Flag)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.SearchLeaveTypeMapping(companyId, siteid, Flag, schema);
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
        [Route("LeavePolicyMaster")]
        public async Task<IActionResult> LeavePolicyMaster()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.LeavePolicyMaster(schema);
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

        //------------------------------------------------------------------------------------

        [HttpGet]
        [Route("QuessAttendanceAttributeMaster")]
        public async Task<IActionResult> QuessAttendanceAttributeMaster()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.QuessAttendanceAttributeMaster(schema);
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
        [Route("QuessAttendanceAttributeClient/{companyId}")]
        public async Task<IActionResult> QuessAttendanceAttributeClient(int? companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.QuessAttendanceAttributeClient(companyId,schema);
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
        [Route("ClientAttendanceAttributeMaster/{companyId}/{siteid}")]
        public async Task<IActionResult> ClientAttendanceAttributeMaster(int? companyId, int? siteid)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.ClientAttendanceAttributeMaster(companyId, siteid,schema);
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
        [Route("ClientAttendanceAttribute/{companyId}/{siteid}")]
        public async Task<IActionResult> ClientAttendanceAttribute(int? companyId, int? siteid)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.ClientAttendanceAttribute(companyId, siteid, schema);
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
        [Route("Search/{companyId}/{siteid}")]
        public async Task<IActionResult> Search(int? companyId, int? siteid)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.Search(companyId, siteid, schema);
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
        [Route("ClientAttributesUpload")]
        public async Task<IActionResult> ClientAttributesUpload(IFormFile file, [FromForm] string CreatedBy)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.ClientAttributesUpload(file, CreatedBy, schema);
            return Ok(result);
        }

        [HttpPost]
        [Route("AttributesMappingUpload")]
        public async Task<IActionResult> AttributesMappingUpload(IFormFile file, [FromForm] string CreatedBy)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.AttributesMappingUpload(file, CreatedBy, schema);
            return Ok(result);
        }

        [HttpPost]
        [Route("AttributesMappingCreate")]
        public async Task<IActionResult> AttributesMappingCreate(AttributesMappingRequest items)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.AttributesMappingCreate(items, schema);
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


        #endregion for mapping



        #region for billing report generation

        [RequestSizeLimit(104857600)]
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId, [FromForm] string SiteId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.Upload(file, CreatedBy, CompanyId, PayPeriodId, SiteId, schema);
            return Ok(result);
        }

        [HttpGet]
        [Route("AttendanceReport/{companyId}/{payPeriodId}/{siteid}")]
        public async Task<IActionResult> AttendanceReport(int? companyId, int? payPeriodId, int? siteid)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.AttendanceReport(companyId, payPeriodId, siteid, schema);
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
        [Route("billableReport/{companyId}/{payPeriodId}/{siteid}")]
        public async Task<IActionResult> billableReport(int? companyId, int? payPeriodId, int? siteid)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.billableReport(companyId, payPeriodId, siteid, schema);
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

        #endregion for billing report generation

        [HttpGet]
        [Route("ClientAttributesDelete/{companyId}/{siteid}/{templateId}/{CreatedBy}")]
        public async Task<IActionResult> ClientAttributesDelete(int? companyId,int? siteid, int? templateId, string CreatedBy)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.ClientAttributesDelete(companyId, siteid, templateId, CreatedBy, schema);
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
        [Route("FinalSubmission/{companyId}/{payPeriodId}")]
        public async Task<IActionResult> FinalSubmission(int? companyId, int? payPeriodId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _IRepository.FinalSubmission(companyId, payPeriodId, schema);
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

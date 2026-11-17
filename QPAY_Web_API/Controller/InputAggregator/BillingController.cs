using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.InputAggregator;
using QPay.UI.Models.Aggregator;


namespace QPay.API.Controller.InputAggregator
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IInputAggregatorRepository _IRepository;
        private readonly IConfiguration _configuration;
        public BillingController(IConfiguration configuration, IInputAggregatorRepository Repository)
        {
            this._IRepository = Repository;
            this._configuration = configuration;
        }

        #region for mapping 

        [HttpGet]
        [Route("QuessAttributeMaster")]
        public async Task<IActionResult> QuessAttributeMaster()
        {
            var response = await _IRepository.QuessAttributeMaster();
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
        [Route("ClientAttributes/{companyId}/{siteid}")]
        public async Task<IActionResult> ClientAttributes(int? companyId, int? siteid)
        {
            var response = await _IRepository.ClientAttributes(companyId, siteid);
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
            var response = await _IRepository.Search(companyId, siteid);
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
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.ClientAttributesUpload(file, CreatedBy);
            return Ok(result);
        }

        [HttpPost]
        [Route("AttributesMappingUpload")]
        public async Task<IActionResult> AttributesMappingUpload(IFormFile file, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.AttributesMappingUpload(file, CreatedBy);
            return Ok(result);
        }

        #endregion for mapping

        #region for billing report generation

        [RequestSizeLimit(104857600)]
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId, [FromForm] string SiteId)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.Upload(file, CreatedBy, CompanyId, PayPeriodId, SiteId);

            return Ok(result);
        }

        [RequestSizeLimit(104857600)]
        [HttpPost]
        [Route("miscUpload")]
        public async Task<IActionResult> MISCUpload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId, [FromForm] string SiteId)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.MISCUpload(file, CreatedBy, CompanyId, PayPeriodId, SiteId);

            return Ok(result);
        }

        [RequestSizeLimit(104857600)]
        [HttpPost]
        [Route("OTRateUpload")]
        public async Task<IActionResult> OTRateUpload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId, [FromForm] string SiteId)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.OTRateUpload(file, CreatedBy, CompanyId, PayPeriodId, SiteId);

            return Ok(result);
        }


        [HttpGet]
        [Route("billableReport/{companyId}/{payPeriodId}")]
        public async Task<IActionResult> billableReport(int? companyId, int? payPeriodId)
        {
            var response = await _IRepository.billableReport(companyId, payPeriodId);
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
        [Route("CompanyPaycode/{companyId}")]
        public async Task<IActionResult> CompanyPaycode(int? companyId)
        {
            var response = await _IRepository.CompanyPaycode(companyId);
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
        [Route("MISCPaycodeSearch/{companyId}/{siteid}/{Flag}")]
        public async Task<IActionResult> MISCPaycodeSearch(int? companyId, int? siteid, string? Flag)
        {
            var response = await _IRepository.MISCPaycodeSearch(companyId, siteid, Flag);
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
        [Route("MISCPaycodes/{companyId}/{siteid}")]
        public async Task<IActionResult> MISCPaycodes(int? companyId, int siteid)
        {
            var response = await _IRepository.MISCPaycodes(companyId, siteid);
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
        [Route("MISCPaycodeMapping")]
        public async Task<IActionResult> MISCPaycodeMapping([FromBody] MISCPaycodeMappingRequest request)
        {
            var response = await _IRepository.MISCPaycodeMapping(request);
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
        [Route("FinalSubmission/{companyId}/{payPeriodId}")]
        public async Task<IActionResult> FinalSubmission(int? companyId, int? payPeriodId)
        {
            var response = await _IRepository.FinalSubmission(companyId, payPeriodId);
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
        [Route("ClientAttributesDelete/{companyId}/{siteid}/{templateId}/{CreatedBy}")]
        public async Task<IActionResult> ClientAttributesDelete(int? companyId, int? siteid, int? templateId, string CreatedBy)
        {


            var response = await _IRepository.ClientAttributesDelete(companyId, siteid, templateId, CreatedBy);
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
        [Route("ClientAttributesMaster/{companyId}/{siteid}")]
        public async Task<IActionResult> ClientAttributesMaster(int? companyId, int? siteid)
        {
            var response = await _IRepository.ClientAttributesMaster(companyId, siteid);
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
        [Route("MappingAttributesDelete/{companyId}/{templateId}/{Quess_Template_Field_Id}/{CreatedBy}")]
        public async Task<IActionResult> MappingAttributesDelete(int? companyId, int? templateId, int? Quess_Template_Field_Id, string CreatedBy)
        {
            var response = await _IRepository.MappingAttributesDelete(companyId, templateId, Quess_Template_Field_Id, CreatedBy);
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
        [Route("ClientAttributesUploadOI")]
        public async Task<IActionResult> ClientAttributesUploadOI(IFormFile file, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.ClientAttributesUploadOI(file, CreatedBy);
            return Ok(result);
        }

        [HttpPost]
        [Route("AttributesMappingUploadOI")]
        public async Task<IActionResult> AttributesMappingUploadOI(IFormFile file, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.AttributesMappingUploadOI(file, CreatedBy);
            return Ok(result);
        }

        [RequestSizeLimit(104857600)]
        [HttpPost]
        [Route("UploadOI")]
        public async Task<IActionResult> UploadOI(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId, [FromForm] string PayPeriodId)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.UploadOI(file, CreatedBy, CompanyId, PayPeriodId);

            return Ok(result);
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Customer;
using QPay.UI.Customer;
using QPay.UI.Models.Customer;

namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceChargeController : ControllerBase
    {
        private readonly IServiceChargeRepository _IRepository;
        public ServiceChargeController(IServiceChargeRepository IRepository)
        {
            this._IRepository = IRepository;
        }



        [HttpGet]
        [Route("servicechargemaster")]
        public async Task<IActionResult> servicechargemaster()
        {
            var response = await _IRepository.serviceChargeMaster();
            if (response != null)
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
        [Route("servicechargemasterNew/{companyId}")]
        public async Task<IActionResult> servicechargemasterNew(int companyId)
        {
            var response = await _IRepository.serviceChargeMasterNew(companyId);
            if (response != null)
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
        [Route("GetUnitType")]
        public async Task<IActionResult> GetUnitType()
        {
            var response = await _IRepository.GetUnitType();
            if (response != null)
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
        [Route("GetUnitTypeBilltoRate")]
        public async Task<IActionResult> GetUnitTypeBilltoRate()
        {
            var response = await _IRepository.GetUnitTypeBilltoRate();
            if (response != null)
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
        [Route("servicechargetype/{serviceChargeId}")]
        public async Task<IActionResult> servicechargetype(int? serviceChargeId)
        {

            var response = await _IRepository.serviceChargeType(serviceChargeId);
            if (response != null)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ServiceChargeRequest request)
        {
            var res = await this._IRepository.Create(request);
            return Ok(res);
        }


        [HttpPost("ServiceChargeDelete")]
        public async Task<IActionResult> ServiceChargeDelete([FromBody] ServiceChargeDeleteRequest request)
        {
            var res = await this._IRepository.ServiceChargeDelete(request);
            return Ok(res);
        }


        [HttpPost]
        [Route("FileUpload")]
        public async Task<IActionResult> FileUpload(IFormFile file, [FromForm] int ServiceChargeMaster, [FromForm] int ServiceChargeType,
              [FromForm] int SlabType, [FromForm] int SlabInnerType, [FromForm] int CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.FileUpload(file, ServiceChargeMaster, ServiceChargeType, SlabType, SlabInnerType, CreatedBy);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllServiceCharge/{companyId}/{Service_Charge_Master_Id}/{Service_Charge_Type_Id}")]
        public async Task<IActionResult> GetAllServiceCharge(int companyId, int Service_Charge_Master_Id, int Service_Charge_Type_Id)
        {
            var response = await _IRepository.GetAllServiceCharge(companyId, Service_Charge_Master_Id, Service_Charge_Type_Id);
            if (response != null)
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
        [Route("FileUploadServiceFeeBilltoRate")]
        public async Task<IActionResult> FileUploadServiceFeeBilltoRateFileUpload(IFormFile file, [FromForm] int CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.FileUploadServiceFeeBilltoRateFileUpload(file, CreatedBy);
            return Ok(result);
        }

    }
}

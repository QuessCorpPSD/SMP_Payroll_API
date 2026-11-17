using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Customer;
using static QPay.UI.Customer.PermHire;

namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermHireController : ControllerBase
    {
        private readonly IPermHireRepository _IRepository;
        public PermHireController(IPermHireRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpPost]
        [Route("GetServiceChargeType")]
        public async Task<IActionResult> GetServiceChargeType(ServiceChargeTypeRequest request)
        {
            var ds = await _IRepository.GetServiceChargeType(request);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost("ServiceChargeTypeCreate")]
        public async Task<IActionResult> ServiceChargeTypeCreate([FromBody] ServiceChargeTypeDto request)
        {
            var res = await this._IRepository.ServiceChargeTypeCreate(request);
            return Ok(res);
        }

        [HttpPost]
        [Route("GetJobCategorySearch")]
        public async Task<IActionResult> GetJobCategorySearch(JobCategoryRequest request)
        {
            var ds = await _IRepository.GetJobCategorySearch(request);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost("JobCategoryCreate")]
        public async Task<IActionResult> JobCategoryCreate([FromBody] JobCategoryDto request)
        {
            var res = await this._IRepository.JobCategoryCreate(request);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetJobCategory")]
        public async Task<IActionResult> GetJobCategory()
        {
            var ds = await _IRepository.GetJobCategory();

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet]
        [Route("GetJobSubCategorySearch/{CompanyId}")]
        public async Task<IActionResult> GetJobSubCategorySearch(int CompanyId)
        {
            var ds = await _IRepository.GetJobSubCategorySearch(CompanyId);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet]
        [Route("GetJobSubCategoryExport/{CompanyId}")]
        public async Task<IActionResult> GetJobSubCategoryExport(int CompanyId)
        {
            var ds = await _IRepository.GetJobSubCategoryExport(CompanyId);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost("JobSubCategoryCreate")]
        public async Task<IActionResult> JobSubCategoryCreate([FromBody] JobSubCategoryDto request)
        {
            var res = await this._IRepository.JobSubCategoryCreate(request);
            return Ok(res);
        }

        [HttpPost]
        [Route("GetPermHireMasterSearch")]
        public async Task<IActionResult> GetPermHireMasterSearch(PermHireMasterRequest request)
        {
            var ds = await _IRepository.GetPermHireMasterSearch(request);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost("PermHireMasterApproveReject")]
        public async Task<IActionResult> PermHireMasterApproveReject([FromBody] PermHireMasterDto request)
        {
            var res = await this._IRepository.PermHireMasterApproveReject(request);
            return Ok(res);
        }

    }
}

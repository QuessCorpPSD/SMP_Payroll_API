using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.AMSCommon;
using QPay.BAL.IRepository.Customer;
using QPay.IRepository.iRepository.Common;

namespace QPay.API.Controller.AMSCommon
{
    [Route("api/[controller]")]
    [ApiController]
    public class AMSCommonController : ControllerBase
    {
        private readonly IAMSCommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public AMSCommonController(
             IAMSCommonRepository iCommon, IConfiguration configuration)
        {
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetAllEntity")]
        public async Task<IActionResult> GetAllEntity()
        {
            var response = await _icommon.GetAllEntity();

            return Ok(response);
        }

        [HttpGet, Route("GetAllEmploymentType")]
        public async Task<IActionResult> GetAllEmploymentType()
        {
            var response = await _icommon.GetAllEmploymentType();

            return Ok(response);
        }

        [HttpGet, Route("GetAllState")]
        public async Task<IActionResult> GetAllState()
        {
            var response = await _icommon.GetAllState();

            return Ok(response);
        }

        [HttpGet, Route("GetAllCompanyByEntity/{entityId}")]
        public async Task<IActionResult> GetAllCompanyByEntity(int entityId)
        {
            var response = await _icommon.GetAllCompanyByEntity(entityId);

            return Ok(response);
        }

        [HttpGet, Route("GetAllDesignationbyCompany/{companyId}")]
        public async Task<IActionResult> GetAllDesignationbyCompany(int companyId)
        {
            var response = await _icommon.GetAllDesignationbyCompany(companyId);

            return Ok(response);
        }

        [HttpGet, Route("GetAllBandbyCompany/{companyId}")]
        public async Task<IActionResult> GetAllBandbyCompany(int companyId)
        {
            var response = await _icommon.GetAllBandbyCompany(companyId);

            return Ok(response);
        }

        [HttpGet, Route("GetAllCitybyState/{stateId}")]
        public async Task<IActionResult> GetAllCitybyState(int stateId)
        {
            var response = await _icommon.GetAllCitybyState(stateId);

            return Ok(response);
        }
    }
}

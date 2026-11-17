using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.Common;
using QPay.BAL.Repository.Common;
using QPay.IRepository.iRepository.Common;
using System.Data;

namespace QPay.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]
    public class CommonController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IProcessCategoryRepository _processCategoryRepository;
        private readonly IAccesstypeRepository _accesstypeRepository;
        private readonly IFinancialYearRepository _financialYearRepository;
        private readonly ICommonRepository _icompanyCode;

        public CommonController(
            IRoleRepository roleRepository,
            IProcessCategoryRepository processCategoryRepository,
            IAccesstypeRepository accesstypeRepository,
            IFinancialYearRepository financialYearRepository,
            ICommonRepository iCompanyCode)
        {
            _roleRepository = roleRepository;
            _processCategoryRepository = processCategoryRepository;
            _accesstypeRepository = accesstypeRepository;
            _financialYearRepository = financialYearRepository;
             _icompanyCode = iCompanyCode;
        }

        [HttpGet]
        [Route("GetAllActiveRole")]
        public async Task<IActionResult> GetAllActiveRole()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();

            return Ok(await _roleRepository.GetAllActiveRole(schema));
        }

        [HttpGet]
        [Route("GetAllProcessCategory")]
        public async Task<IActionResult> GetAllProcessCategory()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();

            return Ok(await _processCategoryRepository.GetAllProcessCategory(schema));
        }

        [HttpGet]
        [Route("GetAccessType")]
        public async Task<IActionResult> GetAccessType()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();

            return Ok(await _accesstypeRepository.GetAllAccessType(schema));
        }

        [HttpGet]
        [Route("GetFinancialYear")]
        public async Task<IActionResult> GetFinancialYear()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();

            return Ok(await _financialYearRepository.GetFinancialYears(schema));
        }

        [HttpGet, Route("GetAllCompanyCode/{userId}")]
        public async Task<IActionResult> GetAllCompanyCode(int userId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _icompanyCode.GetallCompanyCodes(userId, schema);

            return Ok(response);
        }
        [HttpGet, Route("GetAllPayperiod/{companyId}")]
        public async Task<IActionResult> GetAllPayperiod(int companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _icompanyCode.GetAllPayperiod(companyId, schema);

            return Ok(response);
        }
        [HttpGet, Route("GetCurrentPayperiod/{companyId}")]
        public IActionResult GetCurrentPayperiod(int companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = _icompanyCode.GetCurrentPayperiod(companyId, schema);

            return Ok(response);
        }
        [HttpGet, Route("GetMapNamebyCompany/{companyId}")]
        public async Task<IActionResult> GetMapNamebyCompany(int companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _icompanyCode.GetMapNamebyCompany(companyId, schema);

            return Ok(response);
        }
        [HttpGet, Route("GetAllInputType")]
        public async Task<IActionResult> GetAllInputType()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var result = await _icompanyCode.GetAllInputType(schema);

            return Ok(result);
        }
        [HttpPost, Route("GetLotwisePSDStatus")]
        public async Task<IActionResult> GetLotwisePSDStatus([FromForm] int companyId, [FromForm] int payPeriodId, [FromForm] int lotNumber, [FromForm] string Payroll_Input_Type)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _icompanyCode.GetLotwisePSDStatus(companyId, payPeriodId, lotNumber, Payroll_Input_Type, schema);
            return Ok(response);
        }

        [HttpGet, Route("GetSitesByCompanyId/{companyId}")]
        public async Task<IActionResult> GetSitesByCompanyId(int companyId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _icompanyCode.GetSitesByCompanyId(companyId, schema);

            return Ok(response);
        }

        [HttpGet, Route("GetCityByCompanyCode/{CompanyCode}/{Group_Id}")]
        public async Task<IActionResult> GetCityByCompanyCode(string CompanyCode, int Group_Id)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _icompanyCode.GetCityByCompanyCode(CompanyCode, Group_Id, schema);

            return Ok(response);
        }

        [HttpGet, Route("GetPayPeriod")]
        public async Task<IActionResult> GetPayPeriod()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _icompanyCode.GetPayPeriod(schema);

            return Ok(response);
        }

        [HttpGet, Route("GetPaycodes")]
        public async Task<IActionResult> GetPaycodes()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _icompanyCode.GetPaycodes(schema);

            return Ok(response);
        }

        [HttpGet, Route("GetAllState")]
        public async Task<IActionResult> GetAllState()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var state = await this._icompanyCode.GetAllState(schema);
            return Ok(state);
        }


        [HttpGet, Route("GetMultiCommercialPaycodes")]
        public async Task<IActionResult> GetMultiCommercialPaycodes()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _icompanyCode.GetMultiCommercialPaycodes(schema);

            return Ok(response);
        }

        [HttpGet, Route("GetSalaryReleasePayperiod/{companyId}/{Flag}")]
        public async Task<IActionResult> GetSalaryReleasePayperiod(int companyId, string Flag)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _icompanyCode.GetSalaryReleasePayperiod(companyId, Flag,schema);

            return Ok(response);
        }

        [HttpGet, Route("GetProfomaCompany/{userId}")]
        public async Task<IActionResult> GetProfomaCompany(int userId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _icompanyCode.GetProfomaCompany(userId, schema);

            return Ok(response);
        }
    }
}

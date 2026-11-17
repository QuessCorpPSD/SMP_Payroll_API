using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Master;

namespace QPay_Web_API.Controller.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorEmployeeController : ControllerBase
    {
        private readonly IVendorEmployeeRepository _ivendoremp;
        public VendorEmployeeController(
          IVendorEmployeeRepository ivendoremp)
        {
            this._ivendoremp = ivendoremp;
        }

        [HttpGet]
        [Route("GetVendorEmployee/{CompanyId}/{SiteId}/{EmployeeCode}/{EActive}")]
        public async Task<IActionResult> GetVendorEmployee(string CompanyId, string SiteId, string EmployeeCode,
            string EActive)
        {
            var ds = await _ivendoremp.GetVendorEmployee(CompanyId, SiteId, EmployeeCode, EActive);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet]
        [Route("GetVendorEmployeeTemplate")]
        public async Task<IActionResult> GetVendorEmployeeTemplate()
        {
            var ds = await _ivendoremp.GetVendorEmployeeTemplate();
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("ImportVendorEmployee")]
        public async Task<IActionResult> ImportVendorEmployee(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _ivendoremp.ImportVendorEmployee(file, User);
            return Ok(result);
        }
    }
}

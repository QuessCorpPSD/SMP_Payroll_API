using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.IR;

namespace QPay.API.Controller.IR
{
    [Route("api/[controller]")]
    [ApiController]
    public class IRController : ControllerBase
    {
        private readonly IIRRepository _IRepository;
        private readonly IConfiguration _configuration;
        public IRController(IIRRepository IRepository, IConfiguration configuration)
        {
            this._IRepository = IRepository;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetLastThreeYear")]
        public async Task<IActionResult> GetLastThreeYear()
        {
            var ds = await _IRepository.GetLastThreeYear();
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GetIREmployeeDetail/{Company_Id}")]
        public async Task<IActionResult> GetIREmployeeDetail(string? Company_Id)
        {
            var ds = await _IRepository.GetIREmployeeDetail(Company_Id);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GetIRDetail/{Employee_Id}/{Month}/{Year}")]
        public async Task<IActionResult> GetIRDetail(string? Employee_Id, string Month, string Year)
        {
            var ds = await _IRepository.GetIRDetail(Employee_Id, Month, Year);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GetIR8ADetail/{Employee_Id}/{Year}")]
        public async Task<IActionResult> GetIR8ADetail(string? Employee_Id, string? Year)
        {
            var ds = await _IRepository.GetIR8ADetail(Employee_Id, Year);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

    }
}

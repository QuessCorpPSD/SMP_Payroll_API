using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.PayrollInput;

namespace QPay_Web_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivationLwdController : ControllerBase
    {
        private readonly IActivationLwdRepository _iactivationlwd;
        public ActivationLwdController(
            IActivationLwdRepository iActivationlwd)
        {
            this._iactivationlwd = iActivationlwd;
        }

        [HttpGet]
        [Route("GetEmployeeActivationLwd/{companyCode}/{flag}")]
        public async Task<IActionResult> GetEmployeeActivationLwd(string companyCode, string flag)
        {
            var response = await _iactivationlwd.GetEmployeeActivationLwd(companyCode, flag);

            return Ok(response);
        }

        //[HttpPost]
        //[Route("UploadEmployeeActivation")]
        //public async Task<IActionResult> UploadEmployeeActivation(ActivationUpload Activationlist)
        //{
        //    var result = await _iactivationlwd.UploadEmployeeActivation(Activationlist);

        //    var response = new APIResponse<object>
        //    {
        //        statuscode = 200,
        //        message = "Success",
        //        data = result,
        //        error = ""
        //    };

        //    return Ok(response);
        //}


        [HttpPost]
        [Route("UploadEmployeeActivation")]
        public async Task<IActionResult> UploadEmployeeActivation(IFormFile file, [FromForm] string User,
            [FromForm] string COMPANY_CODE, [FromForm] string FLAG)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _iactivationlwd.UploadEmployeeActivation(file, User, COMPANY_CODE, FLAG);            
            return Ok(result);
        }

        [HttpPost]
        [Route("UploadEmployeeLWD")]
        public async Task<IActionResult> UploadEmployeeLWD(IFormFile file, [FromForm] string User,
           [FromForm] string COMPANY_CODE, [FromForm] string FLAG)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _iactivationlwd.UploadEmployeeLWD(file, User, COMPANY_CODE, FLAG);
            return Ok(result);
        }

    }
}

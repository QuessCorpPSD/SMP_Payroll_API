using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository.Admin;
using QPay.IRepository.iRepository.Common;
using QPay.UI.Models.Admin;

namespace QPay.API.Controller.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagement _iUser;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public UserManagementController(IUserManagement _iUser, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._iUser = _iUser;
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("getroletypes")]
        public async Task<IActionResult> GetRoleTypes()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _iUser.GetRoleTypes(schema);
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
        [Route("getreportingto")]
        public async Task<IActionResult> GetReportingTo()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _iUser.GetReportingTo(schema);
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
        [Route("getaccesstype")]
        public async Task<IActionResult> GetAccessType()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _iUser.GetAccessType(schema);
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
        [Route("Search")]
        public async Task<IActionResult> Search(UserManagement _params)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _iUser.Search(_params.UserId, _params.UserName, _params.RoleId, _params.IsCheck, schema);
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
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] adminmenurequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            string xml = XmlHelper2.SerializeObjectToXml(request);

            var response = await _iUser.Create(xml, request.createdBy, request.mode, request.UserDetails.Salt, schema);
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
        [Route("Unlock")]
        public async Task<IActionResult> Unlock([FromBody] adminmenurequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            string xml = XmlHelper2.SerializeObjectToXml(request);

            var response = await _iUser.Unlock(xml, request.createdBy, request.mode, request.UserDetails.Salt, schema);
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

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository.Admin;
using QPay.BAL.IRepository.Invoice;
using QPay.IRepository.iRepository.Common;
using QPay.UI.Models.Admin;

namespace QPay.API.Controller.Admin
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyPermissionController : ControllerBase
    {
        private readonly ICompanyPermissionRepository _iCompany;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public CompanyPermissionController(
            ICompanyPermissionRepository iCompany, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._iCompany = iCompany;
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetUsername")]
        public async Task<IActionResult> GetUsername()
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _iCompany.GetUsername(schema);
            var payload = ResponseWrapManager.ResponseWrapper(response, HttpContext);

            return Ok(payload);
        }

        [HttpGet("Search/{UserId}/{BusinessUnitId}/{CompanyPermissionId}")]
        public async Task<IActionResult> Search(int? UserId, int? BusinessUnitId, int? CompanyPermissionId)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _iCompany.Search(UserId, BusinessUnitId, CompanyPermissionId, schema);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }

        [HttpGet("ExportToExcel/{UserId}/{Businessunitnameid}")]
        public async Task<IActionResult> ExportToExcel(int? UserId, int? Businessunitnameid)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var ds = await _iCompany.ExportToExcel(UserId, Businessunitnameid, schema);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }

        [HttpPost]
        [Route("LoadCompany")]
        public async Task<IActionResult> LoadCompany(LoadCompany _params)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _iCompany.LoadCompany(_params.BusinessUnitNameId, _params.BusinessZonenName, schema);
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


        //[HttpPost]
        //[Route("CreateCompanyPermission")]
        //public async Task<IActionResult> CreateCompanyPermission(CompanyPermission request)
        //{
        //    var result = await _iCompany.CreateCompanyPermission(request);
        //    return Ok(result);
        //}

        [HttpPost]
        [Route("CreateUpdateDelete")]
        public async Task<IActionResult> CreateUpdateDelete([FromBody] CompanyPermissionRequest request)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            string xml = XmlHelper2.SerializeObjectToXml(request);

            var response = await _iCompany.CreateUpdateDelete(xml, request.createdBy, request.mode, schema);
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
        [Route("Editdetails")]
        public async Task<IActionResult> Editdetails(EditPermissiondetails _params)
        {
            var schema = Request.Headers["X-Schema"].FirstOrDefault();
            var response = await _iCompany.Editdetails(_params.Userid, _params.Businessunitnameid, _params.CompanyPermissionId, schema);
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

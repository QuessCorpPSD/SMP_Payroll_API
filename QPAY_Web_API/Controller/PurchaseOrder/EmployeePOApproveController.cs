using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.PurchaseOrder;
using QPay.UI_Domain.Models.PurchaseOrder;
using static QPay.UI_Domain.Models.PurchaseOrder.EmpPO;

namespace QPay_Web_API.Controller.Master.PurchaseOrder
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePOApproveController : ControllerBase
    {
        private readonly IEmployeePOApproveRepository _employeepoapproveRepository;
        private readonly IConfiguration _configuration;
        public EmployeePOApproveController(IConfiguration configuration, IEmployeePOApproveRepository employeepoapproveRepository)
        {
            _employeepoapproveRepository = employeepoapproveRepository;
            _configuration = configuration;
        }

        [HttpPost, Route("EmployeePOApproveSearch")]
        public IActionResult EmployeePOApproveSearch(EmployeePOApproveSearchRequest request)
        {
            var search = _employeepoapproveRepository.EmployeePOApproveSearch(request.companyId, request.siteId,
                request.poNumber, request.pricingType, request.employeeId, request.status);

            var payload = ResponseWrapManager.ResponseWrapper(search, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("EmployeePOApproveReject")]
        public IActionResult EmployeePOApproveReject(EmployeePOApproveRejectRequest request)
        {
            var res = _employeepoapproveRepository.EmployeePOApproveReject(request.employeePOApproveReject,
                request.CreatedBy);
            return Ok(res);
        }

        [HttpPost, Route("EmployeePOExporttoexcel")]
        public IActionResult EmployeePOExporttoexcel(EmployeePOApproveSearchRequest request)
        {
            var search = _employeepoapproveRepository.EmployeePOExporttoexcel(request.companyId, request.siteId,
                request.poNumber, request.pricingType, request.employeeId, request.status);

            var payload = ResponseWrapManager.ResponseWrapper(search, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("GetPoEmpDuration")]
        public IActionResult GetPoEmpDuration(EmployeePODurationRequest request)
        {
            var duration = _employeepoapproveRepository.GetPoEmpDuration(request);
            return Ok(duration);
        }

        [HttpPost, Route("GetPoEmpCalculation")]
        public IActionResult GetPoEmpCalculation(EmployeePOCalculationRequest request)
        {
            var calc = _employeepoapproveRepository.GetPoEmpCalculation(request);
            return Ok(calc);
        }

        [HttpPost, Route("GetPoEmpValue")]
        public IActionResult GetPoEmpValue(EmployeePOValueRequest request)
        {
            var calc = _employeepoapproveRepository.GetPoEmpValue(request);
            return Ok(calc);
        }


    }
}

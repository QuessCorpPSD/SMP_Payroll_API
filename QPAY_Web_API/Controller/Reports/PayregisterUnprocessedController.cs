using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.Master;
using QPay.BAL.IRepository.PurchaseOrder;
using QPay.BAL.IRepository.Reports;
using QPay.UI.Models;
using QPay.UI_Domain.Models.Master;
using QPay.UI_Domain.Models.PurchaseOrder;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayregisterUnprocessedController : ControllerBase
    {
        private readonly IPayregisterUnprocessedRepository _ipayregisterUnprocessedRepository;
        private readonly IPayRegisterRepository _registerRepository;
        private readonly IConfiguration _configuration;

        public PayregisterUnprocessedController(
         IPayregisterUnprocessedRepository payregisterUnprocessedRepository, IPayRegisterRepository registerRepository)
        {
            this._ipayregisterUnprocessedRepository = payregisterUnprocessedRepository;
            this._registerRepository = registerRepository;
        }


        [HttpGet]
        [Route("Exporttoexcel/{companyId}/{payperiodId}")]
        public async Task<IActionResult> Exporttoexcel(int companyId, int payperiodId)
        {
            var ds = await _ipayregisterUnprocessedRepository.Exporttoexcel(companyId, payperiodId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("payregisterDownload")]
        public async Task<IActionResult> payregisterDownload(payregisterInput input)
        {
            FileResponse fileResponse = new FileResponse();
            fileResponse = _registerRepository.PayRegisterDownload(input.companyId, input.payPeriodId, 0, input.payPeriod);

            return Ok(fileResponse);
        }
    }

    public class payregisterInput
    {
        public int companyId { get; set; }
        public int payPeriodId { get; set; }
        public string payPeriod { get; set; }
    }
}

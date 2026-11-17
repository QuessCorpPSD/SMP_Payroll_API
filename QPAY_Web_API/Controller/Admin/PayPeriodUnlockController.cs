using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Admin;
using QPay.BAL.IRepository.GlobalMaster;
using static QPay.UI.Admin.PayPeriodUnlock;
using static QPay.UI.Models.GlobalMaster.LWFClass;

namespace QPay.API.Controller.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPeriodUnlockController : ControllerBase
    {
        private readonly IPayPeriodUnlockRepository _IRepository;
        public PayPeriodUnlockController(IPayPeriodUnlockRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search()
        {
            var ds = await _IRepository.Search();

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("Unlock")]
        public async Task<IActionResult> Unlock(UnlockRequest request)
        {
            var ds = await _IRepository.Unlock(request);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.BankNonInvoice;

namespace QPay.API.Controller.BankNonInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartialHoldReleaseController : ControllerBase
    {
        private readonly IPartialHoldReleaseRepository _ihold;

        public PartialHoldReleaseController(IPartialHoldReleaseRepository ihold)
        {
            _ihold = ihold;
        }

        [HttpPost]
        [Route("PartialHold")]
        public async Task<IActionResult> PartialHold([FromForm] IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _ihold.PartialHold(file, User);

            return Ok(result);
        }

        [HttpPost]
        [Route("PartialHoldRelease")]
        public async Task<IActionResult> PartialHoldRelease([FromForm] IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _ihold.PartialHold(file, User);

            return Ok(result);
        }
    }
}

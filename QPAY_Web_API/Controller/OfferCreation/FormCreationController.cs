using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.GlobalMaster;

namespace QPay.API.Controller.OfferCreation
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormCreationController : ControllerBase
    {
        private readonly IFormCreationRepository _IRepository;
        public FormCreationController(IFormCreationRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet]
        [Route("Search/{FormId}/{ClientId}/{EntityId}/{SiteId}")]
        public async Task<IActionResult> Search(int? FormId, int? ClientId, int? EntityId, int? SiteId)
        {
            var response = await _IRepository.Search(FormId, ClientId, EntityId, SiteId);
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }
    }
}

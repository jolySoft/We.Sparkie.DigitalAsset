using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace We.Sparkie.DigitalAsset.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeartBeatController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
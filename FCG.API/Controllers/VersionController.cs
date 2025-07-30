using FCG.API.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FCG.API.Controllers
{

    [Route("[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("29-05-2025");
        }

    }
}

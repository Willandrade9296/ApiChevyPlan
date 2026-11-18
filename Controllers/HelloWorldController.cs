using Microsoft.AspNetCore.Mvc;

namespace ApiDatalife.Controllers
{

    [Route("api/[Controller]")]
    [ApiController]
    public class HelloWorldController:ControllerBase
    {


        [HttpGet]

        public IActionResult Get()
        {

            return Ok("Hello Wolrd");
        }




    }
}

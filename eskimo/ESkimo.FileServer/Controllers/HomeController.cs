using Microsoft.AspNetCore.Mvc;

namespace ESkimo.FileServer.Controllers
{

    [Route("")]
    public class HomeController : Controller
    {


        public HomeController()
        {

        }


        [HttpGet, Route("")]
        public IActionResult Home()
        {

            return Ok("FileServer");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SP.Scheduler.Models;

namespace SP.Scheduler.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet, Route("")]
        public IActionResult Home()
        {
            return Ok("Scheduler");
        }
    }
}

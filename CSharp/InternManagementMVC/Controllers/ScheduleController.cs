using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InternManagementMVC.Controllers
{
    public class ScheduleController : Controller
    {
        [Route("Schedule/{username}")]
        public IActionResult ScheduleByUsername(string username)
        {
            ViewBag.username = username;
            return View();
        }
    }
}
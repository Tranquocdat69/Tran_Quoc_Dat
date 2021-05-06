using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IternManagemenrMVC.Controllers
{
    public class StudentController : Controller
    {
        [Route("/students")]
        [Route("/Student/Index")]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using InternManagementMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternManagementMVC.Controllers
{
    public class TStudentController : Controller
    {
        private HttpClient namedClient;

        public TStudentController(IHttpClientFactory factory)
        {
            namedClient = factory.CreateClient("IternAPI");
        }
        public IActionResult Index()
        {
            IEnumerable<TStudent> students = null;
            var responseTask = namedClient.GetAsync("tstudent/students");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<TStudent>>();
                readTask.Wait();

                students = readTask.Result;
            }
            else
            {
                students = Enumerable.Empty<TStudent>();
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(students);
        }
    }
}
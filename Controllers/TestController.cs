using Microsoft.AspNetCore.Mvc;
using Physiosoft.Models;

namespace Physiosoft.Controllers
{
    public class TestController : Controller
    {
        private readonly ILogger<TestController> logger;

        public TestController(ILogger<TestController> logger)
        {
            this.logger = logger;
        }

        public string Welcome()
        {
            return "This is the welcome method";
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }

    }
}

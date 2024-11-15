using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{
    public class Admin2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

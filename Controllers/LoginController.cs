using Microsoft.AspNetCore.Mvc;

namespace Presupuesto.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}

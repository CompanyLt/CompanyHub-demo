using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Darbu_records.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class AccessController : Controller
    {
        public IActionResult Index()
        {
           
            return View();
        }


        public async Task<IActionResult> logout()
        {
            HttpContext.Session.Clear();


            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home",new {area=""});
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Darbu_records.Areas.Member.Controllers
{
    [Area("Member")]
    public class DashboardController:Controller
    {


        public IActionResult Index()
        {

            Console.WriteLine("sdsdsUser");
            return View();
        }







    }
}

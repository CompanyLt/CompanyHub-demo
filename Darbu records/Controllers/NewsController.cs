using Microsoft.AspNetCore.Mvc;

namespace Darbu_records.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult post_detail(int id)
        {
            Console.WriteLine(id);
            ViewBag.postid = id;

           


            return View();
        }
    }
}

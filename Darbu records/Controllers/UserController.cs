using Darbu_records.DAL;
using Darbu_records.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Darbu_records.Controllers
{
    public class UserController : Controller
    {

        private readonly WorkerLoader _workerLoader;


        public UserController(WorkerLoader workerLoader) 
        {
            _workerLoader = workerLoader;
        }
        public async Task<IActionResult> Index()
        {
          //  Console.WriteLine("worker detail");
            //GET worker
            IWorker worker = await _workerLoader.GetAsync();


            return View(worker);
        }


        public async Task<IActionResult> WorkerById(int id)
        {
            //GET worker
            IWorker worker = await _workerLoader.GetByIdAsync(id);


            return View("Index",worker);
        }
    }
}

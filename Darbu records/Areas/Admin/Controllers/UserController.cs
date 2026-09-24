using Darbu_records.DAL;
using Darbu_records.Interfaces;
using Darbu_records.Models;
using Darbu_records.Models.User;
using Darbu_records.Query.User;
using Darbu_records.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Darbu_records.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {

        WorkerViewService _workerViewService;
        WorkerAddService _workerAddService;
        IUserQueryBuilder _workerQueryBuilder;
        IUserQueryBuilder _createWorkerQueryBuilder;
        DepartmentLoader _departmentLoader;
        WorkerLoader _workerLoader;
       

        public UserController(WorkerViewService workerViewService, 
            [FromKeyedServices("WorkerViewQueryBuilder")]IUserQueryBuilder workerQueryBuilder,
            [FromKeyedServices("CreateWorkerQueryBuilder")] IUserQueryBuilder createWorkerQueryBuilder,
            DepartmentLoader departmentLoader,
            WorkerLoader workerLoader,
            WorkerAddService workerAddService
            ) { 
        
        _workerViewService = workerViewService;
        _workerQueryBuilder = workerQueryBuilder;
        _departmentLoader = departmentLoader;
        _workerLoader = workerLoader;
        _createWorkerQueryBuilder = createWorkerQueryBuilder;
        _workerAddService = workerAddService;
        
        }

        public async Task <IActionResult> Index()
        {


            UserTaskForm form = new UserTaskForm()
            {
                departmentId = Convert.ToInt32(User.FindFirstValue("DepartmentId"))
            };

            _workerQueryBuilder.SetQuery();

           
          await  _workerViewService.taskExecution(form, _workerQueryBuilder);






            return View(form.workers);
        }


        public async Task<IActionResult> Details(int id)
        {


           

          IWorker worker = await _workerLoader.GetByIdAsync(id);





            return View(worker);
        }

        [HttpGet]
        public async Task <IActionResult> Create()
        {
            CreateUserTaskForm form = new CreateUserTaskForm();
            await _departmentLoader.LoadShareDesk(form.departmentCollection);



            return View(form);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateUserTaskForm form)
        {
            // Console.WriteLine("veikia");
            if(!ModelState.IsValid)
            {
                await _departmentLoader.LoadShareDesk(form.departmentCollection);
                return View(form);
            }
            //Console.WriteLine(form.departmentId);
            //Console.WriteLine(form.name);
            //Console.WriteLine(form.role_id);

             _createWorkerQueryBuilder.SetQuery();
           // Console.Write(_createWorkerQueryBuilder.GetMainQuery());
           // Console.Write(_createWorkerQueryBuilder.GetWorkerFilesQuery());

            await _workerAddService.taskExecution(form, _createWorkerQueryBuilder);

            return RedirectToAction("Index");
        }






        public async Task<IActionResult> Edit(int id)
		{




			IWorker worker = await _workerLoader.GetByIdAsync(id);

          //  Console.WriteLine($" vardas { worker.name} id {id}");



			return View(worker);
		}




		public async Task<IActionResult> ViewWorker(int id)
        {


            UserTaskForm form = new UserTaskForm()
            {
                departmentId = id
            };

            _workerQueryBuilder.SetQuery();
            await _workerViewService.taskExecution(form, _workerQueryBuilder);






            return View("Index",form.workers);
        }










        public async Task<IActionResult> WorkerByDepartment()
        {


          var departmentCollection =   await  _departmentLoader.GetAsync();




            return View(departmentCollection.departmentCollection);
        }




    }
}

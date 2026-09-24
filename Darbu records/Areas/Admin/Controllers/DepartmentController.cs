using Darbu_records.DAL;
using Darbu_records.DepartmentManagement;
using Darbu_records.Enums.Worker;
using Darbu_records.Models.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Darbu_records.Areas.Admin.Controllers
{

    [Area(nameof(Roles.Admin))]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class DepartmentController : Controller
    {

        DepartmentLoader _departmentLoader;
        NavigationLoader _navigationLoader;
        DepartmentAddService _departmentAddService;


        public DepartmentController(DepartmentLoader departmentLoader, NavigationLoader navigationLoader,DepartmentAddService departmentAddService)
        {
            _departmentLoader = departmentLoader;
            _navigationLoader = navigationLoader;
            _departmentAddService = departmentAddService;
        }
        public async Task <IActionResult> Index()
        {

            var departments = await _departmentLoader.GetAsync();

           

          

            return View(departments.departmentCollection);
        }





        public async Task<IActionResult> Create()
        {

            DepartmentTaskForm form = new DepartmentTaskForm();

            form.navigationActionCollection = await _navigationLoader.GetNavigationActionAsync();


           
            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> Create (DepartmentTaskForm form)
        {

          //  form.navigationActionCollection = form.navigationActionCollection.Where(n=>n.isSelected).ToList();
            form.navigationActionCollection.RemoveAll(n => !n.isSelected);

            form.created_by=Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _departmentAddService.taskExecution(form);




           
            return RedirectToAction(nameof(Index));
        }






          
        
    }
}

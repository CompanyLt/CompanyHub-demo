using Darbu_records.CategoryManagement;
using Darbu_records.DAL;
using Darbu_records.Enums.Worker;
using Darbu_records.GroupManagement;
using Darbu_records.Models;
using Darbu_records.Models.Categories;
using Darbu_records.Models.Group;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Darbu_records.Areas.Admin.Controllers
{
    [Area(nameof(Roles.Admin))]
    [Authorize(Roles=nameof(Roles.Admin))]
    public class CategoryController : Controller
    {


        NavigationLoader _navigationLoader { get; set; }
        CategoryToGroupService _categoryToGroupService;
        private readonly CategoryAddService _categoryAddService;
       

        public CategoryController(NavigationLoader navigationLoader,CategoryToGroupService categoryToGroupService,CategoryAddService categoryAddService)
        {
            _navigationLoader = navigationLoader;
            _categoryToGroupService = categoryToGroupService;
            _categoryAddService = categoryAddService;


        }



        public async Task <IActionResult> Index()
        {

            List<Category> categories = new();
            int department = Convert.ToInt32(User.FindFirstValue("DepartmentId"));
            await _navigationLoader.LoadCategoryAsync(categories, department);





            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryToGroup(int id)
        {

            List<Category> categories = await _navigationLoader.LoadCategoriesAsync(id);



            if (categories == null || !categories.Any())
                return RedirectToAction("Index", "Group");

            categories.ForEach(c => c.isSelected = (id == c.GroupId));

         //Console.WriteLine("START");
         //           foreach (var n in  categories)
         //           {
         //               Console.WriteLine(n.Name);
         //           }
         //           Console.WriteLine("END");
          

            CategoryToGroupForm form = new CategoryToGroupForm()
            {
                id = id,
                categories = categories
               
            };



            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryToGroup(CategoryToGroupForm form)
        {
            

            form.department_id = Convert.ToInt32(User.FindFirstValue("DepartmentId"));
            form.department_access = Convert.ToInt32(User.FindFirstValue("DepartmentId"));
            
            
            form.categories = form.categories
            .Where(c => c.isSelected)
             .ToList();

          
            await _categoryToGroupService.taskExecution(form);
           

           


            return RedirectToAction("Index","Group");
        }



        [HttpGet]
        public IActionResult Create()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryForm form)
        {
            form.created_by = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            form.department_id = Convert.ToInt32(User.FindFirstValue("DepartmentId"));
            form.department_access = Convert.ToInt32(User.FindFirstValue("DepartmentId"));
            var id = await _categoryAddService.taskExecution(form);
            //VEIKSMAI



            return RedirectToAction("GroupToCategory", "Group", new { id = id });
        }





    }
}

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
    [Authorize(Roles = nameof(Roles.Admin))]
    public class GroupController : Controller
    {
        NavigationLoader _navigationLoader {  get; set; }

        private readonly GroupAddService _groupAddService;

        private readonly GroupToCategoryService _groupToCategoryService;

        public GroupController(NavigationLoader navigationLoader,
            GroupAddService groupAddService,
            GroupToCategoryService groupToCategoryService) {
        _navigationLoader = navigationLoader;
        _groupAddService = groupAddService;
            _groupToCategoryService = groupToCategoryService;
        
        }
        public async Task <IActionResult> Index()
        {

         List<CategoryGroup> groups= new();
            int department = Convert.ToInt32(User.FindFirstValue("DepartmentId"));
            await _navigationLoader.LoadGroupsAsync(groups, department);


            


            return View(groups);
        }



        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupForm form)
        {
            form.created_by = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            form.department_id = Convert.ToInt32(User.FindFirstValue("DepartmentId"));
            form.department_access = Convert.ToInt32(User.FindFirstValue("DepartmentId"));
            var id = await _groupAddService.taskExecution(form);
            //VEIKSMAI



            return RedirectToAction("CategoryToGroup", "Category", new {id=id});
        }







        [HttpGet]
        public async Task<IActionResult> GroupToCategory(int id)
        {
           
            List<CategoryGroup> groups = await _navigationLoader.LoadGroupsAsync(id);

            groups.ForEach(c => c.isSelected = (id == c.CategoryId));

           
            GroupToCategoryForm form = new GroupToCategoryForm()
            {
                id = id,
                groups = groups,
                SelectedGroupId = groups.FirstOrDefault(c => c.isSelected)?.Id ?? 0
            };
           


            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> GroupToCategory(GroupToCategoryForm form)
        {

            form.department_id = Convert.ToInt32(User.FindFirstValue("DepartmentId"));
            form.department_access = Convert.ToInt32(User.FindFirstValue("DepartmentId"));
            await _groupToCategoryService.taskExecution(form);
            //form.department_id = Convert.ToInt32(User.FindFirstValue("DepartmentId"));
            //form.department_access = Convert.ToInt32(User.FindFirstValue("DepartmentId"));

            //form.categories = form.categories
            //.Where(c => c.isSelected)
            //.ToList();


            //await _categoryToGroupService.taskExecution(form);





            return RedirectToAction("Index", "Category");
        }




    }
}

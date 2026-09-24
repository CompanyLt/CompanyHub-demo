using Darbu_records.DAL;
using Darbu_records.Models.Department;
using Darbu_records.Models.ShareDesk;
using Darbu_records.SharedeskManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Darbu_records.Controllers
{
    [Authorize]
    public class ShareDeskController : Controller
    {
        private readonly ShareDeskRepository _sharedeskRepository;


        DepartmentLoader _departmentLoader;
        NavigationLoader _navigationLoader;

         public ShareDeskController(ShareDeskRepository shareDeskRepository,DepartmentLoader departmentLoader,NavigationLoader navigationLoader)
                {

                _sharedeskRepository = shareDeskRepository;
                _departmentLoader = departmentLoader;
            _navigationLoader = navigationLoader;



                }





        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }


        [HttpPost]       
        public async Task<IActionResult> DeleteSharedTopic(int id)
        {

            ShareForm shareForm = new ShareForm()
            {

                Id = id,
                Department_id = Convert.ToInt32(User.FindFirstValue("DepartmentId"))

            };
            await _sharedeskRepository.DeleteSharedTopicAsync(shareForm);






            return RedirectToAction("Edit");
        }




        public async Task<IActionResult> InstructionGroupWithDepartment(int id)
        {

                
            ///TURIU PAIMTI VISAS KATEGORIJAS PAGAL GRUPID
            var items= await _departmentLoader.GetAsync();


            ShareForm shareForm = new ShareForm()
            {
                Departments = items.departmentCollection,
                Id = id,              
             };
           

            
            return View(shareForm);
        }




        [HttpPost]
        public async Task<IActionResult> ConfirmInstructionGroupShare(ShareForm shareForm)
        {
           
            int department_id = Convert.ToInt32(User.FindFirstValue("Departmentid"));
            var group = await _navigationLoader.GetGroupAsync(shareForm.Id);
            var department = await _departmentLoader.GetDepartmentByIdAsync(department_id);
            // Console.WriteLine($"Grupes id{shareForm.Id}");
            shareForm.Config_id = 2;
            shareForm.Title = group.Name;
            shareForm.Created_by = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            shareForm.Department_id = department_id;
            shareForm.Department_name = department.Name;
           
          //  Console.WriteLine($"share grupe title: {shareForm.Title} department_name: {shareForm.Department_name} description {shareForm.Description} share_id {shareForm.Id} access {shareForm.Department_access} depart_id{shareForm.Department_id}");          
            await _sharedeskRepository.AddShareGroupAsync(shareForm);








            return RedirectToAction("Index", "Home");
        }



        public async Task<IActionResult> ConfirmInstructionCategoryShare(ShareForm shareForm)
        {

            int department_id = Convert.ToInt32(User.FindFirstValue("Departmentid"));
            var department =await  _departmentLoader.GetDepartmentByIdAsync(department_id);
            
            var category = await _navigationLoader.GetCategoryAsync(shareForm.Id);


           // Console.WriteLine($"vardas {category.Name} id {shareForm.Id} dep name {department.Name}");
            shareForm.Config_id = 3;
            shareForm.Title = category.Name;
            shareForm.Created_by = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            shareForm.Department_id = department_id;
            shareForm.Department_name = department.Name;
           
           // Console.WriteLine($"share categoryja title: {shareForm.Title} department_name: {shareForm.Department_name} description {shareForm.Description} share_id {shareForm.Id} access {shareForm.Department_access} depart_id{shareForm.Department_id}");
            await _sharedeskRepository.AddShareCategoryAsync(shareForm);
            //  Console.WriteLine($"share kategorija {id} access {department_access}");



            return View();
        }












        public async Task<IActionResult> InstructionCategoryWithDepartment(int id)
        {
          
            var items = await _departmentLoader.GetAsync();

            ShareForm shareForm = new ShareForm()
            {
                Departments = items.departmentCollection,
                Id = id,
            };
           

            return View(shareForm);
        }













    }
}

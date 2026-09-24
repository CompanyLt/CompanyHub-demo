using Darbu_records.DAL;

using Darbu_records.Formos;
using Darbu_records.IncidentManagement;
using Darbu_records.InstructionManagement;
using Darbu_records.Models;
using Darbu_records.Models.Topic;
using Darbu_records.Query.Topic;
using Darbu_records.TopicManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Darbu_records.Controllers
{
    public class FilesController : Controller
    {

        NavigationLoader _navigationLoader;      
        TopicViewService _topicViewService;
        TopicReviewService _topicReviewService;
        ITopicQueryBuilder _fileViewqueryBuilder;
        ITopicQueryBuilder _fileReviewQueryBuilder;

        public FilesController(NavigationLoader navigationLoader,          
            TopicViewService topicViewService,
            [FromKeyedServices("FileViewQueryBuilder")] ITopicQueryBuilder topicQueryBuilder,
             [FromKeyedServices("FileReviewQueryBuilder")] ITopicQueryBuilder topicReviewQueryBuilder,
            TopicReviewService topicReviewService)
        {
            _navigationLoader = navigationLoader;
           
            _topicViewService = topicViewService;
            _fileViewqueryBuilder = topicQueryBuilder;
            _fileReviewQueryBuilder = topicReviewQueryBuilder;
            _topicReviewService = topicReviewService;
        }
        public IActionResult Index()
        {
            return View();
        }




        public async Task <IActionResult> GroupPrivateView(int department_access)
        {
           
            var groups = await _navigationLoader.GetAsync();

            var filtered = groups.groups.Where(c => c.DepartmentAccess == department_access).ToList();


           

            return View(filtered);
        }





        public async Task<IActionResult> GroupFilesView(int department_access)
        {

            //List<CategoryGroup> groups = new List<CategoryGroup>();
            //await _navigationLoader.LoadGroupsAsync(groups,department_access);
            var groups = await _navigationLoader.GetAsync();

            var filtered = groups.groups.Where(c => c.DepartmentAccess == department_access).ToList();

            return View(filtered);
        }


        public async Task<IActionResult> CategoriesFilesView(int id,int department_access)
        {

          //  Console.WriteLine($"{ categoryId} { Parameter}");
           // List<Category> categories = new List<Category>();

          //  await _navigationLoader.LoadCategoryAsync(categories, department_access);

            var categories = await _navigationLoader.GetAsync();
            var filtered = categories.categories.Where(c => c.GroupId == id && c.DepartmentAccess == department_access).ToList();

          //  var filtered = categories.Where(c => c.GroupId == categoryId).ToList();




            return View(filtered);
        }


        public async Task<IActionResult> FileView(int id,int department_access)
        {

             // Console.WriteLine(department_access);
             // Console.WriteLine(category);
            if (id == null) return View("Index");
            // _irasukiekis.kiekis(1);

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                //  SqlQuery query = new SqlQuery();
                //initialize information
                TaskForm2 forma = new TaskForm2()
                {
                    id = id,
                    department_access = department_access,
                    departmentId = Convert.ToInt32(User.FindFirstValue("DepartmentId"))


                };

                _fileViewqueryBuilder.SetQuery();
                //  await _fileViewService.taskExecution(forma);
                await _topicViewService.taskExecution(forma, _fileViewqueryBuilder);

              


                return View(forma.topicCollection);
            }
            List<TopicForm> forms = new List<TopicForm>();
            return View(forms);
        }

        public async Task<IActionResult> FileReview(int id, int department_access)
        {

          //  Console.WriteLine($"kategorija{id} access {department_access}");
            TaskForm2 taskForm = new TaskForm2()
            {
                id = id,
                department_access = department_access,
                departmentId = Convert.ToInt32(User.FindFirstValue("DepartmentId"))
            };

            //  await _instructionRepository.ReviewTopicAsync(taskForm);
            _fileReviewQueryBuilder.SetQuery();
            await _topicReviewService.taskExecution(taskForm, _fileReviewQueryBuilder);


            //ViewData["tempDirectory"] = "A:\\companyhubFiles\\instruction\\files";

            //foreach(var n in form.instructionForm.filescollection)
            //{
            //   // Console.WriteLine("ACTION");
            //   // Console.WriteLine(n.name);
            //}



            return View(taskForm.topicForm);
        }

    }
}

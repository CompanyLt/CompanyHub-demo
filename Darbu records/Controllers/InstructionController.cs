using Darbu_records.DAL;
using Darbu_records.Data;
using Darbu_records.Enums.Topics;
using Darbu_records.Formos;
using Darbu_records.IncidentManagement;
using Darbu_records.InstructionManagement;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Darbu_records.Models.Topic;
using Darbu_records.OldItems;
using Darbu_records.Query;
using Darbu_records.SearchManagement;
using Darbu_records.Singleton;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Darbu_records.Controllers
{
    [Authorize]
    public class InstructionController : Controller
    {


        INoteTaskService _instructionReviewIframeService;
        INoteTaskService _instructionAddService;
        INoteTaskService _instructionViewService;
        INoteTaskService _instructionUpdateService;
        WorkerLoader _workerLoader;
        NavigationLoader _navigationLoader;


        InstructionRepository _instructionRepository;





        private int vartotojas { set; get; }
        

        public InstructionController([FromKeyedServices("InstructionReviewService")]INoteTaskService instructionReviewIframeService,
            [FromKeyedServices("InstructionAddService")]INoteTaskService instructionAddService,
            [FromKeyedServices("InstructionViewService")] INoteTaskService instructionViewService,
            [FromKeyedServices("InstructionUpdateService")]INoteTaskService instructionUpdateService,
            WorkerLoader workerLoader,
            NavigationLoader navigationLoader,
            InstructionRepository instructionRepository
            )
        {
          _instructionReviewIframeService = instructionReviewIframeService;
            _instructionAddService = instructionAddService;
            _instructionViewService = instructionViewService;
            _instructionUpdateService = instructionUpdateService;
            _workerLoader = workerLoader;
            _navigationLoader = navigationLoader;
            _instructionRepository = instructionRepository;
        }

        private void vartotojas_set()
        {

            vartotojas = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }



        //kiekviena karta kai pasileidzia Index yra nunulinama _record listas
        //visas sarasas
        public IActionResult Index()
        {
            //var ipadress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            //    Console.WriteLine(ipadress);

            return View();
        }
       
        public async Task<IActionResult> InstructionView(int id, int department_access)
        {
           // Console.WriteLine($"kategorija{id} access {department_access}");
            if (id == null) return View("Index");          
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {

                // int id_app = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ;              
                TaskForm taskForm = new TaskForm()
                {
                    categoryId = id,
                    departmentId = Convert.ToInt32(User.FindFirstValue("DepartmentId")),
                    department_access = department_access
                };               
             await  _instructionViewService.taskExecution(taskForm);
                         
                return View(taskForm.instructionCollection);
            }

            return View();
        }

       

      
        [HttpPost]
        public IActionResult InstructionDelete(int id,int idCategory)
        {

            SqlQuery sqlQuery = new SqlQuery();

            Query_Action action = new Query_Action(sqlQuery.dbConn, sqlQuery.instruction_delete);
            action.Record_Delete(id);






            //trecias argumentas tai duomenys new{} anoniminis objektas
            return RedirectToAction("InstructionView", "Instruction", new { category = idCategory });
        }
           
        //instrukcijos realizacija
        public IActionResult InstructionUpdate(int id, string title, string description)
        {
           
            InstructionForm forma = new InstructionForm(title, description, id);

            return View(forma);

        }

        public IActionResult InstructionUpdateExecute(InstructionForm forma)
        {

            try
            {

                TaskForm taskForm = new TaskForm()
                {
                    instructionForm = forma
                };

                _instructionUpdateService.taskExecution(taskForm);              
            }
            catch (Exception e)
            {
                return View("Error");
            }




            return RedirectToAction("Index", "Home");
        }


      

        public async Task<IActionResult> InstructionReview(int id,int department_access)
        {

       
            TaskForm2 taskForm = new TaskForm2()
            {
                id = id,
                department_access = department_access,
                departmentId = Convert.ToInt32(User.FindFirstValue("DepartmentId"))
            };

            await _instructionRepository.ReviewTopicAsync(taskForm);



            //ViewData["tempDirectory"] = "A:\\companyhubFiles\\instruction\\files";

            //foreach(var n in form.instructionForm.filescollection)
            //{
            //   // Console.WriteLine("ACTION");
            //   // Console.WriteLine(n.name);
            //}



            return View(taskForm.topicForm);
        }


        //CIA SVARBIAUSIA NEDUBLIUOTI BINDINGO PVZ 1 argmunentas Files ir antras pvz klaseje Files tai gausis kolozija 
        [HttpPost]
        public async Task<IActionResult> InstructionAdd(TopicForm topicForm ,IEnumerable<IFormFile> formFiles)
        {




            Console.WriteLine(topicForm.categoryId);
            Console.WriteLine(topicForm.title);
            Console.WriteLine(topicForm.description);
           
            ModelState.Remove(key: "file");
            
            if (!ModelState.IsValid)
            {
                //Galima per nameof pasiekti metoda
                return View(nameof(instructionRegistration),topicForm);
            }

            TaskForm2 taskForm = new TaskForm2()
            {
                filesCollection = formFiles,
                topicForm = topicForm,
                workerId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                departmentId = Convert.ToInt32(User.FindFirstValue("DepartmentId"))


            };
           
           await _instructionRepository.AddTopicAsync(taskForm);
           // await _instructionAddService.taskExecution(taskForm);
           


            return RedirectToAction("Index", "Home");
        }

        public async Task <IActionResult> instructionRegistration()
        {
            var navigation = await _navigationLoader.GetAsync();
            var departmentId = Convert.ToInt32(User.FindFirstValue("DepartmentId"));

            var filtered = navigation.groups.Where(c => c.DepartmentAccess == departmentId);
            var filteredCategories = navigation.categories.Where(c => c.DepartmentAccess == departmentId);
            TopicForm form = new TopicForm();
            form.categoryGroup = filtered;
            form.categories = filteredCategories;
            return View(form);
        
        }


        //perduodam id ir patinka kieki
        public IActionResult Patinka_pridejimas(string pavadinimas, string aprasymas, int id, int patinka)
        {
           // Console.WriteLine($"instrkcijos iraso id {id.ToString()}");
            //cia galima perdaryti konstrukroiu
            SqlQuery query = new SqlQuery();
            Query_Action action = new Query_Action(query.dbConn, query.patinka_pridejimas, id);
            action.patinka_pridejimas(id, patinka);

            InstructionForm forma = new InstructionForm(pavadinimas, aprasymas, id, patinka += 1, "");


            return View("Instrukcijos_info", forma);
        }

        public IActionResult nav_bar()
        {

            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Rendernews()
        {



            return PartialView("news");
        }

       
        public async Task <IActionResult> CategoriesInstructionView(int id, int department_access)
        {
          
           // var categories = await _navigationLoader.GetAsync();
            List<Category> categoriesList = new List<Category>();
            int department = Convert.ToInt32(User.FindFirstValue("DepartmentId"));

            await _navigationLoader.LoadCategoriesAsyncPerziurai(categoriesList, department);

            
           
            // Console.WriteLine($"kategorija{id} access {department_access}");
            //  var filtered = _navigationLoader.Get().categories.Where(c => c.GroupId == categoryId).ToList();
            var filtered = categoriesList.Where(c => c.GroupId == id && c.DepartmentAccess == department_access).ToList();

            return View(filtered);
        }

       
        public async Task <IActionResult> GroupInstructionView(int department_access)
        {


           // var groups = await _navigationLoader.GetAsync();

           // var filtered = groups.groups.Where(c => c.DepartmentAccess == department_access).ToList();


            List<CategoryGroup> groups = new();
            int department = Convert.ToInt32(User.FindFirstValue("DepartmentId"));
            await _navigationLoader.LoadGroupsAsync(groups, department);



            // var group = _navigationLoader.Get();

            return View(groups);
        }








    }
}

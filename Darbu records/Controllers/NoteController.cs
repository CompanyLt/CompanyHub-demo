using Darbu_records.Data;
using Darbu_records.Formos;
using Darbu_records.SearchManagement;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Darbu_records.OldItems;
using Darbu_records.Query;
using Darbu_records.SearchManagement;
using Darbu_records.Singleton;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Darbu_records.IncidentManagement;
using Microsoft.Extensions.DependencyInjection;
using Darbu_records.DI;
using Microsoft.AspNetCore.Authorization;
using Darbu_records.DAL;
using Darbu_records.Models.Topic;

namespace Darbu_records.Controllers
{  [Authorize]
    public class NoteController : Controller
    {

       private readonly INoteTaskService _noteUpdateService;
       private readonly  INoteTaskService _noteDeleteService;    
       private readonly INoteTaskService _noteViewService;

       
        private readonly NavigationLoader _navigationLoader;


        NoteRepository _noteRepository;





       
      
        public NoteController([FromKeyedServices("NoteUpdateService")]INoteTaskService noteUpdateService,
            [FromKeyedServices("NoteDeleteService")] INoteTaskService noteDeleteService,
            [FromKeyedServices("NoteViewService")] INoteTaskService noteViewService,                  
            NavigationLoader navigationLoader,
            NoteRepository noteRepository
            )
        {
               _noteUpdateService = noteUpdateService;
               _noteDeleteService = noteDeleteService;           
               _noteViewService = noteViewService;                         
               _navigationLoader = navigationLoader;
               _noteRepository = noteRepository;
        }



  

        //kiekviena karta kai pasileidzia Index yra nunulinama _record listas
        //visas sarasas
        public IActionResult Index()
        {
            //var ipadress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            //    Console.WriteLine(ipadress);
            
            return View();
        }
        public async Task<IActionResult> NoteView(int category,int department_access)
        {
           // Console.WriteLine($"access{department_access}");
         
            if (category == null) return View("Index");
            // _irasukiekis.kiekis(1);
         
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
              //  SqlQuery query = new SqlQuery();
                //initialize information
                TaskForm forma = new TaskForm()
                {
                    categoryId = category,
                    department_access = department_access,
                    noteId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    departmentId=Convert.ToInt32(User.FindFirstValue("DepartmentId"))

                };


                await _noteViewService.taskExecution(forma);


               // IQueryService queryService = new IncidentQueryService(query.dbConn, query.qr, vartotojas);
               // INoteTaskService noteService = new IncidentViewService(queryService);
                //IncidentCenter incidentView = new IncidentCenter(noteService, queryService);



                //execute
               // await incidentView.taskExecute(forma);



                return View(forma.noteCollection);
            }

            return View();
        }
      

       

        [HttpPost]
        public async Task<IActionResult> NoteDelete(int id,int category_id,int department_access)
        {

           // Console.WriteLine(category_id);
            TaskForm form = new TaskForm()
            {
                noteId = id
            };
          //  SqlQuery sqlQuery = new SqlQuery();
          //  IQueryService queryService = new IncidentQueryService(sqlQuery.dbConn, sqlQuery.Delete_Record);
          //  INoteTaskService incidentDeleteService = new IncidentDeleteService(queryService);
         await _noteDeleteService.taskExecution(form);
         //  await incidentDeleteService.taskExecution(form);


          



            return RedirectToAction("NoteView", "Note", new {category=category_id,department_access=department_access});
        }

       
        //Iraso redagavimo realizacija-------
        //inicializacija
        public IActionResult NoteUpdate(IncidentForm incidentForm)
        {

          
            return View(incidentForm);

        }

        //ivykdymas
        public async Task<IActionResult> NoteUpdateExecute(IncidentForm forma)
        {
            TaskForm taskForm= new TaskForm()
            {
                incidentForm = forma,
                
            };

          //  SqlQuery query = new SqlQuery();
           // IQueryService queryService = new IncidentQueryService(query.dbConn, query.Edit_Record, taskForm.incidentForm.iraso_id);
           // INoteTaskService updateTaskService = new IncidentUpdateService(queryService);
           // IncidentCenter incidentUpdate = new IncidentCenter(updateTaskService);
          // await incidentUpdate.taskExecute(taskForm);
           
            await _noteUpdateService.taskExecution(taskForm);


            return RedirectToAction("Index", "Home");
        }
      
        //[HttpPost]
        //public IActionResult search_notes(string search, int id)
        //{

        //    SqlQuery query = new SqlQuery();

        //    SearchService action = new SearchService(query.dbConn, query.incidentSearchQuery, id);
        //    //iraso radimo metodas
        //    action.search_incident(search, id);
        //    action.query_change(query.instruction_all);
        //    action.search_instruction(search, id);
        //    return View(action);






        //}
        //iraso perziura apimanti visa info
        public async Task<IActionResult> NoteReview(int incident_id,int department_access)
        {
            //SENA LOGIKA////////////////////////
            // TaskForm form = new TaskForm()
            // {
            //     noteId = incident_id,

            // };        
            //await _noteReviewService.taskExecution(form);
            /////////////////////////////////////////////////////
            TaskForm2 taskForm = new TaskForm2()
            {
                id = incident_id,
                department_access = department_access,
                departmentId = Convert.ToInt32(User.FindFirstValue("DepartmentId"))
            };

            await _noteRepository.ReviewTopicAsync(taskForm);


            //ViewData["tempDirectory"] = _filePathService.noteFilePath;          
            return View(taskForm.topicForm);
        }


        public async Task <IActionResult> NoteRegistration()
        {
          
            var navigation= await _navigationLoader.GetAsync();
            // var worker = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var departmentId = Convert.ToInt32(User.FindFirstValue("DepartmentId"));
           // var filtered = navigation.groups.Where(c => c.DepartmentAccess == departmentId);


            var filteredCategories = navigation.categories.Where(c=>c.DepartmentAccess == departmentId);
            var filtered = navigation.groups
                .Where(g => g.DepartmentAccess == departmentId &&
                            filteredCategories.Any(c => c.GroupId == g.Id))
                .ToList();

            TopicForm form = new TopicForm();
            form.categoryGroup = filtered;
            form.categories = filteredCategories;
            return View(form);
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

        [HttpPost]
        public async Task<IActionResult> NoteAdd(TopicForm topicForm, IEnumerable<IFormFile> formFiles)
        {

           // Console.WriteLine($"kategorija kurimo{topicForm.categoryId}");
           
            ModelState.Remove(key: "file");
            ModelState.Remove(key: "photos");
            if (!ModelState.IsValid)
            {
                //MODELSTATE PATIKRA
                // var errors = ModelState
                //.Where(m => m.Value.Errors.Count > 0)
                //.Select(m => new
                //{
                //    Field = m.Key,
                //    Errors = m.Value.Errors.Select(e => e.ErrorMessage).ToList()
                //}).ToList();

                // foreach (var error in errors)
                // {
                //     Console.WriteLine($"Field: {error.Field}");
                //     foreach (var err in error.Errors)
                //     {
                //         Console.WriteLine($"Error: {err}");
                //     }
                // }

                return View("noteRegistration");
            }

           
            //////perduodami duomenys i konstruktoriu
            ///

            try
            {
                // vartotojas_set();
                TaskForm2 taskForm = new TaskForm2()
                {
                    filesCollection = formFiles,
                    topicForm = topicForm,
                    workerId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    departmentId = Convert.ToInt32(User.FindFirstValue("DepartmentId")),
                    

                };

               
                //await _noteAddService.taskExecution(taskForm);
                await _noteRepository.AddTopicAsync(taskForm);

            }
            catch (Exception ex)
            {
                //cia tik data
                //DateOnly data =DateOnly.FromDateTime(DateTime.Now);
                DateTime data = DateTime.Now;

                string path = "../Darbu records/log/";
                string logName = $"log_{data:yyyy-MM-dd_HH-mm-ss}.txt";
                string fullpath = Path.Combine(path1: path, path2: logName);

                if (!Directory.Exists(path: path))
                {
                    Directory.CreateDirectory(path: path);


                }
                using (StreamWriter logcreate = new(fullpath))
                {
                    logcreate.WriteLine($"Time: {data} Message: {ex.Message}");
                }

                return RedirectToAction("Index", "Home");
                // Console.WriteLine(ex.Message);

            }



            return RedirectToAction("Index", "Home");
 
        }

        public async Task<IActionResult> GroupNoteView(int department_access)
        {
          //  Console.WriteLine($" ieskau {department_access}");         
            var groups = await _navigationLoader.GetAsync();


            var filtered = groups.groups.Where(c=>c.DepartmentAccess == department_access).ToList();


           
          //  filtered.groups.Where(c => c.DepartmentAccess == department_access).ToList();
         // await  _navigationLoader.LoadGroupsAsync(groups, department_access);


           // var group = _navigationLoader.Get();

            return View(filtered);
        }



        public async Task<IActionResult> CategoriesNoteView(int categoryId,int department_access)
        {
          
            //  Console.WriteLine(categoryId);
            //  Console.WriteLine(department_access);
            var categories = await _navigationLoader.GetAsync();
            var filtered = categories.categories.Where(c=>c.GroupId == categoryId && c.DepartmentAccess==department_access).ToList();

           // var filtered = _navigationLoader.Get().categories.Where(c => c.GroupId == categoryId && c.DepartmentAccess==department_access).ToList();


            return View(filtered);
        }










    }
}

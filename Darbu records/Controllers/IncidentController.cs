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

namespace Darbu_records.Controllers
{  [Authorize]
    public class IncidentController : Controller
    {

       private readonly INoteTaskService _incidentUpdateService;
       private readonly  INoteTaskService _incidentDeleteService;
       private readonly INoteTaskService _incidentAddService;
       private readonly INoteTaskService _incidentViewService;
       private readonly INoteTaskService _incidentReviewService;
        private readonly FilePathService _filePathService;
       
      
        public IncidentController([FromKeyedServices("NoteUpdateService")]INoteTaskService incidentUpdateService,
            [FromKeyedServices("NoteDeleteService")] INoteTaskService incidentDeleteService,
            [FromKeyedServices("NoteAddService")] INoteTaskService incidentAddService,
            [FromKeyedServices("NoteViewService")] INoteTaskService incidentViewService,
            [FromKeyedServices("NoteReviewService")]INoteTaskService incidentReviewService,
            FilePathService filePathService)
        {
               _incidentUpdateService = incidentUpdateService;
               _incidentDeleteService = incidentDeleteService;
               _incidentAddService = incidentAddService;
               _incidentViewService = incidentViewService;
               _incidentReviewService = incidentReviewService;
            _filePathService = filePathService;
        }



  

        //kiekviena karta kai pasileidzia Index yra nunulinama _record listas
        //visas sarasas
        public IActionResult Index()
        {
            //var ipadress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            //    Console.WriteLine(ipadress);
            
            return View();
        }
        public async Task<IActionResult> IncidentView(int category)
        {


            if (category == null) return View("Index");
            // _irasukiekis.kiekis(1);
         
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
              //  SqlQuery query = new SqlQuery();
                //initialize information
                TaskForm forma = new TaskForm()
                {
                    categoryId = category,
                    noteId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier))

                };


                await _incidentViewService.taskExecution(forma);


               // IQueryService queryService = new IncidentQueryService(query.dbConn, query.qr, vartotojas);
               // INoteTaskService noteService = new IncidentViewService(queryService);
                //IncidentCenter incidentView = new IncidentCenter(noteService, queryService);



                //execute
               // await incidentView.taskExecute(forma);



                return View(forma.incidentCollection);
            }

            return View();
        }
      

       

        [HttpPost]
        public async Task<IActionResult> IncidentDelete(int id)
        {

            TaskForm form = new TaskForm()
            {
                noteId = id
            };
          //  SqlQuery sqlQuery = new SqlQuery();
          //  IQueryService queryService = new IncidentQueryService(sqlQuery.dbConn, sqlQuery.Delete_Record);
          //  INoteTaskService incidentDeleteService = new IncidentDeleteService(queryService);
         await _incidentDeleteService.taskExecution(form);
         //  await incidentDeleteService.taskExecution(form);


          



            return RedirectToAction("Index", "Home");
        }

       
        //Iraso redagavimo realizacija-------
        //inicializacija
        public IActionResult IncidentUpdate(IncidentForm incidentForm)
        {

          
            return View(incidentForm);

        }

        //ivykdymas
        public async Task<IActionResult> IncidentUpdateExecute(IncidentForm forma)
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
           
            await _incidentUpdateService.taskExecution(taskForm);


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
        public async Task<IActionResult> IncidentReview(int incident_id)
        {       
            TaskForm form = new TaskForm()
            {
                noteId = incident_id,
                
            };        
           await _incidentReviewService.taskExecution(form);
           // ViewData["tempDirectory"] = _filePathService.incidentFilePath;          
            return View(form.incidentForm);
        }


        public IActionResult IncidentRegistration()
        {
            //Gedimo_forma gedimo_forma = new Gedimo_forma();
            //sukuriam List<IFormFile> images
            // gedimo_forma.images_initializes();

            return View();
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
        public async Task<IActionResult> IncidentAdd(IncidentForm gedimo_forma, List<IFormFile> images, IFormFile file)
        {
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

                return View("incidentRegistration");
            }

           
            //////perduodami duomenys i konstruktoriu
            ///

            try
            {
               // vartotojas_set();
                TaskForm taskForm = new TaskForm()
                {
                    file = file,
                    filesCollection = images,
                    incidentForm = gedimo_forma,
                    noteId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier))

                };
          
                await _incidentAddService.taskExecution(taskForm);

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










    }
}

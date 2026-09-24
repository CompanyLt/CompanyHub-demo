using Darbu_records.DAL;
using Darbu_records.DepartmentBoardManagement;
using Darbu_records.Formos;
using Darbu_records.IncidentManagement;

using Darbu_records.Models;
using Darbu_records.Models.Topic;
using Darbu_records.Query.Topic;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Darbu_records.Controllers
{
    public class DepartmentBoardController : Controller
    {


       DBoardRepository _dBoardRepository;
        


        public DepartmentBoardController(DBoardRepository dBoardRepository)
        {
           _dBoardRepository = dBoardRepository;
           
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task <IActionResult> TopicReview(int id)
        {


            TaskForm2 taskForm = new TaskForm2()
            {
                id = id,
                departmentId = Convert.ToInt32(User.FindFirstValue("departmentId"))
                       
            };
           





            await _dBoardRepository.ReviewTopicAsync(taskForm);




            return View(taskForm.topicForm);
        }


        public IActionResult TopicRegistration()
        {
            return View();
        }


        public async Task<IActionResult> AddTopic(TopicForm topicForm,IEnumerable<IFormFile> formFiles)
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
              
                return View("topicRegistration");
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
                    departmentId = Convert.ToInt32(User.FindFirstValue("DepartmentId"))

                }; 

             
                await _dBoardRepository.AddTopicAsync(taskForm);

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


            ////

            //await _topicAddRepository.AddTopicAsync(taskForm);



            return View();
        }






    }
}

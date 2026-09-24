using Darbu_records.CommentManagement;
using Darbu_records.Formos;
using Darbu_records.Models;
using Darbu_records.Models.Topic;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Darbu_records.Controllers
{
    public class CommentController : Controller
    {

        CommentRepository _commentRepository;

        public CommentController(CommentRepository commentRepository) {
        _commentRepository = commentRepository;
        }
        public IActionResult Index()
        {
            return View();
        }







        public async Task <IActionResult> AddComment(Comment comment,IEnumerable<IFormFile> formFiles)
        {
          //  Console.WriteLine($" id: {comment.topic_id} textas{comment.text} pavadinimas{comment.title} ");

            TaskForm2 taskForm = new TaskForm2()
            {
                filesCollection = formFiles,
                comment=comment,
                workerId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                departmentId = Convert.ToInt32(User.FindFirstValue("DepartmentId"))

            };





            await  _commentRepository.AddTopicAsync(taskForm);




            return RedirectToAction("Index", "Home");
        }


    }
}

using Darbu_records.CommentManagement;
using Darbu_records.Models;
using Microsoft.AspNetCore.Mvc;

namespace Darbu_records.Components
{
    public class CommentViewComponent:ViewComponent
    {
        CommentLoadService _commentLoadService;

        public CommentViewComponent(CommentLoadService commentLoadService) 
        {
        _commentLoadService = commentLoadService;
        
        }

        public async Task<IViewComponentResult> InvokeAsync(string tableName,int topic_id)
        {





            List<Comment> comments = await _commentLoadService.GetComments(tableName,topic_id);

           


            return View(comments);
        }
    }
}

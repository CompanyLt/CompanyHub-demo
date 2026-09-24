using Darbu_records.Formos;
using Darbu_records.Interfaces;
using Darbu_records.Models;
using Darbu_records.Query.Topic;
using Darbu_records.TopicManagement.Services;

namespace Darbu_records.CommentManagement
{
    public class CommentRepository
    {
        CommentAddService _commentAddService;

        //reikai query builderio
        ITopicQueryBuilder _topicQueryBuilder;
        IFilePathBuilder _fileBuilder;
        //FilePath builderio irgi


        public CommentRepository([FromKeyedServices("CommentAddQueryBuilder")] ITopicQueryBuilder topicQueryBuilder,
            TopicAddService topicAddService,
            TopicReviewService topicReviewService,
            [FromKeyedServices("DboardFilePathBuilder")] IFilePathBuilder filePathBuilder,
            CommentAddService commentAddService
            )
        {
           
            _fileBuilder = filePathBuilder;
            _topicQueryBuilder = topicQueryBuilder;
            _commentAddService = commentAddService;
        }

        public async Task AddTopicAsync(TaskForm2 taskForm)
        {
            _topicQueryBuilder.SetQuery();        
            await _commentAddService.taskExecution(taskForm, _topicQueryBuilder, _fileBuilder);
          //  await _topicAddService.taskExecution(taskForm, _topicQueryBuilder, _fileBuilder);

        }



        public async Task ReviewTopicAsync(TaskForm2 taskForm)
        {
         //   _topicReviewQueryBuilder.SetQuery();
          //  await _topicReviewService.taskExecution(taskForm, _topicReviewQueryBuilder);



        }




        public void Debug(TaskForm2 taskForm)
        {
          //  Console.WriteLine($"_topicReviewService is null? {_topicReviewService == null}");
         //   Console.WriteLine($"_topicReviewQueryBuilder is null? {_topicReviewQueryBuilder == null}");
          //  Console.WriteLine($"taskForm is null? {taskForm == null}");
        }



    }

}


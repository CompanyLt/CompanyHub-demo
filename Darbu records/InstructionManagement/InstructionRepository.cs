using Darbu_records.DAL;
using Darbu_records.Formos;
using Darbu_records.Interfaces;
using Darbu_records.Query.Topic;
using Darbu_records.TopicManagement.Services;

namespace Darbu_records.InstructionManagement
{
    public class InstructionRepository
    {


       private readonly TopicAddService _topicAddService;
       private readonly TopicReviewService _topicReviewService;
       private readonly ITopicQueryBuilder _topicAddQueryBuilder;
       private readonly ITopicQueryBuilder _topicReviewQueryBuilder;       
       private readonly IFilePathBuilder _fileBuilder;


        public InstructionRepository([FromKeyedServices("InstructionAddQueryBuilder")] ITopicQueryBuilder topicAddQueryBuilder,
            [FromKeyedServices("InstructionReviewQueryBuilder")] ITopicQueryBuilder topicReviewQueryBuilder,
            TopicAddService topicAddService,           
            [FromKeyedServices("InstructionFilePathBuilder")] IFilePathBuilder filePathBuilder,
            TopicReviewService topicReviewService
            )
        {
            _topicAddService = topicAddService;

            _topicAddQueryBuilder = topicAddQueryBuilder;

            _topicReviewQueryBuilder = topicReviewQueryBuilder;
        
            _fileBuilder = filePathBuilder;

            _topicReviewService = topicReviewService;

        }

        public async Task AddTopicAsync(TaskForm2 taskForm)
        {
            //LOAD query
            _topicAddQueryBuilder.SetQuery();        
            await _topicAddService.taskExecution(taskForm, _topicAddQueryBuilder, _fileBuilder);

        }




        public async Task ReviewTopicAsync(TaskForm2 taskForm)
        {
            _topicReviewQueryBuilder.SetQuery();
           // Console.WriteLine(_topicAddQueryBuilder.GetMainTopic());
            await _topicReviewService.taskExecution(taskForm, _topicReviewQueryBuilder);


        }



    }



}


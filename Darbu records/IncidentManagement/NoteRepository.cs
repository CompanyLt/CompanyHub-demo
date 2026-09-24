using Darbu_records.DAL;
using Darbu_records.Enums.Topics;
using Darbu_records.Formos;
using Darbu_records.Interfaces;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Darbu_records.Models.Topic;
using Darbu_records.OldItems;
using Darbu_records.Query;
using Darbu_records.Query.DepartmentBoard;
using Darbu_records.Query.Topic;
using Darbu_records.TopicManagement.Services;
using DevExpress.Xpo.DB.Helpers;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;

namespace Darbu_records.IncidentManagement
{
    public class NoteRepository
    {

     
        TopicAddService _topicAddService { set; get; }
        TopicReviewService _topicReviewService { set; get; }

        public ITopicQueryBuilder _topicAddQueryBuilder;

        public ITopicQueryBuilder _topicReviewQueryBuilder;

       
        IFilePathBuilder _fileBuilder;


        public NoteRepository([FromKeyedServices("NoteAddQueryBuilder")] ITopicQueryBuilder topicAddQueryBuilder,
            [FromKeyedServices("NoteReviewQueryBuilder")] ITopicQueryBuilder topicReviewQueryBuilder,
            TopicAddService topicAddService,            
            [FromKeyedServices("NoteFilePathBuilder")] IFilePathBuilder filePathBuilder,
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
            await  _topicAddService.taskExecution(taskForm,_topicAddQueryBuilder,_fileBuilder);
                       
        }




        public async Task ReviewTopicAsync(TaskForm2 taskForm)
        {
                 _topicReviewQueryBuilder.SetQuery();
           // Console.WriteLine(_topicAddQueryBuilder.GetMainTopic());
           await _topicReviewService.taskExecution(taskForm, _topicReviewQueryBuilder);


        }



    }









}
    

    


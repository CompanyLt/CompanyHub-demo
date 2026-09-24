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

namespace Darbu_records.DepartmentBoardManagement
{
    public class DBoardRepository
    {

     
        TopicAddService _topicAddService { set; get; }
        TopicReviewService _topicReviewService { set; get; }

        public ITopicQueryBuilder _topicQueryBuilder;
        public ITopicQueryBuilder _topicReviewQueryBuilder;     
        IFilePathBuilder _fileBuilder;


        public DBoardRepository([FromKeyedServices("DepartmentBoardQueryBuilder")] ITopicQueryBuilder topicQueryBuilder,
            [FromKeyedServices("DBoardReviewQueryBuilder")] ITopicQueryBuilder topicReviewQueryBuilder,
            TopicAddService topicAddService,
            TopicReviewService topicReviewService,
            [FromKeyedServices("DboardFilePathBuilder")]IFilePathBuilder filePathBuilder)
        {
            _topicAddService = topicAddService;
            _topicQueryBuilder = topicQueryBuilder;        
            _fileBuilder = filePathBuilder;
            _topicReviewQueryBuilder = topicReviewQueryBuilder;
            _topicReviewService = topicReviewService;
        }

        public async Task AddTopicAsync(TaskForm2 taskForm)
        {
           
            _topicQueryBuilder.SetQuery();
            await  _topicAddService.taskExecution(taskForm,_topicQueryBuilder,_fileBuilder);
                       
        }



        public async Task ReviewTopicAsync(TaskForm2 taskForm)
        {
            _topicReviewQueryBuilder.SetQuery();
           // Console.WriteLine(_topicReviewQueryBuilder.GetMainTopic());
            await _topicReviewService.taskExecution(taskForm, _topicReviewQueryBuilder);



        }




        public void Debug(TaskForm2 taskForm)
        {
            Console.WriteLine($"_topicReviewService is null? {_topicReviewService == null}");
            Console.WriteLine($"_topicReviewQueryBuilder is null? {_topicReviewQueryBuilder == null}");
            Console.WriteLine($"taskForm is null? {taskForm == null}");
        }



    }









}
    

    


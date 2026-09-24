using Darbu_records.DAL;
using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Darbu_records.Models.Topic;
using Darbu_records.Query.Topic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace Darbu_records.TopicManagement.Services
{
    public class TopicReviewService
    {
        IQueryService _queryService;
        FileCommunicationService _fileCommunicationService;
        ITopicQueryBuilder _topicQueryBuilder;
      

        public TopicReviewService([FromKeyedServices("NoteUpdateQueryService")] IQueryService queryService,
            FileCommunicationService fileCommunicationService
           )
        {
            _queryService = queryService;
            _fileCommunicationService = fileCommunicationService;
         
        }

        public async Task taskExecution(TaskForm2 taskForm,ITopicQueryBuilder topicQueryBuilder)
        {
            _topicQueryBuilder = topicQueryBuilder;

           // Console.WriteLine(taskForm.department_access);
           // Console.WriteLine(taskForm.departmentId);
            using (SqlConnection connection = _queryService.sqlConnGet())
            {
              //  var worker = await _workerLoader.GetAsync();
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(_topicQueryBuilder.GetMainTopic(), connection))
                {
                   
                    command.Parameters.Add(new SqlParameter("@topic_id", SqlDbType.Int) { Value = taskForm.id });
                    command.Parameters.Add(new SqlParameter("@status", SqlDbType.Int) { Value = 1 });
                    command.Parameters.Add(new SqlParameter("@department_id", SqlDbType.Int) { Value = taskForm.departmentId});
                    command.Parameters.Add(new SqlParameter("@department_access", SqlDbType.Int) { Value =taskForm.department_access });                 



                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {

                            taskForm.topicForm = new TopicForm()
                            {
                                title = reader["title"].ToString() ?? string.Empty,
                                description = reader["description"]?.ToString() ?? string.Empty,
                                noteId = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0

                            };


                            //json failiukas is sql
                            if (reader["Photos"] != DBNull.Value)
                            {
                                var jsonPath = reader["Photos"].ToString();
                                taskForm.topicForm.fileForm.photosCollection = JsonSerializer.Deserialize<List<RecordFile>>(jsonPath);


                            }

                            List<RecordFile> fileCollection = new();
                            if (reader["Files"] != DBNull.Value)
                            {
                                var jsonPath = reader["Files"].ToString();
                                fileCollection = JsonSerializer.Deserialize<List<RecordFile>>(jsonPath);

                            }
                            //CIA reiktu ideti Failu rusiavima nes razor puseje rusiavima daryti nieko gero                          
                            foreach (var n in fileCollection)
                            {
                                if (_fileCommunicationService.checkPdfExtention(n.name))
                                {
                                    taskForm.topicForm.fileForm.pdfCollection.Add(n);

                                }
                                else
                                {
                                    taskForm.topicForm.fileForm.filesCollection.Add(n);
                                }


                            }




                        }




                    }

                }
                await connection.CloseAsync();


                if (taskForm.topicForm == null) taskForm.topicForm = new TopicForm();
            }

        }
    }
}

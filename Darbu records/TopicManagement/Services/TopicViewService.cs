using Darbu_records.DAL;
using Darbu_records.Formos;
using Darbu_records.Models.Topic;
using Darbu_records.Models;
using Darbu_records.Query.Topic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;
using Darbu_records.Interfaces.NoteService;

namespace Darbu_records.TopicManagement.Services
{
    public class TopicViewService
    {
        IQueryService _queryService;       
        ITopicQueryBuilder _topicQueryBuilder;
  

        public TopicViewService([FromKeyedServices("NoteUpdateQueryService")] IQueryService queryService, WorkerLoader workerLoader)
        {
            _queryService = queryService;                      
        }

        public async Task taskExecution(TaskForm2 taskForm, ITopicQueryBuilder topicQueryBuilder)
        {
            _topicQueryBuilder = topicQueryBuilder;



            using (SqlConnection connection = _queryService.sqlConnGet())
            {
              
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(_topicQueryBuilder.GetMainTopic(), connection))
                {

                    command.Parameters.Add(new SqlParameter("@category", taskForm.id));
                    command.Parameters.Add(new SqlParameter("@department_id", taskForm.departmentId));
                    command.Parameters.Add(new SqlParameter("@department_access", taskForm.department_access));
                    command.Parameters.Add(new SqlParameter("@status", 1));



                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {


                            TopicForm topicForm = new TopicForm()
                            {
                                title = reader["title"].ToString(),
                                description = reader["description"].ToString(),
                                noteId = Convert.ToInt32(reader["id"]),
                                department_access = Convert.ToInt32(reader["department_access"]),
                                categoryId = Convert.ToInt32(reader["category_id"])
                            };


                            taskForm.topicCollection.Add(topicForm);

                          
                  }




                    }

                }
                await connection.CloseAsync();

            
            }

        }
    }
}

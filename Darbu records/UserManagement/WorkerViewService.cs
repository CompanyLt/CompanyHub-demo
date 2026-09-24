using Darbu_records.DAL;
using Darbu_records.Formos;
using Darbu_records.Interfaces;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Darbu_records.Models.Topic;
using Darbu_records.Models.User;
using Darbu_records.Query.Topic;
using Darbu_records.Query.User;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace Darbu_records.UserManagement
{
    public class WorkerViewService
    {

        IQueryService _queryService;
        IUserQueryBuilder _workerQueryBuilder;


        public WorkerViewService([FromKeyedServices("NoteUpdateQueryService")] IQueryService queryService, WorkerLoader workerLoader)
        {
            _queryService = queryService;
        }

        public async Task taskExecution(UserTaskForm taskForm, IUserQueryBuilder workerQueryBuilder)
        {
            _workerQueryBuilder = workerQueryBuilder;

            try
            {
                using var connection = _queryService.sqlConnGet();
                await connection.OpenAsync();


                using var command = new SqlCommand(_workerQueryBuilder.GetMainQuery(), connection);
                command.Parameters.Add(new SqlParameter("@department_id", SqlDbType.Int) { Value = taskForm.departmentId });
               



                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {


                    Worker worker = new Worker()
                    {
                        id = Convert.ToInt32(reader["id"]),
                        name = reader["name"].ToString(),
                        jobTitle = reader["jobTitle"].ToString()

                    };

                    if (!reader.IsDBNull(reader.GetOrdinal("Files")))
                    {

                        var jsonPath = reader.GetString(reader.GetOrdinal("Files"));
                        worker.recordFile = JsonSerializer.Deserialize<RecordFile>(jsonPath);


                    }


                    taskForm.workers.Add(worker);


                }


            }catch(SqlException ex)
            {
                throw;
            }catch (Exception ex)
            {
                throw;
            }

                


        }







    }
}

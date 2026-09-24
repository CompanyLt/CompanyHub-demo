using Darbu_records.DAL;
using Darbu_records.Formos;
using Darbu_records.Interfaces;
using Darbu_records.Models.Topic;
using Darbu_records.Models;
using Darbu_records.Query.Topic;
using Microsoft.Data.SqlClient;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Query.Department;
using Darbu_records.Models.Department;
using System.Data;
using Darbu_records.CategoryManagement;
using Darbu_records.Query.Categories;

namespace Darbu_records.DepartmentManagement
{
    public class DepartmentAddService
    {

      private readonly IQueryService _queryService;      
      private readonly IDepartmentQueryBuilder _departmentQueryBuilder;
        private readonly IFilePathBuilder _departmentFilePathBuilder;


        public DepartmentAddService([FromKeyedServices("InstructionUpdateQueryService")] IQueryService queryService,
            [FromKeyedServices("CreateDepartmentQueryBuilder")]IDepartmentQueryBuilder departmentQueryBuilder,
            [FromKeyedServices("DepartmentFilePathBuilder")] IFilePathBuilder departmentFilePathBuilder)
        {
            _queryService = queryService;
            _departmentQueryBuilder = departmentQueryBuilder;
            _departmentFilePathBuilder = departmentFilePathBuilder;
           
        }

        public async Task taskExecution(DepartmentTaskForm taskForm)
        {

            _departmentQueryBuilder.SetQuery();
            //Pradinei parametrai
           // TopicForm topicForm = taskForm.topicForm;

           
            string query = _departmentQueryBuilder.GetMainTopic();


            //////////////////////////////////////////////////////////////

            int departmentId = -1;

            using var connection = _queryService.sqlConnGet();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                using var command = new SqlCommand(cmdText: query, connection: connection, transaction);
                // Console.WriteLine($"id : {taskForm.workerId}, {topicForm.title}, {topicForm.description}, {taskForm.departmentId}");
                command.Parameters.Add(new SqlParameter("@name",taskForm.name));
                command.Parameters.Add(new SqlParameter("@avatar", "departmentAvatar.png"));
                command.Parameters.Add(new SqlParameter("@created_by", taskForm.created_by));
                command.Parameters.Add(new SqlParameter("@status", "active"));
                departmentId = (int)(await command.ExecuteScalarAsync() ?? 0);








                if (_departmentQueryBuilder.GetNavigationActionAccess() != string.Empty)
                {
                    query = _departmentQueryBuilder.GetNavigationActionAccess();
                        using var accessCommand = new SqlCommand(query, connection, transaction);
                        accessCommand.Parameters.Add(new SqlParameter("@action_id", SqlDbType.Int));
                        accessCommand.Parameters.Add(new SqlParameter("@department_id", SqlDbType.Int));  


                    foreach(var n in taskForm.navigationActionCollection)
                    {
                        accessCommand.Parameters["@action_id"].Value = n.Id;
                        accessCommand.Parameters["@department_id"].Value = departmentId;
                        await accessCommand.ExecuteNonQueryAsync();

                    }
                   




                }


                //cia pridedami failai
                if (_departmentQueryBuilder.GetFileQuery() != string.Empty && taskForm.formImage != null)
                {
                    FileCommunicationService fileCommunicationService = new FileCommunicationService();
                    string fileDirectory = _departmentFilePathBuilder.GetFilePath(taskForm.name, string.Empty, string.Empty);
                    fileCommunicationService.checkDirection(fileDirectory);
                    //SUKURIAME FAILA JEI TOKS YRA IR GRAZINAME JI
                    RecordFile recordFile = await fileCommunicationService.CreateFile(taskForm.formImage, fileDirectory);

                    using var fileCommand = new SqlCommand(_departmentQueryBuilder.GetFileQuery(), connection, transaction);
                    fileCommand.Parameters.Add(new SqlParameter("@department_id", departmentId));
                    fileCommand.Parameters.Add(new SqlParameter("@file_name", recordFile.name));
                    fileCommand.Parameters.Add(new SqlParameter("@directory", recordFile.directory));
                    fileCommand.Parameters.Add(new SqlParameter("@original_name", recordFile.originalName));
                    await fileCommand.ExecuteNonQueryAsync();



                }


                transaction.Commit();
            }
            catch (Exception ex)
            {

                //LOGIKA PRIDEJIMAS I LOGERI AR PAN
                //_logger.Add(ex);
                transaction.Rollback();
                throw;
            }


        }






    }








}


using Darbu_records.DAL;
using Darbu_records.Models.User;
using Darbu_records.Models;
using Darbu_records.Query.User;
using Microsoft.Data.SqlClient;
using System.Data;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Interfaces;
using Darbu_records.Models.Topic;
using Darbu_records.Query.Topic;
using DevExpress.Xpo.DB.Helpers;
using System.Transactions;

namespace Darbu_records.UserManagement
{
    public class WorkerAddService
    {

        IQueryService _queryService;
        IUserQueryBuilder _workerQueryBuilder;
        IFilePathBuilder _workerFilePathBuilder;

        public WorkerAddService([FromKeyedServices("NoteUpdateQueryService")] IQueryService queryService,
            [FromKeyedServices("WorkerFilePathBuilder")]IFilePathBuilder workerFilePathBuilder
            )
        {
            _workerFilePathBuilder = workerFilePathBuilder;
            _queryService = queryService;
        }

        public async Task taskExecution(CreateUserTaskForm taskForm, IUserQueryBuilder workerQueryBuilder)
        {
            _workerQueryBuilder = workerQueryBuilder;

           
            FileCommunicationService fileCommunicationService = new FileCommunicationService();                  
            string fileDirectory = _workerFilePathBuilder.GetFilePath(string.Empty, string.Empty, string.Empty);          
            fileCommunicationService.checkDirection(fileDirectory);
           
            //SUKURIA IR GRAZINA OBJEKTA KURI NAUDOSIME REGISTRAVIMUI I SQL
           RecordFile recordFile = await fileCommunicationService.CreateFile(taskForm.formImage, fileDirectory);


            int workerId = -1;
            using var connection = _queryService.sqlConnGet();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
               


                using var command = new SqlCommand(_workerQueryBuilder.GetMainQuery(), connection,transaction);
                command.Parameters.Add(new SqlParameter("@name", taskForm.name));
                command.Parameters.Add(new SqlParameter("@surname", taskForm.surname));
                command.Parameters.Add(new SqlParameter("@jobTitle", taskForm.jobTitle));
                command.Parameters.Add(new SqlParameter("@department_id", taskForm.departmentId));
                command.Parameters.Add(new SqlParameter("@role_id", taskForm.departmentId));
                command.Parameters.Add(new SqlParameter("@status",1));
                workerId = (int)(await command.ExecuteScalarAsync() ?? 0);



                if (_workerQueryBuilder.GetFilesQuery() != string.Empty)
                {
                  
                    
                        using var fileCommand = new SqlCommand(_workerQueryBuilder.GetFilesQuery(), connection, transaction);
                        fileCommand.Parameters.Add(new SqlParameter("@worker_id", workerId));
                        fileCommand.Parameters.Add(new SqlParameter("@file_name", recordFile.name));
                        fileCommand.Parameters.Add(new SqlParameter("@directory", recordFile.directory));
                        fileCommand.Parameters.Add(new SqlParameter("@original_name", recordFile.originalName));
                        await fileCommand.ExecuteNonQueryAsync();


                    
                }

              await  transaction.CommitAsync();
            }
            catch (SqlException ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }




        }






    }
}

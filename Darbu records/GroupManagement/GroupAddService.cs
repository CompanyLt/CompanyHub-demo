using Darbu_records.DAL;
using Darbu_records.Formos;
using Darbu_records.Interfaces;
using Darbu_records.Models.Topic;
using Darbu_records.Models;
using Darbu_records.Query.Topic;
using Microsoft.Data.SqlClient;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models.Group;
using Darbu_records.UserManagement;
using Darbu_records.Query.Group;

namespace Darbu_records.GroupManagement
{
    public class GroupAddService
    {
        IQueryService _queryService;
       
        IGroupQueryBuilder _createGroupQueryBuilder;
        IFilePathBuilder _groupFilePathBuilder;
        


        public GroupAddService([FromKeyedServices("InstructionUpdateQueryService")] IQueryService queryService,
            [FromKeyedServices("GroupFilePathBuilder")]IFilePathBuilder groupFilePathBuilder,
            [FromKeyedServices("CreateGroupQueryBuilder")]IGroupQueryBuilder createGroupQueryBuilder
            )
        {
            _queryService = queryService;
            _groupFilePathBuilder = groupFilePathBuilder;
            _createGroupQueryBuilder = createGroupQueryBuilder;
          
        }

        public async Task<int> taskExecution(CreateGroupForm taskForm)
        {
            //Seteris
            _createGroupQueryBuilder.SetQuery();



            //Pradinei parametrai

            var query = _createGroupQueryBuilder.GetMainQuery();


            //////////////////////////////////////////////////////////////

            int groupId = -1;

            using var connection = _queryService.sqlConnGet();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                using var command = new SqlCommand(cmdText: query, connection: connection, transaction);
                // Console.WriteLine($"id : {taskForm.workerId}, {topicForm.title}, {topicForm.description}, {taskForm.departmentId}");
                command.Parameters.Add(new SqlParameter("@group_name", taskForm.group_name));
                command.Parameters.Add(new SqlParameter("@status", 1));
                command.Parameters.Add(new SqlParameter("@created_by", taskForm.created_by));             
               
                groupId = (int)(await command.ExecuteScalarAsync() ?? 0);








                if (_createGroupQueryBuilder.GetAccessQuery() != string.Empty)
                {
                    query = _createGroupQueryBuilder.GetAccessQuery();

                    using var accessCommand = new SqlCommand(query, connection, transaction);
                    accessCommand.Parameters.Add(new SqlParameter("@group_id", groupId));
                    accessCommand.Parameters.Add(new SqlParameter("@department_id", taskForm.department_id));
                    accessCommand.Parameters.Add(new SqlParameter("@department_access", taskForm.department_access));
                    await accessCommand.ExecuteNonQueryAsync();




                }





                //cia pridedami failai
                if (_createGroupQueryBuilder.GetFileQuery() != string.Empty && taskForm.formImage != null)
                {
                    FileCommunicationService fileCommunicationService = new FileCommunicationService();
                    string fileDirectory = _groupFilePathBuilder.GetFilePath(taskForm.group_name, string.Empty, string.Empty);
                    fileCommunicationService.checkDirection(fileDirectory);
                    RecordFile recordFile = await fileCommunicationService.CreateFile(taskForm.formImage, fileDirectory);




                    using var fileCommand = new SqlCommand(_createGroupQueryBuilder.GetFileQuery(), connection, transaction);
                    fileCommand.Parameters.Add(new SqlParameter("@group_id", groupId));
                    fileCommand.Parameters.Add(new SqlParameter("@file_name", recordFile.name));
                    fileCommand.Parameters.Add(new SqlParameter("@directory", recordFile.directory));
                    fileCommand.Parameters.Add(new SqlParameter("@original_name", recordFile.originalName));
                    await fileCommand.ExecuteNonQueryAsync();



                }
              


                await transaction.CommitAsync();
                return groupId;
            }
            catch (Exception ex)
            {

                //LOGIKA PRIDEJIMAS I LOGERI AR PAN
                //_logger.Add(ex);
                await transaction.RollbackAsync();
                throw;
            }


        }






    }
}


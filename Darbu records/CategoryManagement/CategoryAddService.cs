using Darbu_records.DAL;
using Darbu_records.Interfaces;
using Darbu_records.Models.Group;
using Darbu_records.Models;
using Darbu_records.Query.Group;
using Microsoft.Data.SqlClient;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Query.Categories;
using Darbu_records.Models.Categories;
using Darbu_records.Enums;

namespace Darbu_records.CategoryManagement
{
    public class CategoryAddService
    {

        IQueryService _queryService;

        ICategoryQueryBuilder _createCategoryQueryBuilder;
        IFilePathBuilder _categoryFilePathBuilder;



        public CategoryAddService([FromKeyedServices("InstructionUpdateQueryService")] IQueryService queryService,
            [FromKeyedServices("CategoryFilePathBuilder")] IFilePathBuilder categoryFilePathBuilder,
            [FromKeyedServices("CreateCategoryQueryBuilder")] ICategoryQueryBuilder createCategoryQueryBuilder
            )
        {
            _queryService = queryService;
            _categoryFilePathBuilder = categoryFilePathBuilder;
            _createCategoryQueryBuilder = createCategoryQueryBuilder;

        }

        public async Task<int> taskExecution(CreateCategoryForm taskForm)
        {
            //Seteris
            _createCategoryQueryBuilder.SetQuery();



            //Pradinei parametrai


           
            var query = _createCategoryQueryBuilder.GetMainQuery();


            //////////////////////////////////////////////////////////////

            int categoryId = -1;

            using var connection = _queryService.sqlConnGet();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                using var command = new SqlCommand(cmdText: query, connection: connection, transaction);
                // Console.WriteLine($"id : {taskForm.workerId}, {topicForm.title}, {topicForm.description}, {taskForm.departmentId}");
                command.Parameters.Add(new SqlParameter("@category_name", taskForm.category_name));
                command.Parameters.Add(new SqlParameter("@status", 1));
                command.Parameters.Add(new SqlParameter("@department_id", taskForm.department_id));
                command.Parameters.Add(new SqlParameter("@created_by", taskForm.created_by));

                categoryId = (int)(await command.ExecuteScalarAsync() ?? 0);








                if (_createCategoryQueryBuilder.GetAccessQuery() != string.Empty)
                {
                    query = _createCategoryQueryBuilder.GetAccessQuery();

                    using var accessCommand = new SqlCommand(query, connection, transaction);
                    accessCommand.Parameters.Add(new SqlParameter("@category_id", categoryId));
                    accessCommand.Parameters.Add(new SqlParameter("@department_id", taskForm.department_id));
                    accessCommand.Parameters.Add(new SqlParameter("@department_access", taskForm.department_access));
                    await accessCommand.ExecuteNonQueryAsync();




                }





                //cia pridedami failai
                if (_createCategoryQueryBuilder.GetFileQuery() != string.Empty && taskForm.formImage !=null)
                {
                    FileCommunicationService fileCommunicationService = new FileCommunicationService();
                    string fileDirectory = _categoryFilePathBuilder.GetFilePath(taskForm.category_name, string.Empty, string.Empty);
                    fileCommunicationService.checkDirection(fileDirectory);
                    //SUKURIAME FAILA JEI TOKS YRA IR GRAZINAME JI
                    RecordFile recordFile = await fileCommunicationService.CreateFile(taskForm.formImage, fileDirectory);

                    using var fileCommand = new SqlCommand(_createCategoryQueryBuilder.GetFileQuery(), connection, transaction);
                    fileCommand.Parameters.Add(new SqlParameter("@category_id", categoryId));
                    fileCommand.Parameters.Add(new SqlParameter("@file_name", recordFile.name));
                    fileCommand.Parameters.Add(new SqlParameter("@directory", recordFile.directory));
                    fileCommand.Parameters.Add(new SqlParameter("@original_name", recordFile.originalName));
                    await fileCommand.ExecuteNonQueryAsync();



                }



                await transaction.CommitAsync();
                return categoryId;
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

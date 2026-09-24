using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Darbu_records.Models.Categories;
using Darbu_records.Models.Department;
using Darbu_records.Query.Categories;
using Darbu_records.Query.Department;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Darbu_records.CategoryManagement
{
    public class CategoryToGroupService
    {

        private readonly IQueryService _queryService;
        private readonly ICategoryQueryBuilder _categoryToGroupQueryBuilder;



        public CategoryToGroupService([FromKeyedServices("InstructionUpdateQueryService")] IQueryService queryService,
            [FromKeyedServices("CategoryToGroupQueryBuilder")] ICategoryQueryBuilder categoryToGroupQueryBuilder)
        {
            _queryService = queryService;
            _categoryToGroupQueryBuilder = categoryToGroupQueryBuilder;

        }

        public async Task taskExecution(CategoryToGroupForm form)
        {

            _categoryToGroupQueryBuilder.SetQuery();
            //Pradinei parametrai
            // TopicForm topicForm = taskForm.topicForm;


            string query = _categoryToGroupQueryBuilder.GetMainQuery();


            //////////////////////////////////////////////////////////////

           
            using var connection = _queryService.sqlConnGet();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
               
                using var command = new SqlCommand(cmdText: query, connection: connection, transaction);
                // Console.WriteLine($"id : {taskForm.workerId}, {topicForm.title}, {topicForm.description}, {taskForm.departmentId}");
             //   command.Parameters.Add(new SqlParameter("@group_id", form.id));
                command.Parameters.Add(new SqlParameter("@group_id",SqlDbType.Int));
                           
                    command.Parameters["@group_id"].Value = form.id;                  
                    await command.ExecuteNonQueryAsync();
              







                if (_categoryToGroupQueryBuilder.GetAccessQuery() != string.Empty)
                {
                    query = _categoryToGroupQueryBuilder.GetAccessQuery();
                    using var accessCommand = new SqlCommand(query, connection, transaction);
                    accessCommand.Parameters.Add(new SqlParameter("@group_id", SqlDbType.Int));
                    accessCommand.Parameters.Add(new SqlParameter("@category_id", SqlDbType.Int));

                    foreach (var category in form.categories)
                    {
                        accessCommand.Parameters["@group_id"].Value = form.id;
                        accessCommand.Parameters["@category_id"].Value = category.Id;
                        await accessCommand.ExecuteNonQueryAsync();

                    }





                }


                transaction.Commit();
            }
            catch (Exception ex)
            {

                //LOGIKA PRIDEJIMAS I LOGERI AR PAN
                //_logger.Add(ex);
                Console.WriteLine(ex.ToString());
                transaction.Rollback();
                throw;
            }


        }



    }
}

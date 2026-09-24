using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models.Categories;
using Darbu_records.Models.Group;
using Darbu_records.Query.Categories;
using Darbu_records.Query.Group;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Darbu_records.GroupManagement
{
    public class GroupToCategoryService
    {


        private readonly IQueryService _queryService;
        private readonly IGroupQueryBuilder _groupToCategoryQueryBuilder;



        public GroupToCategoryService([FromKeyedServices("InstructionUpdateQueryService")] IQueryService queryService,
            [FromKeyedServices("GroupToCategoryQueryBuilder")] IGroupQueryBuilder groupToCategoryQueryBuilder)
        {
            _queryService = queryService;
            _groupToCategoryQueryBuilder = groupToCategoryQueryBuilder;

        }

        public async Task taskExecution(GroupToCategoryForm form)
        {
            //INICIALIZACIJA
            _groupToCategoryQueryBuilder.SetQuery();
            //Pradinei parametrai
            // TopicForm topicForm = taskForm.topicForm;


            string query = _groupToCategoryQueryBuilder.GetMainQuery();

           
            //////////////////////////////////////////////////////////////

           

            using var connection = _queryService.sqlConnGet();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
              
                using var command = new SqlCommand(cmdText: query, connection: connection, transaction);
                // Console.WriteLine($"id : {taskForm.workerId}, {topicForm.title}, {topicForm.description}, {taskForm.departmentId}");
                command.Parameters.Add(new SqlParameter("@category_id", form.id));
                command.Parameters.Add(new SqlParameter("@group_id", form.SelectedGroupId));

                await command.ExecuteNonQueryAsync();

           

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

using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models.ShareDesk;
using Darbu_records.Query.ShareDesk;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Darbu_records.SharedeskManagement
{
    public class ShareCancelService
    {
        IQueryService _queryService;
        IShareQueryBuilder _shareQueryBuilder;




        public ShareCancelService([FromKeyedServices("InstructionUpdateQueryService")] IQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task taskExecution(ShareForm shareForm, IShareQueryBuilder shareQueryBuilder)
        {
            //Seteris
            _shareQueryBuilder = shareQueryBuilder;

            //   Console.WriteLine($"share categoryja title: {shareForm.Title} department_name: {shareForm.Department_name} description {shareForm.Description} share_id {shareForm.Id} access {shareForm.Department_access} depart_id{shareForm.Department_id}");

            //////////////////////////////////////////////////////////////



            using (SqlConnection connection = _queryService.sqlConnGet())
            {


                await connection.OpenAsync();

                try
                {
                    // Console.WriteLine($"share categoryja title: {shareForm.Title} department_name: {shareForm.Department_name} description {shareForm.Description} share_id {shareForm.Id} access {shareForm.Department_access} depart_id{shareForm.Department_id} config {shareForm.Config_id}");
                    string query = _shareQueryBuilder.GetShareDesk();
                    //   Console.WriteLine(query );
                    using (SqlCommand command = new SqlCommand(cmdText: query, connection: connection))
                    {
                        // Console.WriteLine($"id : {taskForm.workerId}, {topicForm.title}, {topicForm.description}, {taskForm.departmentId}");
                        command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = shareForm.Id });
                        command.Parameters.Add(new SqlParameter("@department_id", SqlDbType.Int) { Value = shareForm.Department_id }); //Cia savo departmentid


                        await command.ExecuteNonQueryAsync();

                    }




                }
                catch (Exception ex)
                {

                   


                }

                await connection.CloseAsync();
            }






        }



    }
}


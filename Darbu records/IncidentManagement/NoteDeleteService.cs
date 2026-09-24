
using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.OldItems;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
namespace Darbu_records.IncidentManagement
{
    public class NoteDeleteService:INoteTaskService
    {

        IQueryService _queryService;



        public NoteDeleteService([FromKeyedServices("NoteDeleteQueryService")] IQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task taskExecution(TaskForm form)
        {

            using(SqlConnection connection = _queryService.sqlConnGet())
            {
                        await connection.OpenAsync();
                        using (SqlCommand comand = new SqlCommand(_queryService.queryActionGet(), connection))
                        {
                            int n = 0;
                            comand.Parameters.Add(new SqlParameter("@note_id", SqlDbType.Int) { Value=form.noteId});
                            comand.Parameters.Add(new SqlParameter("@status", SqlDbType.Int) { Value=n});


                           await comand.ExecuteNonQueryAsync();
                        }
                        await connection.CloseAsync();


            }
            







        }




    }

    
}

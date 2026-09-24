using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.OldItems;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Darbu_records.IncidentManagement
{
    public class NoteUpdateService:INoteTaskService
    {
        IQueryService _queryService;


        public NoteUpdateService([FromKeyedServices("NoteUpdateQueryService")]IQueryService queryService)
        {
            _queryService = queryService;
        }



        public async Task taskExecution(TaskForm form)
        {

            SqlConnection connection = _queryService.sqlConnGet();
            try
            {
               await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(_queryService.queryActionGet(), connection))
                {
                    command.Parameters.Add(new SqlParameter("@title", SqlDbType.VarChar) {Value= form.noteForm.title});
                    command.Parameters.Add(new SqlParameter("@description", SqlDbType.VarChar) { Value = form.noteForm.description});                  
                    command.Parameters.Add(new SqlParameter("@note_id", SqlDbType.Int) { Value = form.noteForm.noteId});
                   await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("klaida" + e.Message);
                throw;
                //tuscia, galima realizacija                 
            }
            finally
            {
               await connection.CloseAsync();
                connection.Dispose();
            }



        }




    }
}

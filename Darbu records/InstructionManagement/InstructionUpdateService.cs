using Darbu_records.Formos;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using Darbu_records.Interfaces.NoteService;
using System.Xml.Linq;
using Darbu_records.OldItems;

namespace Darbu_records.InstructionManagement
{
    public class InstructionUpdateService : INoteTaskService
    {

        IQueryService _instructionUpdateQueryService;



        public InstructionUpdateService([FromKeyedServices("InstructionUpdateQueryService")] IQueryService instructionUpdateQUeryService)
        {
            _instructionUpdateQueryService = instructionUpdateQUeryService;
        }

        public async Task taskExecution(TaskForm taskForm)
        {



            try
            {
                
                using (SqlConnection connection = _instructionUpdateQueryService.sqlConnGet())
                {
                    await connection.OpenAsync();
                 using (SqlCommand command = new SqlCommand(_instructionUpdateQueryService.queryActionGet(),connection))
                                {
                                    command.Parameters.Add(new SqlParameter("@name", taskForm.instructionForm.pavadinimas));
                                    command.Parameters.Add(new SqlParameter("@description", taskForm.instructionForm.aprasymas));
                                    command.Parameters.Add(new SqlParameter("@id", taskForm.instructionForm.iraso_id));
                                    await command.ExecuteNonQueryAsync();
                                }

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
                _instructionUpdateQueryService.sqlConnGet().Close();
            }
        }

    }
}

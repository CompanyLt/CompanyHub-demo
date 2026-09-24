using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.OldItems;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Darbu_records.SearchManagement
{


    public class InstructionSearchService: INoteTaskService
    {

        IQueryService queryService;

        public InstructionSearchService(IQueryService queryService)
        {
            this.queryService = queryService;
        }




        public async Task taskExecution(TaskForm taskForm)
        {
            if(string.IsNullOrEmpty(taskForm.searchText))
            {
                return;
            }
            
            try
            {
                await queryService.sqlConnGet().OpenAsync();
                using (SqlCommand command = new SqlCommand(queryService.queryActionGet(), queryService.sqlConnGet()))
                {

                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) {Value = queryService.idGet() } );
                    command.Parameters.Add(new SqlParameter("@status", SqlDbType.Int) { Value = 1 });
                    using (SqlDataReader reader =   await command.ExecuteReaderAsync())
                    {
                        string fileName = string.Empty;
                        while (reader.Read() == true)
                        {
                            fileName = reader.GetString(reader.GetOrdinal("title"));
                            if(fileName.Contains(taskForm.searchText,StringComparison.OrdinalIgnoreCase))
                            {
                              InstructionForm _instrukcijos_forma = new InstructionForm(
                               reader["title"].ToString(),
                               reader["description"].ToString(),
                               Convert.ToInt32(reader["instruction_id"])                              
                               );                                    
                               taskForm.instructionCollection.Add(_instrukcijos_forma);

                            }
                          

                          






                        }
                    }

                }

            }

            finally { queryService.sqlConnGet().CloseAsync(); }





        }

    }
    
}

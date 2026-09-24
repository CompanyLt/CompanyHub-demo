using Darbu_records.Formos;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using Darbu_records.Interfaces.NoteService;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Darbu_records.Models;
using Darbu_records.DAL;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Darbu_records.InstructionManagement
{
    public class InstructionReviewService : INoteTaskService
    {

        IQueryService _queryService;
        FileCommunicationService _fileCommunicationService;


        public InstructionReviewService([FromKeyedServices("InstructionReviewQueryService")]IQueryService queryService,FileCommunicationService fileCommunicationService)
        {
            _queryService = queryService;
            _fileCommunicationService = fileCommunicationService;
        }

        public async Task taskExecution(TaskForm taskForm)
        {


           
            using(SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(_queryService.queryActionGet(), connection))
                            {                  
                                command.Parameters.Add(new SqlParameter("@instruction_id", SqlDbType.Int) { Value = taskForm.noteId });
                                command.Parameters.Add(new SqlParameter("@status", SqlDbType.Int) { Value = 1 });



                                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                                {                   
                                   if(await reader.ReadAsync())
                                    {
                                          
                                            taskForm.instructionForm = new InstructionForm(
                                           reader["title"].ToString() ?? string.Empty,
                                           reader["description"]?.ToString() ?? string.Empty,
                                           reader["instruction_id"] != DBNull.Value ? Convert.ToInt32(reader["instruction_id"]) : 0,
                                           reader["likes_count"] != DBNull.Value ? Convert.ToInt32(reader["likes_count"]) : 0                                                             
                                            );

                                            if (reader["Photos"]!= DBNull.Value)
                                            {
                                                var jsonPath = reader["Photos"].ToString();
                                                taskForm.instructionForm.fileForm.photosCollection = JsonSerializer.Deserialize<List<RecordFile>>(jsonPath);


                                            }

                                            List<RecordFile> fileCollection = new();
                                            if (reader["Files"] != DBNull.Value)
                                            {
                                                var jsonPath = reader["Files"].ToString();
                                                 fileCollection = JsonSerializer.Deserialize<List<RecordFile>>(jsonPath);

                                            }
							//CIA reiktu ideti Failu rusiavima nes razor puseje rusiavima daryti nieko gero                          
                                        foreach(var n in fileCollection)
                                        {
                               
                                            if (_fileCommunicationService.checkPdfExtention(n.name)){
                                                taskForm.instructionForm.fileForm.pdfCollection.Add(n);

                                            }
                                            else
                                            {
									            taskForm.instructionForm.fileForm.filesCollection.Add(n);
								            }


                                        }
                                           



                                   }




                                }

                            }
                            await connection.CloseAsync();


                            if (taskForm.instructionForm == null) taskForm.instructionForm = new InstructionForm();
            }
            
        }

    }
}

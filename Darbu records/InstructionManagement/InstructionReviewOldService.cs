using Darbu_records.Formos;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using Darbu_records.Interfaces.NoteService;

namespace Darbu_records.InstructionManagement
{
    public class InstructionReviewOldService : INoteTaskService
    {

        IQueryService queryService;



        public InstructionReviewOldService(IQueryService queryService)
        {
            this.queryService = queryService;
        }

        public async Task taskExecution(TaskForm taskForm)
        {


            await queryService.sqlConnGet().OpenAsync();

            using (SqlCommand command = new SqlCommand(queryService.queryActionGet(), queryService.sqlConnGet()))
            {
                command.Parameters.Add(new SqlParameter("@instruction_id", SqlDbType.Int) { Value = queryService.idGet() });
                command.Parameters.Add(new SqlParameter("@status", SqlDbType.Int) { Value = 1 });



                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    int current_postid = -1;
                   while (await reader.ReadAsync())
                    {
                       
                        int post_id = Convert.ToInt32(reader["instruction_id"]);

                        if (current_postid != post_id)
                        {
                            
                            taskForm.instructionForm = new InstructionForm(
                           reader["title"].ToString() ?? string.Empty,
                           reader["description"]?.ToString() ?? string.Empty,
                           reader["instruction_id"] != DBNull.Value ? Convert.ToInt32(reader["instruction_id"]) : 0,
                           reader["Patinka"] != DBNull.Value ? Convert.ToInt32(reader["Patinka"]) : 0,
                           reader["instructionFileName"]?.ToString() ?? string.Empty
                            );

                            //taskForm.instructionForm.photos_initialize();
                            current_postid=post_id;

                        }

                        //cia sudeda viska i konteineri
                        if (taskForm.instructionForm != null && !reader.IsDBNull(reader.GetOrdinal("file_name")))
                        {
                            //   Photos photo = new Photos(reader["file_name"].ToString(), Convert.ToDateTime(reader["upload_date"]));
                            Photos photo = new Photos()
                            {
                                name = reader.GetString(reader.GetOrdinal("file_name")),
                                upload_date = reader.GetDateTime(reader.GetOrdinal("upload_date")),
                                directory = reader.GetString(reader.GetOrdinal("directory"))

                            };
                            //taskForm.instructionForm.photos.Add(photo);
                           
                        }



                    }




                }

            }
            await queryService.sqlConnGet().CloseAsync();


            if (taskForm.instructionForm == null) taskForm.instructionForm = new InstructionForm();
        }

    }
}

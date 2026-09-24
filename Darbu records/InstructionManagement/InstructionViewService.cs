using Darbu_records.Formos;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.OldItems;

namespace Darbu_records.InstructionManagement
{
    public class InstructionViewService : INoteTaskService
    {

        IQueryService _queryService;



        public InstructionViewService([FromKeyedServices("InstructionViewQueryService")]IQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task taskExecution(TaskForm taskForm)
        {




            taskForm.instructionCollection = new();
            using(SqlConnection connection = _queryService.sqlConnGet())
            {
              await  connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(_queryService.queryActionGet(), connection))
                            {
                                command.Parameters.Add(new SqlParameter("@category", taskForm.categoryId));
                                command.Parameters.Add(new SqlParameter("@department_id", taskForm.departmentId));
                                command.Parameters.Add(new SqlParameter("@department_access", taskForm.department_access));
                                command.Parameters.Add(new SqlParameter("@status", 1));

                                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                                {

                                    while (await reader.ReadAsync())
                                    {

                            InstructionForm _instrukcijos_forma = new InstructionForm()
                            {
                                pavadinimas = reader["title"].ToString(),
                                aprasymas = reader["description"].ToString(),
                                iraso_id = Convert.ToInt32(reader["id"]),
                                department_access = Convert.ToInt32(reader["department_access"]),
                                categoryId = Convert.ToInt32(reader["category_id"])
                            };
                                           
                                          

                                        taskForm.instructionCollection.Add(_instrukcijos_forma);
                                        //  Console.WriteLine(reader["List"].ToString());



               
                                    }



                                }


                }
                            await connection.CloseAsync();

            }
            






        }

    }
}

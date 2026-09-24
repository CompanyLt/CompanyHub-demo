using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using static DevExpress.Utils.SafeXml;

namespace Darbu_records.IncidentManagement
{
    public class NoteViewService:INoteTaskService
    {
       
        IQueryService _queryService;
       


       public NoteViewService([FromKeyedServices("NoteViewQueryService")] IQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task taskExecution(TaskForm taskForm)
        {
          //cashe realizacija
            //string casheKey = $"incident_{taskForm.category}";

            //if (_memoryCashe.TryGetValue(casheKey, out List<IncidentForm> incidentCollection))
            //{

            //    taskForm.incidentCollection = incidentCollection;
            //    return;
            //}

            taskForm.noteCollection = new List<NoteForm>();
            using (SqlConnection connection =  _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(_queryService.queryActionGet(), connection))
                {
                  
                    //noteId tai perduotas vartotojo id, kol kas nerandu sprendimo
                    command.Parameters.Add(new SqlParameter("@created_by", taskForm.noteId));
                    command.Parameters.Add(new SqlParameter("@category", taskForm.categoryId));
                    command.Parameters.Add(new SqlParameter("@status", 1));
                    command.Parameters.Add(new SqlParameter("@department_id", taskForm.departmentId));
                    command.Parameters.Add(new SqlParameter("@department_access", taskForm.department_access));

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
#nullable disable

                        while (await reader.ReadAsync())
                        {                          
                            // Console.WriteLine("sdsd");
                            NoteForm noteForm = new NoteForm()
                            {
                                title = reader["title"].ToString(),
                                description = reader["description"].ToString(),
                                noteId = Convert.ToInt32(reader["id"].ToString()),
                                categoryId = Convert.ToInt32(reader["category_id"].ToString()),
                                department_access = Convert.ToInt32(reader["department_access"].ToString())

                            };
                                

                            taskForm.noteCollection.Add(noteForm);
                            //  Console.WriteLine(reader["List"].ToString());




                        }
#nullable enable


                    }


                }


            await connection.CloseAsync();
            }
           
           
            //var cacheOptions = new MemoryCacheEntryOptions()
            //.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

            //_memoryCashe.Set(casheKey, taskForm.incidentCollection, cacheOptions);
        }


      





    }

   
}

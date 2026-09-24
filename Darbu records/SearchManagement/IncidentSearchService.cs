using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.OldItems;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Darbu_records.SearchManagement
{


    public class IncidentSearchService: INoteTaskService
    {

        IQueryService queryService;

        public IncidentSearchService(IQueryService queryService)
        {
            this.queryService = queryService;
        }




       public async Task taskExecution(TaskForm taskForm)
        {
            if (string.IsNullOrEmpty(taskForm.searchText))
            {
                return;
            }

            try
            {
              await  queryService.sqlConnGet().OpenAsync();
                using (SqlCommand command = new SqlCommand(queryService.queryActionGet(), queryService.sqlConnGet()))
                {
                    int status = 1;
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = queryService.idGet() });
                    command.Parameters.Add(new SqlParameter("@status", SqlDbType.Int) {Value = 1 });
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        string fileName = string.Empty;
                        while (await reader.ReadAsync() == true)
                        {
                            fileName = reader.GetString(reader.GetOrdinal("List"));
                            if (fileName.Contains(value: taskForm.searchText, StringComparison.OrdinalIgnoreCase))
                            {
                                IncidentForm Gedimo_forma = new IncidentForm(
                                reader["List"].ToString(),
                                reader["Description"].ToString(),
                                Convert.ToInt32(reader["Record_Id"]),
                                reader["Solution"].ToString()
                              );
                                taskForm.incidentCollection.Add(Gedimo_forma);

                            }
                            //                            int index = 0;
                            //#nullable disable
                            //                            foreach (var n in reader["List"].ToString())
                            //                            {

                            //                                if (taskForm.searchText[index] == n && taskForm.searchText.Length >= 3)
                            //                                {

                            //                                    index += 1;
                            //                                }
                            //                                else
                            //                                {

                            //                                    index = 0;
                            //                                }
                            //                                if (index == 3)
                            //                                {

                            //                                    IncidentForm Gedimo_forma = new IncidentForm(
                            //                                        reader["List"].ToString(),
                            //                                reader["Description"].ToString(),
                            //                                Convert.ToInt32(reader["Record_Id"]),
                            //                                reader["Solution"].ToString()
                            //                              );
                            //                                    taskForm.incidentCollection.Add(Gedimo_forma);
                            //#nullable enable
                            //                                    break;
                            //                                }

                            //                            }






                        }
                    }

                }

            }

            finally { 
                
               await queryService.sqlConnGet().CloseAsync(); 
            
            }






        }

    }
}

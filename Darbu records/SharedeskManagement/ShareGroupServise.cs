using Darbu_records.DAL;
using Darbu_records.Formos;
using Darbu_records.Interfaces;
using Darbu_records.Models.Topic;
using Darbu_records.Models;
using Darbu_records.Query.Topic;
using Microsoft.Data.SqlClient;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Query.ShareDesk;
using Darbu_records.Models.ShareDesk;
using System.Text.RegularExpressions;
using System.Data;

namespace Darbu_records.SharedeskManagement
{
    public class ShareGroupServise
    {

        IQueryService _queryService;      
        IShareQueryBuilder _shareQueryBuilder;
      



        public ShareGroupServise([FromKeyedServices("InstructionUpdateQueryService")] IQueryService queryService)
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
                using (SqlTransaction transaction = connection.BeginTransaction())
                {

                    try
                    {
                       // Console.WriteLine($"share categoryja title: {shareForm.Title} department_name: {shareForm.Department_name} description {shareForm.Description} share_id {shareForm.Id} access {shareForm.Department_access} depart_id{shareForm.Department_id} config {shareForm.Config_id}");
                        string query = _shareQueryBuilder.GetShareDesk();
                     //   Console.WriteLine(query );
                        using (SqlCommand command = new SqlCommand(cmdText: query, connection: connection,transaction:transaction))
                        {
                            // Console.WriteLine($"id : {taskForm.workerId}, {topicForm.title}, {topicForm.description}, {taskForm.departmentId}");
                            command.Parameters.Add(new SqlParameter("@title", SqlDbType.NVarChar) { Value = shareForm.Title });
                            command.Parameters.Add(new SqlParameter("@description", shareForm.Description));
                            command.Parameters.Add(new SqlParameter("@share_id", shareForm.Id));
                            command.Parameters.Add(new SqlParameter("@config_id", shareForm.Config_id));
                            command.Parameters.Add(new SqlParameter("@created_by", shareForm.Created_by));
                            command.Parameters.Add(new SqlParameter("@department_id", shareForm.Department_access)); //Cia kam
                            command.Parameters.Add(new SqlParameter("@department_access", shareForm.Department_id)); //Cia savo departmentid
                            command.Parameters.Add(new SqlParameter("@department", shareForm.Department_name));

                            int shareId = Convert.ToInt32(await command.ExecuteScalarAsync());

                          //  Console.WriteLine($"share categoryja title: {shareForm.Title} department_name: {shareForm.Department_name} description {shareForm.Description} share_id {shareForm.Id} access {shareForm.Department_access} depart_id{shareForm.Department_id}");


                            string shareAccessQuery = _shareQueryBuilder.GetShareDeskAccess();
                           // Console.WriteLine($"ar veikia{shareAccessQuery}");

                            using (SqlCommand categoryCmd = new SqlCommand(shareAccessQuery, connection, transaction))
                            {
                               
                        
                                categoryCmd.Parameters.AddWithValue("@department_access", shareForm.Department_id);
                                categoryCmd.Parameters.AddWithValue("@department_id", shareForm.Department_access);
                                categoryCmd.Parameters.AddWithValue("@share_id", shareId);
                                categoryCmd.Parameters.AddWithValue("@group_id", shareForm.Id);

                                await categoryCmd.ExecuteNonQueryAsync();
                            }



                        }


                     //   Console.WriteLine("Transaction sėkmingai patvirtinta.");
                      await  transaction.CommitAsync();
                    }
                    catch(Exception ex)
                    {


                      //  Console.WriteLine("Transaction nesėkmingai."+ex.Message);
                     await   transaction.RollbackAsync();
                    }

                   




                }
               

                         


                await connection.CloseAsync();
            }






        }



    }






}


using Darbu_records.DAL;
using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Darbu_records.Models.Topic;
using Darbu_records.Query.Topic;
using DevExpress.Xpo.DB.Helpers;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace Darbu_records.CommentManagement
{
    public class CommentLoadService
    {

        IQueryService _queryService;
        CommentConnectionService _connectionService;
        FileCommunicationService _fileCommunicationService;
       public CommentLoadService([FromKeyedServices("CommentConnectionService")]IQueryService queryService,CommentConnectionService connectionService,FileCommunicationService fileCommunicationService)
        {
            _queryService = queryService;
            _connectionService = connectionService;
            _fileCommunicationService = fileCommunicationService;
        }






        public async Task<List<Comment>> GetComments(string tableName,long topic_id)
        {
            //GAUNAME QUERY siunciame pvz "departmentComment" ir ten jau viduje pagla tai switch pagalba paimam
            string query = _connectionService.GetQuery(tableName);

           // Console.WriteLine($"{topic_id}");
           //Console.WriteLine(query);


            _queryService.queryActionSet(query);
            List<Comment> commentsCollection = new List<Comment>();
       
            using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add(new SqlParameter("@comment_id", SqlDbType.Int) { Value = topic_id});
                  



                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {

                            Comment comment = new Comment()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                topic_id = Convert.ToInt32(reader["comment_id"]),
                                text = reader["text"].ToString(),
                                created_at = Convert.ToDateTime(reader["created_at"]),
                                created_by = Convert.ToInt32(reader["created_by"])
                            };


                            //json failiukas is sql
                            if (reader["Photos"] != DBNull.Value)
                            {
                                var jsonPath = reader["Photos"].ToString();
                                comment.fileForm.photosCollection = JsonSerializer.Deserialize<List<RecordFile>>(jsonPath);


                            }

                            List<RecordFile> fileCollection = new();
                            if (reader["Files"] != DBNull.Value)
                            {
                                var jsonPath = reader["Files"].ToString();
                                fileCollection = JsonSerializer.Deserialize<List<RecordFile>>(jsonPath);

                            }
                            //CIA reiktu ideti Failu rusiavima nes razor puseje rusiavima daryti nieko gero                          
                            foreach (var n in fileCollection)
                            {
                                if (_fileCommunicationService.checkPdfExtention(n.name))
                                {
                                    comment.fileForm.pdfCollection.Add(n);

                                }
                                else
                                {
                                    comment.fileForm.filesCollection.Add(n);
                                }
                            }
                            commentsCollection.Add(comment);
                        }
                    }
                }
                await connection.CloseAsync();


              
            }




         
         return commentsCollection;
        }


    }
}

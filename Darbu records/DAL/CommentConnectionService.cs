using DevExpress.Xpo.DB.Helpers;

namespace Darbu_records.DAL
{
    public class CommentConnectionService
    {



        string query1 = $@"SELECT                           
                            N.created_by,
                            N.created_at,                           
                            N.id,
                            N.text,
                            N.comment_id,
                            (
                                SELECT 
                                    PI.file_name AS fileName,
                                    PI.upload_date AS fileDate,
                                    PI.directory AS fileDirectory,
                                    PI.original_name AS originalName
                                FROM DBoardCommentPhotos AS PI
                                WHERE N.id = PI.comment_id
                                FOR JSON PATH
                            ) AS Photos,

                            (
                                SELECT 
                                    IFL.file_name AS fileName,
                                    IFL.upload_date AS fileDate,
                                    IFL.directory AS fileDirectory,
                                    IFL.original_name AS originalName
                                FROM DBoardCommentFiles AS IFL
                                WHERE N.id = IFL.comment_id
                                FOR JSON PATH
                            ) AS Files

                        FROM DepartmentBoardComment AS N
                        WHERE N.comment_id = @comment_id";




        public string GetQuery(string key)
        {
            string query = key switch
            {
                "DepartmentBoardComment" => query1,
                "IncidentComment" => "SELECT * FROM IncidentComment ORDER BY created_at DESC",
                _ => throw new ArgumentException("Unknown table")
            };


            return query;
        }




    }
}

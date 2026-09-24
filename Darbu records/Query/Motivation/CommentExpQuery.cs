using Darbu_records.Enums.ShareDesk;
using Darbu_records.Enums.Topics;

namespace Darbu_records.Query.Motivation
{
    public class CommentExpQuery
    {
        string mainQuery = string.Empty;

        string accessQuery = string.Empty;




        public void SetShareDesk(ShareDeskTable shareDeskTable)
        {

                mainQuery = $@"    SELECT 
                                db.comment_count - ISNULL(rc.read_count, 0) AS NewComments
                            FROM DepartmentBoard db
                            LEFT JOIN DBoard_read_comments rc
                                ON rc.topic_id = db.Id
                                AND rc.worker_id = @WorkerId
                            WHERE db.Id = @TopicId;                  
                    ";


        }


        public void SetShareAccess(TopicCategoryAccessTable categoryAccessTable)
        {

            accessQuery = $@"                                           
                       INSERT INTO {categoryAccessTable} (
                            category_id,
                            department_id,
                            department_access,
                            share_id
                        )
                        SELECT
                            C.id,
                            @department_assign,
                            @department_access,
                            @share_id
                        FROM Category C
                        WHERE C.group_id = @group_id
                        AND EXISTS (
                            SELECT 1
                            FROM {categoryAccessTable} CA
                            WHERE CA.category_id = C.id
                            AND CA.department_id = @department_id
                        )
                        AND NOT EXISTS (
                            SELECT 1
                            FROM {categoryAccessTable} CA2
                            WHERE CA2.category_id = C.id
                            AND CA2.department_id = @department_assign
                            );";




        }






        ////////////////
        public string GetShareDesk()
        {




            return mainQuery ?? string.Empty;
        }

        public string GetShareAccess()
        {




            return accessQuery ?? string.Empty;
        }



    }
}

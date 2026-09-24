using Darbu_records.Enums.ShareDesk;
using Darbu_records.Enums.Topics;

namespace Darbu_records.Query.ShareDesk
{
    public class ShareCategoryQuery
    {

        string mainQuery = string.Empty;

        string accessQuery = string.Empty;




        public void SetShareDesk(ShareDeskTable shareDeskTable)
        {

            mainQuery = $@"                      
                      
                    INSERT INTO {shareDeskTable} (title,description,share_id,config_id,created_by,department_id, department_access,created_at, department)
                    VALUES (@title,@description,@share_id,@config_id,@created_by,@department_id, @department_access, GETDATE(),@department);
                    SELECT SCOPE_IDENTITY()";


        }


        public void SetShareAccess(TopicCategoryAccessTable categoryAccessTable)
        {

            accessQuery = $@"INSERT INTO {categoryAccessTable} (
                category_id,
                department_id,
                department_access,
                share_id
            )
            SELECT
                @group_id,
                @department_assign,
                @department_access,
                @share_id
            WHERE EXISTS (
                SELECT 1
                FROM {categoryAccessTable} CA
                WHERE CA.category_id = @group_id
                AND CA.department_id = @department_id
            )
            AND NOT EXISTS (
                SELECT 1
                FROM {categoryAccessTable} CA2
                WHERE CA2.category_id = @group_id
                AND CA2.department_id = @department_assign
            );
            ";
         

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

using Darbu_records.Enums;
using Darbu_records.Enums.ShareDesk;
using Darbu_records.Enums.Topics;

namespace Darbu_records.Query.ShareDesk
{
    public class ShareGroupQuery
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


        public void SetShareAccess(GroupAccessTable groupAccessTable)
        {

            accessQuery = $@" 
                        INSERT INTO {groupAccessTable} (
                        group_id,
                        department_id,
                        department_access,
                        share_id
                    )
                    VALUES (
                        @group_id,        
                        @department_id,
                        @department_access,
                        @share_id
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


using Darbu_records.Enums.ShareDesk;
using Darbu_records.Enums.Topics;

namespace Darbu_records.Query.ShareDesk
{
    public class ShareCancelQuery
    {

        string mainQuery = string.Empty;

        string accessQuery = string.Empty;

        string cancelQuery = string.Empty;


        public void SetShareDesk(ShareDeskTable shareDeskTable)
        {

            mainQuery = $@"                      
                      
                    DELETE FROM {shareDeskTable} WHERE id=@id AND (department_id = @department_id OR department_access = @department_id);";


        }


        public void SetShareAccess(TopicCategoryAccessTable categoryAccessTable)
        {

            accessQuery = $@" ";




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

        public string GetShareCancel()
        {
            return cancelQuery ?? string.Empty;
        }

    }
}

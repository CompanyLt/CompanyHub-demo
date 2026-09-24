using Darbu_records.Enums.ShareDesk;
using Darbu_records.Enums.Topics;

namespace Darbu_records.Query.ShareDesk
{
    public class ShareCategoryQueryBuilder:IShareQueryBuilder
    {

        ShareCategoryQuery _mainQuery;


        public ShareCategoryQueryBuilder(ShareCategoryQuery mainQuery)
        {
            _mainQuery = mainQuery;
        }



        public void SetQuery()
        {
            _mainQuery.SetShareDesk(ShareDeskTable.ShareDesk);
            _mainQuery.SetShareAccess(TopicCategoryAccessTable.CategoryAccess);
        }




        public string GetShareDesk()
            => _mainQuery.GetShareDesk();


        public string GetShareDeskAccess()
            => _mainQuery.GetShareAccess();

    }
}

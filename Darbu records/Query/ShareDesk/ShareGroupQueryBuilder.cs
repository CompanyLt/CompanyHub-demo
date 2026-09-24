using Darbu_records.Enums;
using Darbu_records.Enums.ShareDesk;
using Darbu_records.Enums.Topics;

namespace Darbu_records.Query.ShareDesk
{
    public class ShareGroupQueryBuilder : IShareQueryBuilder
    {


        ShareGroupQuery _shareGroupQuery;


        public ShareGroupQueryBuilder(ShareGroupQuery shareGroupQuery)
        {
            _shareGroupQuery = shareGroupQuery;
        }



        public void SetQuery()
        {
            _shareGroupQuery.SetShareDesk(ShareDeskTable.ShareDesk);
            _shareGroupQuery.SetShareAccess(GroupAccessTable.GroupAccess);
        }




        public string GetShareDesk()
            => _shareGroupQuery.GetShareDesk();
           
       
        public string GetShareDeskAccess()
            => _shareGroupQuery.GetShareAccess();
         
     
    }
}

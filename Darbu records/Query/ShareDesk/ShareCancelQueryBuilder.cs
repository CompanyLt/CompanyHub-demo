using Darbu_records.Enums.ShareDesk;
using Darbu_records.Enums.Topics;

namespace Darbu_records.Query.ShareDesk
{
    public class ShareCancelQueryBuilder:IShareQueryBuilder
    {
        ShareCancelQuery _mainQuery;


        public ShareCancelQueryBuilder(ShareCancelQuery mainQuery)
        {
            _mainQuery = mainQuery;
        }



        public void SetQuery()
        {
            _mainQuery.SetShareDesk(ShareDeskTable.ShareDesk);          
        }




        public string GetShareDesk()
            => _mainQuery.GetShareDesk();


        public string GetShareDeskAccess()
            => _mainQuery.GetShareAccess();




    }
}

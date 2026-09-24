namespace Darbu_records.Query.Group
{
    public class GroupToCategoryQueryBuilder:IGroupQueryBuilder
    {

        private readonly GroupToCategoryQuery _query;
        public GroupToCategoryQueryBuilder(GroupToCategoryQuery query)
        {
            _query = query;
        }

        public string GetAccessQuery()
        {
            return _query.GetAccessQuery();
        }

        public string GetFileQuery()
        {
            return _query.GetFileQuery();
        }

        public string GetMainQuery()
        {
            return _query.GetMainQuery();
        }

        public void SetQuery()
        {
            _query.SetMainQuery();
          //  _query.SetAccessQuery();
           // _query.SetFileQuery();
        }





    }
}

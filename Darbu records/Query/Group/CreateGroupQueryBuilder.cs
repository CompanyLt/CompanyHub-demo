namespace Darbu_records.Query.Group
{
    public class CreateGroupQueryBuilder : IGroupQueryBuilder
    {
        private readonly CreateGroupQuery _query;
        public CreateGroupQueryBuilder(CreateGroupQuery query)
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
            _query.SetAccessQuery();
            _query.SetFileQuery();
        }
    }
}

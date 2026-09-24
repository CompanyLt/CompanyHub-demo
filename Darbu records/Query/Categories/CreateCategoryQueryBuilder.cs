using Darbu_records.Query.Group;

namespace Darbu_records.Query.Categories
{
    public class CreateCategoryQueryBuilder:ICategoryQueryBuilder
    {
        private readonly CreateCategoryQuery _query;
        public CreateCategoryQueryBuilder(CreateCategoryQuery query)
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

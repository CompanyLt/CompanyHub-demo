namespace Darbu_records.Query.Categories
{
    public class CategoryToGroupQueryBuilder : ICategoryQueryBuilder
    {

       private readonly CategoryToGroupQuery _categoryToGroupQuery;


        public CategoryToGroupQueryBuilder(CategoryToGroupQuery categoryToGroupQuery)
        {
            _categoryToGroupQuery = categoryToGroupQuery;
        }


        public string GetAccessQuery()
        {
           return _categoryToGroupQuery.GetAccessQuery();
        }

        public string GetFileQuery()
        {
           return _categoryToGroupQuery.GetFileQuery();
        }

        public string GetMainQuery()
        {
            return _categoryToGroupQuery.GetMainQuery();
        }

        public void SetQuery()
        {
            _categoryToGroupQuery.SetMainQuery();
            _categoryToGroupQuery.SetAccessQuery();
        }
    }
}

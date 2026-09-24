namespace Darbu_records.Query.Categories
{
    public interface ICategoryQueryBuilder
    {
        void SetQuery();

        string GetMainQuery();



        string GetAccessQuery();

        string GetFileQuery();
    }
}

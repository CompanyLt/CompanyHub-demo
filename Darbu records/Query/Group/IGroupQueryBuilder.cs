namespace Darbu_records.Query.Group
{
    public interface IGroupQueryBuilder
    {

        void SetQuery();

        string GetMainQuery();



        string GetAccessQuery();

        string GetFileQuery();
    }
}

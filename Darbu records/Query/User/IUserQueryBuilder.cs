namespace Darbu_records.Query.User
{
    public interface IUserQueryBuilder
    {

        void SetQuery();
        string GetMainQuery();

        string GetAchievmentQuery();

        string GetFilesQuery();






    }
}

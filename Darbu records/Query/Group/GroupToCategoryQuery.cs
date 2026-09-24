namespace Darbu_records.Query.Group
{
    public class GroupToCategoryQuery
    {

        string mainQuery = string.Empty;
        string accessQuery = string.Empty;
        string fileQuery = string.Empty;
        public void SetMainQuery()
        {

           // mainQuery = $@"DELETE FROM CategoryAccess WHERE category_id=@category_id";

            mainQuery = $@"UPDATE Category SET group_id=@group_id WHERE id=@category_id";


        }



        public void SetAccessQuery()
        {

            accessQuery = $@"INSERT INTO CategoryAccess(category_id,department_id,department_access,group_id) VALUES(@category_id,@department_id,@department_access,@group_id)";



        }



        public void SetFileQuery()
        {
            fileQuery = $@"";
        }

        public string GetMainQuery() =>
           mainQuery;



        public string GetAccessQuery() =>
            accessQuery;


        public string GetFileQuery() =>
            fileQuery;
    }
}

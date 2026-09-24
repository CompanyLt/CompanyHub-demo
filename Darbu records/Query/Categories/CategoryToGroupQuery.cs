namespace Darbu_records.Query.Categories
{
    public class CategoryToGroupQuery
    {
        string mainQuery = string.Empty;
        string accessQuery = string.Empty;        
        string fileQuery = string.Empty;





        public void SetMainQuery()
        {

            //   mainQuery = $@"DELETE FROM CategoryAccess WHERE group_id=@group_id";
            mainQuery = $@"UPDATE Category SET group_id = NULL WHERE group_id = @group_id";


         

        }



        public void SetAccessQuery()
        {

            accessQuery = $@"UPDATE Category SET group_id = @group_id WHERE id=@category_id";



        }

       

        public void SetFileQuery()
        {
            fileQuery = $@"";
        }




        ////////////////
        public string GetMainQuery() =>
            mainQuery;



        public string GetAccessQuery() =>
            accessQuery;

       
        public string GetFileQuery() =>
            fileQuery;

    }
}

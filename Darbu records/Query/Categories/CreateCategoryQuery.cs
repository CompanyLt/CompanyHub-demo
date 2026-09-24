namespace Darbu_records.Query.Categories
{
    public class CreateCategoryQuery
    {
        string mainQuery = string.Empty;
        string accessQuery = string.Empty;
        string fileQuery = string.Empty;





        public void SetMainQuery()
        {

            mainQuery = $@"INSERT INTO Category
                            (                              
                                category_name,
                                status,
                                created_by,                                                           
                                created_at,
                                DepartmentId
                              
                            )
                            OUTPUT INSERTED.id
                            VALUES
                            (                              
                                @category_name,
                                @status,
                                @created_by,                            
                                GETDATE(),
                                @department_id
                            );                         
                    ";




        }



        public void SetAccessQuery()
        {

            accessQuery = $@"INSERT INTO CategoryAccess(category_id,department_id,department_access) VALUES(@category_id,@department_id,@department_access)";



        }

        public void SetFileQuery()
        {
            fileQuery = $@"INSERT INTO CategoryFiles(category_id,file_name,upload_date,directory,original_name) VALUES(@category_id,@file_name,GETDATE(),@directory,@original_name)";
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

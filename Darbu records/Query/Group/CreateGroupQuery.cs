using Darbu_records.Enums.Worker;

namespace Darbu_records.Query.Group
{
    public class CreateGroupQuery
    {
        string mainQuery = string.Empty;
        string accessQuery = string.Empty;
        string fileQuery = string.Empty;





        public void SetMainQuery()
        {

            mainQuery = $@"INSERT INTO GroupCategory
                            (                              
                                group_name,
                                status,
                                created_by,                                                           
                                created_at
                              
                            )
                            OUTPUT INSERTED.id
                            VALUES
                            (                              
                                @group_name,
                                @status,
                                @created_by,                              
                                GETDATE()                                
                            );                         
                    ";




        }



        public void SetAccessQuery()
        {

            accessQuery = $@"INSERT INTO GroupAccess(group_id,department_id,department_access) VALUES(@group_id,@department_id,@department_access)";



        }

        public void SetFileQuery()
        {          
            fileQuery = $@"INSERT INTO GroupFiles(group_id,file_name,upload_date,directory,original_name) VALUES(@group_id,@file_name,GETDATE(),@directory,@original_name)";
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

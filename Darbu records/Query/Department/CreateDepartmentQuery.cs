using Darbu_records.Enums.Topics;

namespace Darbu_records.Query.Department
{
    public class CreateDepartmentQuery
    {
     



            string mainQuery = string.Empty;        
            string accessQuery = string.Empty;
            string fileQuery = string.Empty;





            public void SetMainTopic()
            {

                mainQuery = $@"INSERT INTO Departments(Name,avatar,created_by) OUTPUT INSERTED.id VALUES(@name,@avatar,@created_by)";




            }



            public void SetNavigationActionAccess()
            {
               
                accessQuery = $@"INSERT INTO NavigationActionAccess(action_id,department_id) VALUES(@action_id,@department_id)";



            }



                public void SetFileQuery()
                {
                    fileQuery = $@"INSERT INTO DepartmentsFiles(department_id,file_name,upload_date,directory,original_name) VALUES(@department_id,@file_name,GETDATE(),@directory,@original_name)";
                }


        ////////////////
        public string GetMainTopic()
            {




                return mainQuery;
            }


            public string GetNavigationActionAccess()
            {





                return accessQuery;
            }

        public string GetFileQuery()
        {

            return fileQuery;
        }

           

    }
}

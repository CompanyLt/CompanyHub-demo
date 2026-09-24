using Darbu_records.Enums.Worker;

namespace Darbu_records.Query.User
{
    public class CreateWorkerQuery
    {

        string mainQuery = string.Empty;

        string achievmentQuery = string.Empty;


        string workerFilesQuery = string.Empty;

        public void SetMainQuery(WorkerTable workerTable)
        {

            mainQuery = $@"INSERT INTO {workerTable}
                            (                              
                                name,
                                surname,
                                jobTitle,                                                                                                               
                                DepartmentId,
                                role_id,
                                created_at
                              
                            )
                            OUTPUT INSERTED.id
                            VALUES
                            (                              
                                @name,
                                @surname,
                                @jobTitle,                                                                                                                     
                                @department_id,
                                @role_id,
                                GETDATE()
                                
                            );                         
                    ";


        }


        public void SetAchievmentQuery(WorkerTable workerTable)
        {

            achievmentQuery = $@"     ";




        }


        public void SetFilesQuery()
        {
            workerFilesQuery = $@"INSERT INTO WorkerFiles(worker_id,file_name,upload_date,directory,original_name) VALUES(@worker_id,@file_name,GETDATE(),@directory,@original_name)";
        }






        ////////////////
        public string GetViewQuery()
        {




            return mainQuery;
        }

        public string GetAchievmentQuery()
        {




            return achievmentQuery;
        }

        public string GetWorkerFilesQuery()
        {
            return workerFilesQuery;
        }



    }
}

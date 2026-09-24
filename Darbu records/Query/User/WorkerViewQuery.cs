
using Darbu_records.Enums.Worker;

namespace Darbu_records.Query.User
{
    public class WorkerViewQuery
    {

        string mainQuery = string.Empty;

        string achievmentQuery = string.Empty;

        string fileQuery = string.Empty;


        public void SetMainQuery(WorkerTable workerTable)
        {

            mainQuery = $@"
                            SELECT
                                w.*,
                                (
                                    SELECT
                                        WF.file_name AS fileName,
                                        WF.upload_date AS fileDate,
                                        WF.directory AS fileDirectory,
                                        WF.original_name AS originalName
                                    FROM WorkerFiles AS WF
                                    WHERE WF.worker_id = w.id
                                    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                                ) AS Files
                            FROM {workerTable} AS w
                            WHERE w.DepartmentId = @department_id
                        ";


        }


        public void SetAchievmentQuery(WorkerTable workerTable)
        {

            achievmentQuery = $@"     ";




        }

        public void SetFilesQuery()
        {
            fileQuery = $@"INSERT INTO WorkerFiles(worker_id,file_name,upload_date,directory,original_name) VALUES(@worker_id,@file_name,GETDATE(),@directory,@original_name)";
        }






        ////////////////
        public string GetMainQuery()
        {




            return mainQuery;
        }

        public string GetAchievmentQuery()
        {




            return achievmentQuery;
        }


       public string GetFilesQuery()
        {
            return fileQuery;
        }






    }
}

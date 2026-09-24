using Darbu_records.Enums.Worker;

namespace Darbu_records.Query.User
{
    public class WorkerViewQueryBuilder:IUserQueryBuilder
    {

        WorkerViewQuery _workerViewQuery;

       public WorkerViewQueryBuilder(WorkerViewQuery workerViewQuery)
        {
            _workerViewQuery = workerViewQuery;
        }




        public void SetQuery()
        {
            _workerViewQuery.SetMainQuery(WorkerTable.worker);
            _workerViewQuery.SetFilesQuery();

        }



        public string GetMainQuery()=>
            _workerViewQuery.GetMainQuery();
       

        public string GetAchievmentQuery()=>
            _workerViewQuery.GetAchievmentQuery();

        public string GetFilesQuery()=>
            _workerViewQuery.GetFilesQuery();

    }
}

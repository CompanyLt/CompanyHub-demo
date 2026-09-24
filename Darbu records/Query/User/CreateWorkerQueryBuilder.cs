using Darbu_records.Enums.Worker;

namespace Darbu_records.Query.User
{
    public class CreateWorkerQueryBuilder:IUserQueryBuilder
    {

        CreateWorkerQuery _createWorkerQuery {  get; set; }

        public CreateWorkerQueryBuilder(CreateWorkerQuery createWorkerQuery)
        {
            _createWorkerQuery = createWorkerQuery;
        }




        public void SetQuery()
        {
            _createWorkerQuery.SetMainQuery(WorkerTable.worker);
            _createWorkerQuery.SetFilesQuery();

        }



        public string GetMainQuery() =>
            _createWorkerQuery.GetViewQuery();


        public string GetAchievmentQuery() =>
            _createWorkerQuery.GetAchievmentQuery();


        public string GetFilesQuery() =>
            _createWorkerQuery.GetWorkerFilesQuery();
    }
}

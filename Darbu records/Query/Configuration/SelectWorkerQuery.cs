using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Microsoft.Data.SqlClient;

namespace Darbu_records.Query.Configuration
{
    public class SelectWorkerQuery:IQueryService
    {
        IConnectionRepository _connectionRepository;


        private string queryAction = @"SELECT
                                    W.id,
                                    W.name,
                                    W.surname,
                                    W.jobTitle,                                   
                                    W.DepartmentId,
                                    D.Name as Department,
                                    W.progress,
                                    W.expierence,
                                    R.role_name,
                                    R.id AS role_id
                                    FROM worker AS W
                                    INNER JOIN Role AS R
                                    on W.role_id=R.id
                                    INNER JOIN  Departments AS D
                                    on W.DepartmentId=D.Id
                                    WHERE W.id=@workerId AND W.status=@status;

                                    ";
        public SelectWorkerQuery(IConnectionRepository connectionRepository)
        {
            _connectionRepository = connectionRepository;
        }





        public string dbConnGet()
        {
            throw new NotImplementedException();
        }

        public string dbQueryGet()
        {
            throw new NotImplementedException();
        }



        public string queryActionGet()
        {
            return queryAction;
        }

        public void queryActionSet(string query)
        {
            //  queryAction = query;
            queryAction = "SELECT AchievmentId,Name,Description,Image FROM Achievment WHERE WorkerId=@workerId";
        }

        public SqlConnection sqlConnGet()
        {
            return _connectionRepository.getSqlConnection();
        }

        public int idGet()
        {
            return 0;

        }
    }
}


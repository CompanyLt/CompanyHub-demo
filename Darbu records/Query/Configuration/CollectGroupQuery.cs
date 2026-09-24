using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Microsoft.Data.SqlClient;

namespace Darbu_records.Query.Configuration
{
    public class CollectGroupQuery : IQueryService
    {
        IConnectionRepository _connectionRepository;


        private string queryAction2 = "SELECT id, group_name FROM GroupCategory WHERE DepartmentsId=@departmentId and department_parameter=@department_parameter AND status=@status";

        string queryAction = $@"SELECT 
             gca.group_id,
             gc.group_name
            
            FROM GroupCategory gc
            INNER JOIN GroupCategoryAssign gca
            ON gc.id = gca.group_id
            WHERE gca.department_id =@departmentId AND gca.department_parameter=@department_parameter AND gc.status=@status";

        public CollectGroupQuery(IConnectionRepository connectionRepository)
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
            queryAction = query;
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

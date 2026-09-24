using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Microsoft.Data.SqlClient;

namespace Darbu_records.Query.Comment
{
    public class ConnectionService : IQueryService
    {
        string queryAction;

        public IConnectionRepository _connectionRepository { get; set; }

        public ConnectionService(IConnectionRepository connectionRepository)
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

        public int idGet()
        {
            throw new NotImplementedException();
        }

        public string queryActionGet()
        {
            return queryAction;
        }

        public void queryActionSet(string query)
        {
           queryAction = query;
        }

        public SqlConnection sqlConnGet()
        {
            return _connectionRepository.getSqlConnection();
        }
    }
}

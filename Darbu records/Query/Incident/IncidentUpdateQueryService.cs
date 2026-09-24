using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Microsoft.Data.SqlClient;

namespace Darbu_records.Query.Incident
{
    public class IncidentUpdateQueryService : IQueryService
    {
        IConnectionRepository _connectionRepository;


        private string queryAction = "UPDATE Note SET title = @title , description = @description ,solution = @solution WHERE note_id = @note_id";
        public IncidentUpdateQueryService(IConnectionRepository connectionRepository)
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

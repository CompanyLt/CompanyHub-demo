using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Microsoft.Data.SqlClient;

namespace Darbu_records.Query.Incident
{
    public class IncidentViewQueryService : IQueryService
    {
        IConnectionRepository _connectionRepository;


        private string queryAction = @"Select
            N.title,
            N.description,
            N.note_id           
            FROM
            Note AS N
            INNER JOIN
            NoteCategory NC ON N.note_id = NC.note_id
            INNER JOIN 
            Category C ON NC.category_id = C.id
            WHERE C.id = @category AND N.status=@status AND N.appUser_id=@id";
        public IncidentViewQueryService(IConnectionRepository connectionRepository)
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

using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace Darbu_records.Query.Incident
{
    public class IncidentAddQueryService : IQueryService
    {

        IConnectionRepository _connectionRepository;
        Controller controller {  get; set; }
        private string queryAction= "INSERT INTO Note(title,appUser_id,description,status,upload_date) OUTPUT INSERTED.note_id VALUES(@title,@appUser_id,@description,@status,GETDATE())";
       public IncidentAddQueryService(IConnectionRepository connectionRepository)
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

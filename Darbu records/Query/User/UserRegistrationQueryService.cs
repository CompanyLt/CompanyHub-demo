using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Darbu_records.Query.User
{
    public class UserRegistrationQueryService:IQueryService
    {
        IConnectionRepository _connectionRepository;
       
        private string queryAction = "INSERT INTO Gedimas(List,appUser_Id,Description,Solution,file_name,status) OUTPUT INSERTED.Record_Id VALUES(@name,@client_id,@description,@solution,@file_name,@status)";
        public UserRegistrationQueryService(IConnectionRepository connectionRepository)
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
            queryAction = "INSERT INTO AppUser(Login,Password,Email) VALUES(@name, @password, @email)";
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

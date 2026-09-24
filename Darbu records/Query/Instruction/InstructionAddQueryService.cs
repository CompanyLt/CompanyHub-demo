using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace Darbu_records.Query.Instruction
{
    public class InstructionAddQueryService : IQueryService
    {

        IConnectionRepository _connectionRepository;
        Controller controller {  get; set; }
        private string queryAction= "INSERT INTO Instruction(title,description,department_id,appUserId,upload_date,status) OUTPUT INSERTED.instruction_id VALUES(@title,@description,@department_id,@appUserId,GETDATE(),@status)";
        public InstructionAddQueryService(IConnectionRepository connectionRepository)
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

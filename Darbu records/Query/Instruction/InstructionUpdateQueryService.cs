using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Microsoft.Data.SqlClient;

namespace Darbu_records.Query.Instruction
{
    public class InstructionUpdateQueryService : IQueryService
    {


       IConnectionRepository _connectionRepository;

        string queryAction ="UPDATE Instruction SET title = @name , description = @description WHERE instruction_id = @id";
      public InstructionUpdateQueryService(IConnectionRepository connectionRepository)
        {
            _connectionRepository = connectionRepository;
        }

        string IQueryService.dbConnGet()
        {
            throw new NotImplementedException();
        }

        string IQueryService.dbQueryGet()
        {
            throw new NotImplementedException();
        }

        int IQueryService.idGet()
        {
            throw new NotImplementedException();
        }

        string IQueryService.queryActionGet()
        {
            return queryAction;
        }

        void IQueryService.queryActionSet(string query)
        {
            queryAction = query;
        }

        SqlConnection IQueryService.sqlConnGet()
        {
            return _connectionRepository.getSqlConnection();
        }
    }
}

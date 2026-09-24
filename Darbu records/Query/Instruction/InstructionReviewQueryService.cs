using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Microsoft.Data.SqlClient;

namespace Darbu_records.Query.Instruction
{
    public class InstructionReviewQueryService : IQueryService
    {
        IConnectionRepository _connectionRepository;


        private string queryAction = @"
SELECT
    I.title,
    I.description,
    I.instruction_id,          
    S.likes_count,

    (
        SELECT 
            PI.file_name AS fileName,
            PI.upload_date AS fileDate,
            PI.directory AS fileDirectory,
            PI.original_name AS originalName
        FROM InstructionPhotos AS PI
        WHERE I.instruction_id = PI.instruction_id
        FOR JSON PATH
    ) AS Photos,

    (
        SELECT 
            IFL.file_name AS fileName,
            IFL.original_name AS originalName,            
            IFL.upload_date AS fileDate,
            IFL.directory AS fileDirectory
        FROM InstructionFiles AS IFL
        WHERE I.instruction_id = IFL.instruction_id
        FOR JSON PATH
    ) AS Files

FROM Instruction AS I
LEFT JOIN Instruction_status AS S 
    ON I.instruction_id = S.instruction_id
WHERE I.instruction_id = @instruction_id 
  AND I.status = @status;
";


        public InstructionReviewQueryService(IConnectionRepository connectionRepository)
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

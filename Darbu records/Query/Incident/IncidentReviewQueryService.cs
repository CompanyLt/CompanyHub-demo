using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Microsoft.Data.SqlClient;

namespace Darbu_records.Query.Incident
{
    public class IncidentReviewQueryService : IQueryService
    {
        IConnectionRepository _connectionRepository;


        private string queryAction = @"
SELECT
    N.title,
    N.description,
    N.note_id,          
    
    (
        SELECT 
            PI.file_name AS photoName,
            PI.upload_date AS photoDate,
            PI.directory AS photoDirectory
        FROM NotePhotos AS PI
        WHERE N.note_id = PI.note_id
        FOR JSON PATH
    ) AS Photos,

    (
        SELECT 
            IFL.file_name AS fileName,
            IFL.upload_date AS fileDate,
            IFL.directory AS fileDirectory
        FROM NoteFiles AS IFL
        WHERE N.note_id = IFL.note_id
        FOR JSON PATH
    ) AS Files

FROM Note AS N
WHERE N.note_id = @note_id 
  AND N.status = @status;
";


        public IncidentReviewQueryService(IConnectionRepository connectionRepository)
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

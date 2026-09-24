using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Microsoft.Data.SqlClient;

namespace Darbu_records.Query.File
{
 
        public class FileViewQueryService : IQueryService
        {
            IConnectionRepository _connectionRepository;


        private string queryAction = @"Select
            N.title,
            N.description,
            N.id,
            N.category_id,
            NA.department_id,
            NA.department_access
            FROM
            Note AS N
            INNER JOIN
            NoteAccess NA ON N.id = NA.topic_id         
            WHERE N.category_id = @category AND N.status=@status AND NA.department_id=@department_id";
        public FileViewQueryService(IConnectionRepository connectionRepository)
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


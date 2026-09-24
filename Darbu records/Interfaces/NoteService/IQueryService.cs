using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace Darbu_records.Interfaces.NoteService
{
    public interface IQueryService
    {


        string dbQueryGet();

         string queryActionGet();


         string dbConnGet();


         SqlConnection sqlConnGet();



         int idGet();


        void queryActionSet(string query);

    }
}

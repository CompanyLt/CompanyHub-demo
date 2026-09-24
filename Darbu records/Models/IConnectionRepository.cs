using Microsoft.Data.SqlClient;

namespace Darbu_records.Models
{
    public interface IConnectionRepository
    {



        string getConnetcion();

        SqlConnection getSqlConnection();
    }
}

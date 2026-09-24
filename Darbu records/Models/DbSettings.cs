using Microsoft.Data.SqlClient;
namespace Darbu_records.Models
{
    public class DbSettings:IConnectionRepository
    {
        string dbConnection=@"Data Source=localhost\SQLEXPRESS;
                                    Initial Catalog=CompanyHub;
                                    Integrated Security=True;
                                    Connect Timeout=30;Encrypt=False;";
       private SqlConnection sqlConnection ;

        public DbSettings()
        {
            sqlConnection = new SqlConnection(dbConnection);
        }

        public string getConnetcion()
        {
            return dbConnection;
        }

        public SqlConnection getSqlConnection()
        {
            return new SqlConnection(dbConnection);
        }
    }
}

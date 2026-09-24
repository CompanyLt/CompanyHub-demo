
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Darbu_records.Interfaces.NoteService;

namespace Darbu_records.Query
{
    public class SearchQueryService:IQueryService
    {

       private string dbConnection;
       private  string queryAction;
       private  SqlConnection sqlConnection;

       private  int _id;




        public SearchQueryService(string dbConnection, string queryAction,int id)
        {
            this.dbConnection = dbConnection;
            this.queryAction = queryAction;
            sqlConnection = new SqlConnection(this.dbConnection);
            _id = id;
           
        }



    

  

        public string queryActionGet()
        {
            return queryAction;
        }

        public string dbConnGet()
        {
            return dbConnection;
        }

        public SqlConnection sqlConnGet()
        {
            return sqlConnection;
        }


        public int idGet()
        {
            return _id;
        }


        public string dbQueryGet()
        {
            return dbConnection;
        }

        public void queryActionSet(string query)
        {
            queryAction = query;
        }

    }
}

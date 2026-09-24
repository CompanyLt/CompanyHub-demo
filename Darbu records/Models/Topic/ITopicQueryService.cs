using Microsoft.Data.SqlClient;

namespace Darbu_records.Models.Topic
{
    public interface ITopicQueryService
    {

        public interface IQueryService
        {


            string dbQueryGet();

            string queryActionGet();


            string dbConnGet();


            SqlConnection sqlConnGet();



            int idGet();


            string GetTopicTable();


            string GetTopicReaction();

            string GetTopicCategory();

            string GetTopicFile();

            string GetTopicPhotos();



            void queryActionSet(string query);

        }
    }
}

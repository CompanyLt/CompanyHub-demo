namespace Darbu_records.Query.Topic
{
    public interface ITopicQueryBuilder
    {


       void SetQuery();

       string GetMainTopic();



       string GetTopicReaction();



      string GetTopicCategory();
            


      string GetTopicImages();



      string GetTopicFiles();

      string GetTopicComment();

      string GetTopicReadComment();

      string GetTopicAccess();
           
    }
}

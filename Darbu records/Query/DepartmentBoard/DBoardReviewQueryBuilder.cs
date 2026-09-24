using Darbu_records.Enums.Topics;
using Darbu_records.Query.Topic;

namespace Darbu_records.Query.DepartmentBoard
{
    public class DBoardReviewQueryBuilder:ITopicQueryBuilder
    {
        TopicQuery _topicQuery;

        public DBoardReviewQueryBuilder(TopicQuery topicQuery)
        {
            _topicQuery = topicQuery;
          
        }



        public void SetQuery()
        {
            _topicQuery.SetReviewTopic(TopicMainTable.DepartmentBoard, TopicFileTable.DepartmentBoardFiles, TopicImageTable.DepartmentBoardPhotos,TopicAccessTable.DepartmentBoardAccess);


        }





        public string GetMainTopic()
             => _topicQuery.GetMainTopic();

        public string GetTopicReaction()
            => _topicQuery.GetTopicReaction();

        public string GetTopicCategory()
            => _topicQuery.GetTopicCategory();

        public string GetTopicImages()
            => _topicQuery.GetTopicImages();

        public string GetTopicFiles()
            => _topicQuery.GetTopicFiles();

        public string GetTopicComment()
         => _topicQuery.GetTopicComment();

        public string GetTopicReadComment()
             => _topicQuery.GetTopicCommentRead();


        public string GetTopicAccess()
        => _topicQuery.GetTopicAccess();







    }
}


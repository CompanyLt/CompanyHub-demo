using Darbu_records.Enums.Topics;
using Darbu_records.Query.Topic;

namespace Darbu_records.Query.File
{
    public class FileReviewQueryBuilder:ITopicQueryBuilder
    {

        // TopicQuery _topicQuery;
        TopicReviewQuery _topicQuery;

        public FileReviewQueryBuilder(TopicReviewQuery topicQuery)
        {
            _topicQuery = topicQuery;

        }



        public void SetQuery()
        {
            _topicQuery.SetReviewTopic(TopicMainTable.CompanyFiles, TopicFileTable.CompanyFiles_Files, TopicImageTable.CompanyFilesPhotos, TopicCategoryAccessTable.CategoryAccess);


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

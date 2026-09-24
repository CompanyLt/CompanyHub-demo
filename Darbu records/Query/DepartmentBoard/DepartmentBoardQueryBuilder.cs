using Darbu_records.Enums.Topics;
using Darbu_records.Models.Topic;
using Darbu_records.Query.Topic;

namespace Darbu_records.Query.DepartmentBoard
{
    public class DepartmentBoardQueryBuilder:ITopicQueryBuilder
    {


        TopicQuery _topicQuery;

        public DepartmentBoardQueryBuilder(TopicQuery topicQuery)
        {
            _topicQuery = topicQuery;
         
        }



        public void SetQuery()
        {         
            _topicQuery.SetMainTopic(TopicMainTable.DepartmentBoard);
            _topicQuery.SetTopicAccess(TopicAccessTable.DepartmentBoardAccess);
            _topicQuery.SetTopicReaction(TopicReactionTable.None);
            _topicQuery.SetTopicCategory(TopicCategoryTable.None);
            _topicQuery.SetTopicImages(TopicImageTable.DepartmentBoardPhotos);
            _topicQuery.SetTopicFiles(TopicFileTable.DepartmentBoardFiles);
            _topicQuery.SetTopicComment(TopicCommentTable.DepartmentBoardComment);
            _topicQuery.SetTopicCommentRead(TopicCommentTable.DBoard_read_comments);

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
            =>_topicQuery.GetTopicAccess();



















    }
}

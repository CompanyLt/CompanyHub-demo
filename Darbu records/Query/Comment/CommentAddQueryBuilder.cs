using Darbu_records.Enums.Topics;
using Darbu_records.Query.Topic;

namespace Darbu_records.Query.Comment
{
    public class CommentAddQueryBuilder:ITopicQueryBuilder
    {

        CommentQuery _commentQuery;

        public CommentAddQueryBuilder(CommentQuery commentQuery)
        {
            _commentQuery = commentQuery;

        }



        public void SetQuery()
        {
            _commentQuery.SetMainTopic(TopicMainTable.DepartmentBoardComment);
            _commentQuery.SetTopicReaction(TopicReactionTable.None);
            _commentQuery.SetTopicCategory(TopicCategoryTable.None);
               _commentQuery.SetTopicImages(TopicImageTable.DBoardCommentPhotos);
              _commentQuery.SetTopicFiles(TopicFileTable.DBoardCommentFiles);
           // _commentQuery.SetTopicImages(TopicImageTable.None);
           // _commentQuery.SetTopicFiles(TopicFileTable.None);

        }


        public string GetMainTopic()
       => _commentQuery.GetMainTopic();

        public string GetTopicReaction()
            => _commentQuery.GetTopicReaction();

        public string GetTopicCategory()
            => _commentQuery.GetTopicCategory();

        public string GetTopicImages()
            => _commentQuery.GetTopicImages();

        public string GetTopicFiles()
            => _commentQuery.GetTopicFiles();


        public string GetTopicComment()
         => _commentQuery.GetTopicComment();

        public string GetTopicReadComment()
             => _commentQuery.GetTopicCommentRead();

        public string GetTopicAccess()
            => _commentQuery.GetAccessTopic();

    }
}

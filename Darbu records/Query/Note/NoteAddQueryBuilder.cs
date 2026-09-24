using Darbu_records.Enums.Topics;
using Darbu_records.Models.Topic;
using Darbu_records.Query.Topic;

namespace Darbu_records.Query.Note
{
    public class NoteAddQueryBuilder:ITopicQueryBuilder
    {


        TopicQuery _topicQuery;

        public NoteAddQueryBuilder(TopicQuery topicQuery)
        {
            _topicQuery = topicQuery;
           
        }



        public void SetQuery()
        {         
            _topicQuery.SetMainTopic(TopicMainTable.Note);
            _topicQuery.SetTopicReaction(TopicReactionTable.None);
            _topicQuery.SetTopicCategory(TopicCategoryTable.NoteAccess);
            _topicQuery.SetTopicImages(TopicImageTable.NotePhotos);
            _topicQuery.SetTopicFiles(TopicFileTable.NoteFiles);
            _topicQuery.SetTopicAccess(TopicAccessTable.NoteAccess);
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

using Darbu_records.Enums.Topics;
using Darbu_records.Query.Topic;

namespace Darbu_records.Query.Instruction
{
    public class InstructionAddQueryBuilder: ITopicQueryBuilder
    {
        TopicQuery _topicQuery;

        public InstructionAddQueryBuilder(TopicQuery topicQuery)
        {
            _topicQuery = topicQuery;

        }



        public void SetQuery()
        {
            _topicQuery.SetMainTopic(TopicMainTable.Instruction);
            _topicQuery.SetTopicReaction(TopicReactionTable.None);
            _topicQuery.SetTopicCategory(TopicCategoryTable.InstructionAccess);
            _topicQuery.SetTopicImages(TopicImageTable.InstructionPhotos);
            _topicQuery.SetTopicFiles(TopicFileTable.InstructionFiles);
            _topicQuery.SetTopicAccess(TopicAccessTable.InstructionAccess);
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

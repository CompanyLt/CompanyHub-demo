using Darbu_records.Enums.Topics;

namespace Darbu_records.Query.Comment
{
    public class CommentReadQuery
    {
        string mainQuery = string.Empty;
        string reactionQuery = string.Empty;
        string categoryQuery = string.Empty;
        string filesQuery = string.Empty;
        string imagesQuery = string.Empty;
        string commentQuery = string.Empty;
        string commentReadQuery = string.Empty;







        public void SetMainTopic(TopicMainTable table)
        {

            mainQuery = $@"INSERT INTO {table}(comment_id,text,created_by,created_at) OUTPUT INSERTED.id VALUES(@comment_id,@text,@created_by,GETDATE())";




        }



        public void SetReviewTopic(TopicMainTable table, TopicFileTable fileTable, TopicImageTable imageTable)
        {

            mainQuery = $@"
                        IF EXISTS (
                            SELECT 1
                            FROM DBoard_read_comments
                            WHERE worker_id = @WorkerId
                              AND topic_id = @TopicId
                        )
                        BEGIN
                            UPDATE DBoard_read_comments
                            SET read_count = db.comment_count
                            FROM DepartmentBoard db
                            WHERE DBoard_read_comments.topic_id = db.Id
                              AND worker_id = @WorkerId
                              AND topic_id = @TopicId
                        END
                        ELSE
                        BEGIN
                            INSERT INTO DBoard_read_comments (worker_id, topic_id, read_count)
                            SELECT @WorkerId, @TopicId, comment_count
                            FROM DepartmentBoard
                            WHERE Id = @TopicId
                        END
                        ";
        }


        public void SetTopicReaction(TopicReactionTable table)
        {
            if (table == TopicReactionTable.None) return;
            reactionQuery = $@"INSERT INTO {table}(topic_id,likes_count,dislikes_count) VALUES(@id,@like,@dislike)";




        }

        public void SetTopicCategory(TopicCategoryTable table)
        {
            if (table == TopicCategoryTable.None) return;
            categoryQuery = $@"INSERT INTO {table}(topic_id,category_id) VALUES(@topic_id, @category_id)";




        }


        public void SetTopicImages(TopicImageTable table)
        {

            if (table == TopicImageTable.None) return;
            imagesQuery = $@"INSERT INTO {table}(comment_id,file_name,upload_date,directory,original_name) VALUES(@comment_id,@file_name,GETDATE(),@directory,@original_name)";




        }

        public void SetTopicFiles(TopicFileTable table)
        {
            if (table == TopicFileTable.None) return;
            filesQuery = $@"INSERT INTO {table}(comment_id,file_name,upload_date,directory,original_name) VALUES(@comment_id,@file_name,GETDATE(),@directory,@original_name)";



        }








        ////////////////
        public string GetMainTopic()
        {




            return mainQuery ?? string.Empty;
        }

        public string GetAccessTopic()
        {
            return string.Empty;
        }


        public string GetTopicReaction()
        {





            return reactionQuery ?? string.Empty;
        }

        public string GetTopicCategory()
        {





            return categoryQuery ?? string.Empty;
        }


        public string GetTopicImages()
        {





            return imagesQuery ?? string.Empty;
        }

        public string GetTopicFiles()
        {





            return filesQuery ?? string.Empty;
        }

        public string GetTopicComment()
        {





            return commentQuery ?? string.Empty;
        }


        public string GetTopicCommentRead()
        {





            return commentReadQuery ?? string.Empty;
        }


    }
}

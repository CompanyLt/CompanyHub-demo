using Darbu_records.Enums.Topics;

namespace Darbu_records.Query.Topic
{
    public class TopicViewQuery
    {
        string mainQuery = string.Empty;
        string reactionQuery = string.Empty;
        string categoryQuery = string.Empty;
        string filesQuery = string.Empty;
        string imagesQuery = string.Empty;
        string commentQuery = string.Empty;
        string commentReadQuery = string.Empty;
        string accessQuery = string.Empty;







        public void SetViewTopic(TopicMainTable table,TopicCategoryAccessTable categoryTable)
        {

            mainQuery = $@"Select
                        I.title,
                        I.description,
                        I.id,
                        I.category_id,
                        CA.department_access
                        FROM 
                        {table} AS I
                        INNER JOIN 
                        {categoryTable} AS CA
                        ON I.category_id = CA.category_id            
                        WHERE I.category_id = @category AND CA.department_id = @department_id AND I.status=@status AND CA.department_access=@department_access;";
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
            imagesQuery = $@"INSERT INTO {table}(topic_id,file_name,upload_date,directory,original_name) VALUES(@topic_id,@file_name,GETDATE(),@directory,@original_name)";




        }

        public void SetTopicFiles(TopicFileTable table)
        {
            if (table == TopicFileTable.None) return;
            filesQuery = $@"INSERT INTO {table}(topic_id,file_name,upload_date,directory,original_name) VALUES(@topic_id,@file_name,GETDATE(),@directory,@original_name)";



        }


        public void SetTopicAccess(TopicAccessTable table)
        {
            if (table == TopicAccessTable.None) return;
            accessQuery = $@"INSERT INTO {table}(topic_id,department_id,department_access) VALUES(@topic_id,@department_id,@department_access)";



        }


        public void SetTopicComment(TopicCommentTable table)
        {


        }


        public void SetTopicCommentRead(TopicCommentTable table)
        {

            commentReadQuery = $@"";
        }








        ////////////////
        public string GetMainTopic()
        {




            return mainQuery ?? string.Empty;
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


        public string GetTopicAccess()
        {





            return accessQuery ?? string.Empty;
        }







    }
}

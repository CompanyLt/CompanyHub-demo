using Darbu_records.Enums.Topics;

namespace Darbu_records.Query.Comment
{
    public class CommentQuery
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

            mainQuery = $@"SELECT
                            N.title,
                            N.description,
                            N.id,          
    
                            (
                                SELECT 
                                    PI.file_name AS fileName,
                                    PI.upload_date AS fileDate,
                                    PI.directory AS fileDirectory,
                                    PI.original_name AS originalName
                                FROM {imageTable} AS PI
                                WHERE N.id = PI.comment_id
                                FOR JSON PATH
                            ) AS Photos,

                            (
                                SELECT 
                                    IFL.file_name AS fileName,
                                    IFL.upload_date AS fileDate,
                                    IFL.directory AS fileDirectory,
                                    IFL.original_name AS originalName
                                FROM {fileTable} AS IFL
                                WHERE N.id = IFL.comment_id
                                FOR JSON PATH
                            ) AS Files

                        FROM {table} AS N
                        WHERE N.id = @comment_id 
                          AND N.status = @status;";
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

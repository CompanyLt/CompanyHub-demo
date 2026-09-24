using Darbu_records.Enums.Topics;
using Darbu_records.Query.Comment;
using DevExpress.Xpo.DB.Helpers;

namespace Darbu_records.Query.Topic
{
    public class TopicQuery
    {



        string mainQuery = string.Empty;
        string reactionQuery = string.Empty;
        string categoryQuery = string.Empty;
        string filesQuery = string.Empty;
        string imagesQuery = string.Empty;
        string commentQuery = string.Empty;
        string commentReadQuery = string.Empty;
        string accessQuery = string.Empty;






        public void SetMainTopic(TopicMainTable table)
        {

            mainQuery = $@"INSERT INTO {table}(title,description,category_id,created_by,created_at,status) OUTPUT INSERTED.id VALUES(@title,@description,@category_id,@created_by,GETDATE(),@status)";




        }



        public void SetReviewTopic(TopicMainTable table, TopicFileTable fileTable, TopicImageTable imageTable,TopicAccessTable accessTable)
        {

            mainQuery = $@"SELECT
                            N.title,
                            N.description,
                            N.id,
                            NA.department_id,
							NA.department_access,
    
                            (
                                SELECT 
                                    PI.file_name AS fileName,
                                    PI.upload_date AS fileDate,
                                    PI.directory AS fileDirectory,
                                    PI.original_name AS originalName
                                FROM {imageTable} AS PI
                                WHERE N.id = PI.topic_id
                                FOR JSON PATH
                            ) AS Photos,

                            (
                                SELECT 
                                    IFL.file_name AS fileName,
                                    IFL.upload_date AS fileDate,
                                    IFL.directory AS fileDirectory,
                                    IFL.original_name AS originalName
                                FROM {fileTable} AS IFL
                                WHERE N.id = IFL.topic_id
                                FOR JSON PATH
                            ) AS Files
                        
                        FROM {table} AS N
                        INNER JOIN {accessTable} AS NA ON N.id=NA.topic_id
                        WHERE N.id = @topic_id 
                          AND N.status = @status AND NA.department_id=@department_id;";
        }


        public void SetTopicReaction(TopicReactionTable table)
        {
            if (table == TopicReactionTable.None) return;
            reactionQuery = $@"INSERT INTO {table}(topic_id,likes_count,dislikes_count) VALUES(@id,@like,@dislike)";




        }

        public void SetTopicCategory(TopicCategoryTable table)
        {
            if (table == TopicCategoryTable.None) return;
            categoryQuery = $@"INSERT INTO {table}(topic_id,department_id,department_access) VALUES(@topic_id, @department_id, @department_access)";




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

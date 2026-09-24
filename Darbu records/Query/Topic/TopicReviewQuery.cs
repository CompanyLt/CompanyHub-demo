using Darbu_records.Enums.Topics;
using Darbu_records.Query.Comment;

namespace Darbu_records.Query.Topic
{
    public class TopicReviewQuery
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

            mainQuery = $@"SELECT
                            N.title,
                            N.description,
                            N.topic_id,          
    
                            (
                                SELECT 
                                    PI.file_name AS fileName,
                                    PI.upload_date AS fileDate,
                                    PI.directory AS fileDirectory,
                                    PI.original_name AS originalName
                                FROM NotesPhotos AS PI
                                WHERE N.note_id = PI.note_id
                                FOR JSON PATH
                            ) AS Photos,

                            (
                                SELECT 
                                    IFL.file_name AS fileName,
                                    IFL.upload_date AS fileDate,
                                    IFL.directory AS fileDirectory,
                                    IFL.original_name AS originalName
                                FROM NoteFiles AS IFL
                                WHERE N.note_id = IFL.note_id
                                FOR JSON PATH
                            ) AS Files

                        FROM {table} AS N
                        WHERE N.note_id = @note_id 
                          AND N.status = @status;";
        }

        public void SetMainTopic(TopicMainTable table,TopicFileTable fileTable,TopicImageTable imageTable)
        {

            mainQuery = $@"SELECT
                            N.title,
                            N.description,
                            N.topic_id,          
    
                            (
                                SELECT 
                                    PI.file_name AS fileName,
                                    PI.upload_date AS fileDate,
                                    PI.directory AS fileDirectory,
                                    PI.original_name AS originalName
                                FROM {imageTable} AS PI
                                WHERE N.topic_id = PI.topic_id
                                FOR JSON PATH
                            ) AS Photos,

                            (
                                SELECT 
                                    IFL.file_name AS fileName,
                                    IFL.upload_date AS fileDate,
                                    IFL.directory AS fileDirectory,
                                    IFL.original_name AS originalName
                                FROM {fileTable} AS IFL
                                WHERE N.topic_id = IFL.topic_id
                                FOR JSON PATH
                            ) AS Files

                        FROM {table} AS N
                        WHERE N.topic_id = @topic_id 
                          AND N.status = @status;";
        }

        public void SetReviewTopic(TopicMainTable table, TopicFileTable fileTable, TopicImageTable imageTable, TopicCategoryAccessTable topicCategoryAccessTable)
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
                       INNER JOIN 
                         {topicCategoryAccessTable} AS CA
                         ON N.category_id = CA.category_id            
                       WHERE N.id =@topic_id AND CA.department_id = @department_id AND N.status=@status AND CA.department_access=@department_access";
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

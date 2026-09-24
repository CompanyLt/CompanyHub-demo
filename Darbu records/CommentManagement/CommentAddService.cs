using Darbu_records.DAL;
using Darbu_records.Formos;
using Darbu_records.Interfaces;
using Darbu_records.Models.Topic;
using Darbu_records.Models;
using Darbu_records.Query.Topic;
using Microsoft.Data.SqlClient;
using Darbu_records.Interfaces.NoteService;

namespace Darbu_records.CommentManagement
{
    public class CommentAddService
    {

        IQueryService _queryService;
        NavigationLoader _navigationLoader;
        ITopicQueryBuilder _topicQueryBuilder;
        IFilePathBuilder _filePathBuilder;



        public CommentAddService([FromKeyedServices("InstructionUpdateQueryService")] IQueryService queryService, NavigationLoader navigationLoader)
        {
            _queryService = queryService;
            _navigationLoader = navigationLoader;
        }

        public async Task taskExecution(TaskForm2 taskForm, ITopicQueryBuilder topicQueryBuilder, IFilePathBuilder filePathBuilder)
        {
            //Seteris
            _topicQueryBuilder = topicQueryBuilder;
            _filePathBuilder = filePathBuilder;


            //Pradinei parametrai
            Comment topicForm = taskForm.comment;
            CategoryGroup? group = await _navigationLoader.GetGroupAsync(topicForm.groupId);
            Category? category = await _navigationLoader.GetCategoryAsync(topicForm.categoryId);
            string query = _topicQueryBuilder.GetMainTopic();

            FileCommunicationService fileCommunicationService = new FileCommunicationService();

           //  Console.WriteLine($"nahuj {group.Name}   {category.Name}    {topicForm.title}  topic if{topicForm.id} ");
           //  string fileDirectory = _filePathService.getInstructionFilePath(group.Name,category.Name);  
             string fileDirectory = _filePathBuilder.GetFilePath(group.Name, category.Name, topicForm.title);
          


            // Console.WriteLine($"nahuj {group.Name}   {category.Name}    {topicForm.title}  direktorija {fileDirectory}");
            //Console.WriteLine(_topicQueryBuilder.GetMainTopic());
            //Console.WriteLine(_topicQueryBuilder.GetTopicImages());
            //Console.WriteLine(_topicQueryBuilder.GetTopicFiles());
            //Console.WriteLine(_topicQueryBuilder.GetTopicCategory());
            FileForm fileForm = new FileForm();
            ///////////////////////////////////////////////////////////////////////////////////



            fileCommunicationService.checkDirection(fileDirectory);




            //RUSIUOJAME
            await fileCommunicationService.filesSorter(taskForm.filesCollection, fileForm, fileDirectory);


            //////////////////////////////////////////////////////////////

            long topicId = -1;

            using (SqlConnection connection = _queryService.sqlConnGet())
            {

                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(cmdText: query, connection: connection))
                {
                    // Console.WriteLine($"id : {taskForm.workerId}, {topicForm.title}, {topicForm.description}, {taskForm.departmentId}");
                   
                    command.Parameters.Add(new SqlParameter("@text", topicForm.text));
                    command.Parameters.Add(new SqlParameter("@comment_id", topicForm.topic_id));
                    command.Parameters.Add(new SqlParameter("@created_by", taskForm.workerId));                   
                    topicId = (long)(await command.ExecuteScalarAsync() ?? 0);






                }
                //Patinka sql irasas
                //UZKLAUSOS SETERIS
                if (_topicQueryBuilder.GetTopicReaction() != string.Empty)
                {
                    query = _topicQueryBuilder.GetTopicReaction();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@id", topicId));
                        command.Parameters.Add(new SqlParameter("@like", 1));
                        command.Parameters.Add(new SqlParameter("@dislike", 1));
                        await command.ExecuteNonQueryAsync();

                    }


                }




                if (_topicQueryBuilder.GetTopicCategory() != string.Empty)
                {
                    query = _topicQueryBuilder.GetTopicCategory();

                    using (SqlCommand command = new(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@topic_id", topicId));
                        command.Parameters.Add(new SqlParameter("@category_id", topicForm.categoryId));
                        await command.ExecuteNonQueryAsync();
                    }

                }
                //kategorijos idejimas



                //cia pridedami nuotraukos
                if (_topicQueryBuilder.GetTopicImages() != string.Empty)
                {
                    query = _topicQueryBuilder.GetTopicImages();
                  //  Console.WriteLine(query);
                    foreach (var temp in fileForm.photosCollection)
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {


                            command.Parameters.Add(new SqlParameter("@comment_id", topicId));
                            command.Parameters.Add(new SqlParameter("@file_name", temp.name));
                            command.Parameters.Add(new SqlParameter("@directory", temp.directory));
                            command.Parameters.Add(new SqlParameter("@original_name", temp.originalName));
                            await command.ExecuteNonQueryAsync();

                        }
                    }
                }




                //cia pridedami failai
                if (_topicQueryBuilder.GetTopicFiles() != string.Empty)
                {
                    query = _topicQueryBuilder.GetTopicFiles();
                 //   Console.WriteLine(query);
                    foreach (var temp in fileForm.filesCollection)
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {


                            command.Parameters.Add(new SqlParameter("@comment_id", topicId));
                            command.Parameters.Add(new SqlParameter("@file_name", temp.name));
                            command.Parameters.Add(new SqlParameter("@directory", temp.directory));
                            command.Parameters.Add(new SqlParameter("@original_name", temp.originalName));
                            await command.ExecuteNonQueryAsync();

                        }
                    }
                }

                ///KOMENTARO SKAITYMO LOGIKA              
                query = "UPDATE DepartmentBoard SET comment_count=comment_count+1 WHERE id=@topic_id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add(new SqlParameter("@topic_id", topicForm.topic_id));                   
                    await command.ExecuteNonQueryAsync();

                }



                await connection.CloseAsync();
            }






        }



    }




}


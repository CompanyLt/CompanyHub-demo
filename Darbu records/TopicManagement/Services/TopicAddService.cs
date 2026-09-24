using Darbu_records.DAL;
using Darbu_records.Enums.Topics;
using Darbu_records.Formos;
using Darbu_records.Interfaces;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Darbu_records.Models.Topic;
using Darbu_records.OldItems;
using Darbu_records.Query;
using Darbu_records.Query.DepartmentBoard;
using Darbu_records.Query.Topic;
using DevExpress.Xpo.DB.Helpers;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;

namespace Darbu_records.TopicManagement.Services
{
    public class TopicAddService
    {

        IQueryService _queryService;     
        NavigationLoader _navigationLoader;
        ITopicQueryBuilder _topicQueryBuilder;
        IFilePathBuilder _filePathBuilder;



        public TopicAddService([FromKeyedServices("InstructionUpdateQueryService")] IQueryService queryService, NavigationLoader navigationLoader)
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
            TopicForm topicForm = taskForm.topicForm;

            CategoryGroup? group = await _navigationLoader.GetGroupAsync(topicForm.groupId);
            Category? category = await _navigationLoader.GetCategoryAsync(topicForm.categoryId);
            
            string query = _topicQueryBuilder.GetMainTopic();

            FileCommunicationService fileCommunicationService = new FileCommunicationService();

            // Console.WriteLine($"nahuj {group.Name}   {category.Name}    {topicForm.title}");
            // string fileDirectory = _filePathService.getInstructionFilePath(group.Name,category.Name);  
            string fileDirectory = _filePathBuilder.GetFilePath(group.Name, category.Name, topicForm.title);
            // Console.WriteLine($"nahuj {group.Name}   {category.Name}    {topicForm.title}  direktorija {fileDirectory}");
          
            FileForm fileForm = new FileForm();
            ///////////////////////////////////////////////////////////////////////////////////



            fileCommunicationService.checkDirection(fileDirectory);


            if (topicForm.categoryId == 0) { topicForm.categoryId = 1; }

            //RUSIUOJAME
            await fileCommunicationService.filesSorter(taskForm.filesCollection, fileForm, fileDirectory);


            //////////////////////////////////////////////////////////////

            int topicId = -1;
            
            using var connection = _queryService.sqlConnGet();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                using var command = new SqlCommand(cmdText: query, connection: connection,transaction);
                // Console.WriteLine($"id : {taskForm.workerId}, {topicForm.title}, {topicForm.description}, {taskForm.departmentId}");
                command.Parameters.Add(new SqlParameter("@title", topicForm.title));
                command.Parameters.Add(new SqlParameter("@description", topicForm.description));
                command.Parameters.Add(new SqlParameter("@category_id", topicForm.categoryId));
                command.Parameters.Add(new SqlParameter("@created_by", taskForm.workerId));
                command.Parameters.Add(new SqlParameter("@status", 1));
                topicId = (int)(await command.ExecuteScalarAsync() ?? 0);








                if (_topicQueryBuilder.GetTopicAccess() != string.Empty)
                {
                    query = _topicQueryBuilder.GetTopicAccess();

                    using var accessCommand = new SqlCommand(query, connection,transaction);
                    accessCommand.Parameters.Add(new SqlParameter("@topic_id", topicId));
                    accessCommand.Parameters.Add(new SqlParameter("@department_id", taskForm.departmentId));
                    accessCommand.Parameters.Add(new SqlParameter("@department_access", taskForm.departmentId));
                    await accessCommand.ExecuteNonQueryAsync();




                }
                //Patinka sql irasas
                //UZKLAUSOS SETERIS
                if (_topicQueryBuilder.GetTopicReaction() != string.Empty)
                {
                    query = _topicQueryBuilder.GetTopicReaction();

                    using var reactionCommand = new SqlCommand(query, connection, transaction);
                    reactionCommand.Parameters.Add(new SqlParameter("@id", topicId));
                    reactionCommand.Parameters.Add(new SqlParameter("@like", 1));
                    reactionCommand.Parameters.Add(new SqlParameter("@dislike", 1));
                    await reactionCommand.ExecuteNonQueryAsync();




                }

                //cia pridedami nuotraukos
                if (_topicQueryBuilder.GetTopicImages() != string.Empty)
                {
                    query = _topicQueryBuilder.GetTopicImages();


                    foreach (var temp in fileForm.photosCollection)
                    {
                        using var photoCommand = new SqlCommand(query, connection, transaction);
                        photoCommand.Parameters.Add(new SqlParameter("@topic_id", topicId));
                        photoCommand.Parameters.Add(new SqlParameter("@file_name", temp.name));
                        photoCommand.Parameters.Add(new SqlParameter("@directory", temp.directory));
                        photoCommand.Parameters.Add(new SqlParameter("@original_name", temp.originalName));
                        await photoCommand.ExecuteNonQueryAsync();


                    }
                }



                //cia pridedami failai
                if (_topicQueryBuilder.GetTopicFiles() != string.Empty)
                {
                    query = _topicQueryBuilder.GetTopicFiles();
                    foreach (var temp in fileForm.filesCollection)
                    {
                        using var fileCommand = new SqlCommand(query, connection, transaction);
                        fileCommand.Parameters.Add(new SqlParameter("@topic_id", topicId));
                        fileCommand.Parameters.Add(new SqlParameter("@file_name", temp.name));
                        fileCommand.Parameters.Add(new SqlParameter("@directory", temp.directory));
                        fileCommand.Parameters.Add(new SqlParameter("@original_name", temp.originalName));
                        await fileCommand.ExecuteNonQueryAsync();


                    }
                }

                if (_topicQueryBuilder.GetTopicReadComment() != string.Empty)
                {
                    query = "INSERT INTO DBoard_read_comments(worker_id,topic_id) VALUES(@worker_id,@topic_id)";
                    using var commentCommand = new SqlCommand(query, connection, transaction);
                    commentCommand.Parameters.Add(new SqlParameter("@topic_id", topicId));
                    commentCommand.Parameters.Add(new SqlParameter("@worker_id", taskForm.workerId));
                    await commentCommand.ExecuteNonQueryAsync();


                }


             await   transaction.CommitAsync();
            }
            catch(Exception ex) 
            {

                //LOGIKA PRIDEJIMAS I LOGERI AR PAN
                //_logger.Add(ex);
              await  transaction.RollbackAsync();
                throw;
            }
                    
                           
        }






        }



    }















using Darbu_records.DAL;
using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Darbu_records.OldItems;
using DevExpress.Xpo.DB.Helpers;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;

namespace Darbu_records.InstructionManagement
{
    public class InstructionAddService : INoteTaskService
    {

        IQueryService _queryService;
        FilePathService _filePathService;

        NavigationLoader _navigationLoader;

        public InstructionAddService([FromKeyedServices("InstructionAddQueryService")] IQueryService queryService, FilePathService filePathService, NavigationLoader navigationLoader)
        {
            _queryService = queryService;
            _filePathService = filePathService;
            _navigationLoader = navigationLoader;
        }

        public async Task taskExecution(TaskForm taskForm)
        {
            //Pradinei parametrai
            InstructionForm instructionForm = taskForm.instructionForm;
            CategoryGroup? group = await _navigationLoader.GetGroupAsync(instructionForm.groupId);
            Category? category = await _navigationLoader.GetCategoryAsync(instructionForm.categoryId);
            FileCommunicationService fileCommunicationService = new FileCommunicationService();
            string fileDirectory = _filePathService.getInstructionFilePath(group.Name,category.Name);   
            FileForm fileForm = new FileForm();
            ///////////////////////////////////////////////////////////////////////////////////
         

           
            fileCommunicationService.checkDirection(fileDirectory);          
            
           

           
            //RUSIUOJAME
             await fileCommunicationService.filesSorter(taskForm.filesCollection,fileForm, fileDirectory);
           

            //////////////////////////////////////////////////////////////

            int instructionId = -1;

            using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(cmdText: _queryService.queryActionGet(), connection: connection))
                {
                    command.Parameters.Add(new SqlParameter("@title", taskForm.instructionForm.pavadinimas));
                    command.Parameters.Add(new SqlParameter("@description", taskForm.instructionForm.aprasymas));
                    command.Parameters.Add(new SqlParameter("@department_id", taskForm.departmentId));
                    command.Parameters.Add(new SqlParameter("@appUserId", taskForm.noteId));
                    command.Parameters.Add(new SqlParameter("@status", 1));
                    instructionId = (int)(await command.ExecuteScalarAsync() ?? 0);






                }
                //Patinka sql irasas

                string query = "INSERT INTO Instruction_status(instruction_id,likes_count,dislikes_count) VALUES(@id,@patinka,@nepatinka)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", instructionId));
                    command.Parameters.Add(new SqlParameter("@patinka", 1));
                    command.Parameters.Add(new SqlParameter("@nepatinka", 1));
                    await command.ExecuteNonQueryAsync();

                }

                //kategorijos idejimas
                query = "INSERT INTO InstructionCategory(instruction_id,category_id) VALUES(@instruction_id, @category_id)";
                using (SqlCommand command = new(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@instruction_id", instructionId));
                    command.Parameters.Add(new SqlParameter("@category_id", taskForm.instructionForm.categoryId));
                    await command.ExecuteNonQueryAsync();
                }


                //cia pridedami nuotraukos

                query = "INSERT INTO InstructionPhotos(instruction_id,file_name,upload_date,directory,original_name) VALUES(@instruction_id,@file_name,GETDATE(),@directory,@original_name)";
                foreach (var temp in fileForm.photosCollection)
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        command.Parameters.Add(new SqlParameter("@instruction_id", instructionId));
                        command.Parameters.Add(new SqlParameter("@file_name", temp.name));
                        command.Parameters.Add(new SqlParameter("@directory", temp.directory));
                        command.Parameters.Add(new SqlParameter("@original_name", temp.originalName));
                        await command.ExecuteNonQueryAsync();

                    }
                }



                //cia pridedami failai

                query = "INSERT INTO InstructionFiles(instruction_id,file_name,upload_date,directory,original_name) VALUES(@instruction_id,@file_name,GETDATE(),@directory,@original_name)";
                foreach (var temp in fileForm.filesCollection)
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        command.Parameters.Add(new SqlParameter("@instruction_id", instructionId));
                        command.Parameters.Add(new SqlParameter("@file_name", temp.name));
                        command.Parameters.Add(new SqlParameter("@directory", temp.directory));
                        command.Parameters.Add(new SqlParameter("@original_name", temp.originalName));
                        await command.ExecuteNonQueryAsync();

                    }
                }


                await connection.CloseAsync();
            }






        }



    }









}
    

    


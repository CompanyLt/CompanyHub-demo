using Darbu_records.CategoryManagement;
using Darbu_records.CommentManagement;
using Darbu_records.DAL;
using Darbu_records.DepartmentBoardManagement;
using Darbu_records.DepartmentManagement;
using Darbu_records.Formos;
using Darbu_records.GroupManagement;
using Darbu_records.IncidentManagement;
using Darbu_records.InstructionManagement;
using Darbu_records.Interfaces;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Query.Categories;
using Darbu_records.Query.Comment;
using Darbu_records.Query.Configuration;
using Darbu_records.Query.Department;
using Darbu_records.Query.DepartmentBoard;
using Darbu_records.Query.File;
using Darbu_records.Query.Group;
using Darbu_records.Query.Incident;
using Darbu_records.Query.Instruction;
using Darbu_records.Query.Note;
using Darbu_records.Query.ShareDesk;
using Darbu_records.Query.Topic;
using Darbu_records.Query.User;
using Darbu_records.SharedeskManagement;
using Darbu_records.TopicManagement.Services;
using Darbu_records.UserManagement;
using Microsoft.Extensions.DependencyInjection;

namespace Darbu_records.DI
{
    public static class ServiceRegistration
    {

       
       

    




       

        public static void NoteServices(WebApplicationBuilder builder)
        {
            //Services
      
            builder.Services.AddKeyedTransient<INoteTaskService, NoteDeleteService>("NoteDeleteService");
            builder.Services.AddKeyedTransient<INoteTaskService, NoteUpdateService>("NoteUpdateService");
            builder.Services.AddKeyedTransient<INoteTaskService, NoteViewService>("NoteViewService");     
            builder.Services.AddScoped<NoteRepository>();

            builder.Services.AddKeyedTransient<ITopicQueryBuilder,NoteAddQueryBuilder>("NoteAddQueryBuilder");
            builder.Services.AddKeyedTransient<ITopicQueryBuilder, NoteReviewQueryBuilder>("NoteReviewQueryBuilder");
            builder.Services.AddKeyedTransient<IQueryService, FileViewQueryService>("FileViewQueryService");
          

        }


        public static void FilePathServices(WebApplicationBuilder builder)
        {
            //Services
            builder.Services.AddKeyedTransient<IFilePathBuilder, DboardFilePathBuilder>("DboardFilePathBuilder");

            builder.Services.AddKeyedTransient<IFilePathBuilder, NoteFilePathBuilder>("NoteFilePathBuilder");

            builder.Services.AddKeyedTransient<IFilePathBuilder, InstructionFilePathBuilder>("InstructionFilePathBuilder");

            builder.Services.AddKeyedTransient<IFilePathBuilder, WorkerFilePathBuilder>("WorkerFilePathBuilder");

            builder.Services.AddKeyedTransient<IFilePathBuilder,GroupFilePathBuilder>("GroupFilePathBuilder");
            builder.Services.AddKeyedTransient<IFilePathBuilder, CategoryFilePathBuilder>("CategoryFilePathBuilder");
            builder.Services.AddKeyedTransient<IFilePathBuilder, DepartmentFilePathBuilder>("DepartmentFilePathBuilder");


        }





        public static void TopicService(WebApplicationBuilder builder)
        {
            //Services
            builder.Services.AddKeyedTransient<ITopicQueryBuilder, DepartmentBoardQueryBuilder>("DepartmentBoardQueryBuilder");
            builder.Services.AddKeyedTransient<ITopicQueryBuilder, DBoardReviewQueryBuilder>("DBoardReviewQueryBuilder");
            builder.Services.AddScoped<DBoardRepository>();
            builder.Services.AddScoped<TopicAddService>();
            builder.Services.AddScoped<TopicReviewService>();
            builder.Services.AddScoped<TopicViewService>();
            builder.Services.AddTransient<TopicQuery>();
            builder.Services.AddTransient<TopicReviewQuery>();
            builder.Services.AddTransient<TopicViewQuery>();


        }




        public static void CommentServices(WebApplicationBuilder builder)
        {
            builder.Services.AddKeyedTransient<IQueryService, ConnectionService>("CommentConnectionService");
            builder.Services.AddKeyedTransient<ITopicQueryBuilder,CommentAddQueryBuilder>("CommentAddQueryBuilder");
            builder.Services.AddScoped<CommentConnectionService>();
         
            builder.Services.AddScoped<CommentRepository>();
            builder.Services.AddTransient<CommentAddService>();
            builder.Services.AddTransient<CommentQuery>();



        }


        public static void ShareDesk(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ShareGroupQuery>();
            builder.Services.AddTransient<ShareCancelQuery>();
            builder.Services.AddTransient<ShareCategoryQuery>();
            builder.Services.AddKeyedTransient<IShareQueryBuilder, ShareGroupQueryBuilder>("ShareGroupQueryBuilder");
            builder.Services.AddScoped<ShareGroupServise>();
            builder.Services.AddScoped<ShareCategoryService>();
            builder.Services.AddScoped<ShareCancelService>();
            builder.Services.AddKeyedTransient<IShareQueryBuilder, ShareCategoryQueryBuilder>("ShareCategoryQueryBuilder");
            builder.Services.AddKeyedTransient<IShareQueryBuilder, ShareCancelQueryBuilder>("ShareCancelQueryBuilder");


        }


        public static void GroupServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<GroupAddService>();
            builder.Services.AddScoped<GroupToCategoryService>();
            builder.Services.AddTransient<CreateGroupQuery>();
            builder.Services.AddTransient<GroupToCategoryQuery>();
            builder.Services.AddKeyedTransient<IGroupQueryBuilder, CreateGroupQueryBuilder>("CreateGroupQueryBuilder");
            builder.Services.AddKeyedTransient<IGroupQueryBuilder, GroupToCategoryQueryBuilder>("GroupToCategoryQueryBuilder");
        }

        public static void CategoryServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<CategoryAddService>();
            builder.Services.AddScoped<CategoryToGroupService>();
            builder.Services.AddTransient<CategoryToGroupQuery>();
            builder.Services.AddTransient<CreateCategoryQuery>();
            builder.Services.AddKeyedTransient<ICategoryQueryBuilder, CategoryToGroupQueryBuilder>("CategoryToGroupQueryBuilder");
            builder.Services.AddKeyedTransient<ICategoryQueryBuilder, CreateCategoryQueryBuilder>("CreateCategoryQueryBuilder");
        }




        public static void FileService(WebApplicationBuilder builder)
        {
            builder.Services.AddKeyedTransient<ITopicQueryBuilder, FileViewQueryBuilder>("FileViewQueryBuilder");
            builder.Services.AddKeyedTransient<ITopicQueryBuilder, FileReviewQueryBuilder>("FileReviewQueryBuilder");
        }



        //public static void IncidentQueryServices(WebApplicationBuilder builder)
        //{
        //    builder.Services.AddKeyedTransient<IQueryService, IncidentAddQueryService>("IncidentAddQueryService");
        //    builder.Services.AddKeyedTransient<IQueryService, IncidentDeleteQueryService>("IncidentDeleteQueryService");
        //    builder.Services.AddKeyedTransient<IQueryService, IncidentUpdateQueryService>("IncidentUpdateQueryService");
        //    builder.Services.AddKeyedTransient<IQueryService, IncidentViewQueryService>("IncidentViewQueryService");
        //    builder.Services.AddKeyedTransient<IQueryService, IncidentReviewQueryService>("IncidentReviewQueryService");
        //}


        public static void NoteQueryServices(WebApplicationBuilder builder)
        {
          
            builder.Services.AddKeyedTransient<IQueryService, NoteDeleteQueryService>("NoteDeleteQueryService");
            builder.Services.AddKeyedTransient<IQueryService, NoteUpdateQueryService>("NoteUpdateQueryService");
            builder.Services.AddKeyedTransient<IQueryService, NoteViewQueryService>("NoteViewQueryService");
           
        }

        //INSTRUCTION QUERY
        public static void InstructionQueryServices(WebApplicationBuilder builder)
        {
            builder.Services.AddKeyedTransient<IQueryService, InstructionReviewQueryService>("InstructionReviewQueryService");
            builder.Services.AddKeyedTransient<IQueryService, InstructionViewQueryService>("InstructionViewQueryService");
            builder.Services.AddKeyedTransient<IQueryService, InstructionAddQueryService>("InstructionAddQueryService");
            builder.Services.AddKeyedTransient<IQueryService, InstructionUpdateQueryService>("InstructionUpdateQueryService");


            builder.Services.AddKeyedTransient<ITopicQueryBuilder, InstructionAddQueryBuilder>("InstructionAddQueryBuilder");
            builder.Services.AddKeyedTransient<ITopicQueryBuilder, InstructionReviewQueryBuilder>("InstructionReviewQueryBuilder");




        }

        public static void InstructionServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<InstructionRepository>();
            builder.Services.AddKeyedTransient<INoteTaskService, InstructionReviewService>("InstructionReviewService");
            builder.Services.AddKeyedTransient<INoteTaskService, InstructionViewService>("InstructionViewService");
            builder.Services.AddKeyedTransient<INoteTaskService, InstructionAddService>("InstructionAddService");
            builder.Services.AddKeyedTransient<INoteTaskService, InstructionUpdateService>("InstructionUpdateService");
        }


        public static void WorkerQueryServices(WebApplicationBuilder builder)
        {
            builder.Services.AddKeyedTransient<IQueryService,SelectWorkerQuery>("SelectWorkerQuery");
            builder.Services.AddKeyedTransient<IUserQueryBuilder, WorkerViewQueryBuilder>("WorkerViewQueryBuilder");
            builder.Services.AddKeyedTransient<IUserQueryBuilder, CreateWorkerQueryBuilder>("CreateWorkerQueryBuilder");
            builder.Services.AddTransient<WorkerViewQuery>();
            builder.Services.AddTransient<CreateWorkerQuery>();
            builder.Services.AddScoped<WorkerViewService>();
            builder.Services.AddScoped<WorkerAddService>();





        }

        public static void DepartmentServices(WebApplicationBuilder builder)
        {
            builder.Services.AddKeyedTransient<IDepartmentQueryBuilder, CreateDepartmentQueryBuilder>("CreateDepartmentQueryBuilder");
            builder.Services.AddTransient<CreateDepartmentQuery>();
            builder.Services.AddScoped<DepartmentAddService>();
        }



        public static void ConfigurationServices(WebApplicationBuilder builder)
        {

            builder.Services.AddKeyedTransient<IQueryService, CollectGroupQuery>("CollectGroupQuery");
        }

        public static void ConfigurationQueryServices(WebApplicationBuilder builder)
        {

            builder.Services.AddKeyedTransient<IQueryService, CollectGroupQuery>("CollectGroupQuery");
        }




    }

    
}

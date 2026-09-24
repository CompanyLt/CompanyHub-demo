using Darbu_records.Formos;
using Darbu_records.Models.ShareDesk;
using Darbu_records.Query.ShareDesk;
using Darbu_records.TopicManagement.Services;

namespace Darbu_records.SharedeskManagement
{
    public class ShareDeskRepository
    {

        IShareQueryBuilder _shareGroupQueryBuilder;
        IShareQueryBuilder _shareCategoryQueryBuilder;
        IShareQueryBuilder _shareCancelQueryBuilder;
        ShareGroupServise _shareGroupServise;
        ShareCancelService _shareCancelService;
        ShareCategoryService _shareCategoryService;


        public ShareDeskRepository([FromKeyedServices("ShareGroupQueryBuilder")]IShareQueryBuilder shareGroupQueryBuilder,
            ShareGroupServise shareGroupServise,
            ShareCategoryService shareCategoryService,
            ShareCancelService shareCancelService,
            [FromKeyedServices("ShareCategoryQueryBuilder")] IShareQueryBuilder shareCategoryQueryBuilder,
            [FromKeyedServices("ShareCancelQueryBuilder")] IShareQueryBuilder shareCancelQueryBuilder
            ) 
        {
        
        _shareGroupQueryBuilder = shareGroupQueryBuilder;
            _shareCategoryQueryBuilder = shareCategoryQueryBuilder;
        _shareGroupServise = shareGroupServise;
            _shareCategoryService = shareCategoryService;
            _shareCancelService = shareCancelService;
            _shareCancelQueryBuilder = shareCancelQueryBuilder;
            _shareCategoryQueryBuilder = shareCategoryQueryBuilder;



        
        }







        public async Task AddShareGroupAsync(ShareForm shareForm)
        {



            _shareGroupQueryBuilder.SetQuery();
          await  _shareGroupServise.taskExecution(shareForm, _shareGroupQueryBuilder);


           // Console.WriteLine($"share grupe {shareForm.Id} access {shareForm.Department_access}   department_id{shareForm.Department_id}");
            //LOAD query
            // _topicAddQueryBuilder.SetQuery();
            // await _topicAddService.taskExecution(taskForm, _topicAddQueryBuilder, _fileBuilder);

        }

        public async Task AddShareCategoryAsync(ShareForm shareForm)
        {





            _shareCategoryQueryBuilder.SetQuery();
            await _shareCategoryService.taskExecution(shareForm, _shareCategoryQueryBuilder);


            // Console.WriteLine($"share grupe {shareForm.Id} access {shareForm.Department_access}   department_id{shareForm.Department_id}");
            //LOAD query
            // _topicAddQueryBuilder.SetQuery();
            // await _topicAddService.taskExecution(taskForm, _topicAddQueryBuilder, _fileBuilder);

        }




        public async Task DeleteSharedTopicAsync(ShareForm shareForm)
        {
            //inicializacija
            _shareCancelQueryBuilder.SetQuery();

          
            await _shareCancelService.taskExecution(shareForm, _shareCancelQueryBuilder);

        }

    }
}

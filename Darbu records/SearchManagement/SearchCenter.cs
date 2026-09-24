using System.Data.SqlClient;
using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.OldItems;

namespace Darbu_records.SearchManagement
{
    public class SearchCenter
    {

        INoteTaskService taskService;
        IQueryService queryService;

        public SearchCenter(INoteTaskService taskService, IQueryService queryService)
        {
            this.taskService = taskService;
            this.queryService = queryService;
        }



        public async Task taskExecute(TaskForm taskForm)
        {

            await taskService.taskExecution(taskForm);



        }

        public void changeTaskService(INoteTaskService taskService,IQueryService queryService)
        {
            this.taskService = taskService;
            this.queryService = queryService;
        }

      






    }
}

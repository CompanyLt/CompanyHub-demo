using Darbu_records.Formos;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Darbu_records.Interfaces.NoteService;

namespace Darbu_records.SearchManagement
{
    public class IncidentCenter
    {
      private readonly IQueryService queryService;

      private readonly  INoteTaskService taskService;


        public IncidentCenter(INoteTaskService task, IQueryService query)
        {
           taskService = task;
          queryService = query;

        }

        public IncidentCenter(INoteTaskService task)
        {
            taskService = task;
        }


            //vykdo uzduoti susijusia su incidentais
            public async Task taskExecute(TaskForm taskForm)
         {

              await  taskService.taskExecution(taskForm);


         }




        


        

    }

}

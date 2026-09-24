using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Darbu_records.InstructionManagement
{
    public class InstructionCenter
    {
        public IQueryService queryService;

        public INoteTaskService taskService;


        public InstructionCenter(INoteTaskService task, IQueryService query)
        {
            taskService = task;
            queryService = query;

        }


        public async Task taskExecute(TaskForm taskForm)
        {

            await taskService.taskExecution(taskForm);


        }




    }
}

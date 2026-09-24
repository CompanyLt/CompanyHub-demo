using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;

namespace Darbu_records.InstructionManagement
{
    public class InstructionDeleteService
    {

        IQueryService queryService;



        public InstructionDeleteService(IQueryService queryService)
        {
            this.queryService = queryService;
        }

        public async Task taskExecution(TaskForm taskForm)
        {




        }
    }
}

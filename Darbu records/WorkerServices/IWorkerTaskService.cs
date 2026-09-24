using Darbu_records.Models;

namespace Darbu_records.WorkerServices
{
    public interface IWorkerTaskService
    {

        void taskSet(Worker worker);

        int taskGet(Worker worker);

       



    }
}

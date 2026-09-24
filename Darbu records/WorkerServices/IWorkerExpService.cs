using Darbu_records.Models;
namespace Darbu_records.WorkerServices
{
    public interface IWorkerExpService
    {

        void expSet(Worker worker);

        int expGet(Worker worker);

        void levelSet(Worker worker);

        int levelGet(Worker worker);







    }
}

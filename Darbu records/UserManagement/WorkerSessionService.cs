using Darbu_records.Interfaces;
using Darbu_records.Models;
using System.Text.Json;

namespace Darbu_records.UserManagement
{
    public class WorkerSessionService
    {
        IWorker _worker;
        IHttpContextAccessor _contextAccessor;

        public WorkerSessionService(IWorker worker,IHttpContextAccessor httpContextAccessor)
        {
            _worker = worker;
            _contextAccessor = httpContextAccessor;
        }




        public void SetWorker()
        {
            if(_contextAccessor.HttpContext == null) { return; }
            _contextAccessor.HttpContext.Session.SetString("Worker", JsonSerializer.Serialize(_worker));

        }
    }
}

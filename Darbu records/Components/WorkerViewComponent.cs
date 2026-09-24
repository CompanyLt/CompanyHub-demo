using Darbu_records.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Darbu_records.Components
{
    public class WorkerViewComponent:ViewComponent
    {
      private readonly WorkerLoader _workerLoader;

        public WorkerViewComponent(WorkerLoader workerLoader)
        {
            _workerLoader = workerLoader;
        }


        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default")
        {

            var worker = await _workerLoader.GetAsync();

            return View(viewName, worker);
        }
    }
}

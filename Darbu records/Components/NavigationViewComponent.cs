using Darbu_records.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Darbu_records.Components
{
    public class NavigationViewComponent:ViewComponent
    {
      private readonly NavigationLoader _loader;

        public NavigationViewComponent(NavigationLoader loader)
        {
            _loader = loader;
        }


        public async Task<IViewComponentResult> InvokeAsync(string nameView="Default")
        {

            var appInfo = await _loader.GetAsync();

            return View(nameView,appInfo);
        }

       
    }
}

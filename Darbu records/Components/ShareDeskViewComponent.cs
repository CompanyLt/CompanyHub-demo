using Darbu_records.DAL;
using Darbu_records.Formos;
using Darbu_records.Models.ShareDesk;
using Microsoft.AspNetCore.Mvc;

namespace Darbu_records.Components
{
    public class ShareDeskViewComponent:ViewComponent
    {

        ShareDeskLoader _loader;

        public ShareDeskViewComponent(ShareDeskLoader loader)
        {
            _loader = loader;
        }



        public async Task<IViewComponentResult> InvokeAsync(string stringName, string shareType)
        {
      

            if (shareType == "department")
            {
             ShareDesk shareDesk = await _loader.GetAsync();
               
                return View(stringName, shareDesk);
                      
            }
            else
            {
              
                ShareDesk shareDesk = await _loader.GetSharedAsync();

                return View(stringName, shareDesk);

            }
            



        }



    }
}

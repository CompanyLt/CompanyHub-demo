using Darbu_records.DAL;
using Darbu_records.Formos;
using Microsoft.AspNetCore.Mvc;

namespace Darbu_records.Components
{
    public class DepartmentBoardViewComponent:ViewComponent
    {

        DepartmentBoardLoader _loader;

        public DepartmentBoardViewComponent(DepartmentBoardLoader loader)
        {
            _loader = loader;
        }



        public async Task<IViewComponentResult> InvokeAsync(string stringName="Default")
        {

           DepartmentBoard departmentBoard =  await _loader.GetAsync();

            return View(stringName, departmentBoard);
        }


    }
}

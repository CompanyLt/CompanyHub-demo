namespace Darbu_records.Models.Department
{
    public class DepartmentTaskForm
    {

        public string name { get; set; }

        public string status { get; set; }

        public int created_by { get; set; }

        public List<ActionNavigationItem> navigationActionCollection { get; set; }

        public IFormFile formImage { get; set; }

    }
}

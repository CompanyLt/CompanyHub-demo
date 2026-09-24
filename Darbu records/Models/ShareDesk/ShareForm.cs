using Darbu_records.Models.Department;

namespace Darbu_records.Models.ShareDesk
{
    public class ShareForm
    {

       public IEnumerable<DepartmentItem> Departments { get; set; }

        public int Id { get; set; } 

        public string Title { get; set; }

        public string Description { get; set; }

        public string Department_name { get; set; }

        public int Created_by { get; set; }

       public int GroupId {  get; set; }

       public int Department_id { get; set; }

        public int Department_access {  get; set; }

        public int Share_id { get; set; }

        public int Config_id { get; set; }






    }
}

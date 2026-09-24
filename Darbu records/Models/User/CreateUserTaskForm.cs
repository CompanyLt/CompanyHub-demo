using Darbu_records.Enums;
using Darbu_records.Models.Department;
using System.ComponentModel.DataAnnotations;

namespace Darbu_records.Models.User
{
    public class CreateUserTaskForm
    {

        public int id { get; set; }

        public string name { get; set; }

        public string surname { get; set; }

        public string jobTitle {  get; set; }

        public int tableNumber { get; set; }

        public DateTime created_at { get; set; }

        [Range(1,int.MaxValue,ErrorMessage ="Pasirinkite skyriu")]
        public int departmentId { get; set; }

        public Status status;

        [Range(1,int.MaxValue,ErrorMessage ="Pasirinkite role")]
        public int role_id { get; set; }

        public List<DepartmentItem> departmentCollection { get; set; } = new();
        [Display(Name ="Ikelkite nuotrauka")]
        [Required(ErrorMessage ="Reikalinga nuotrauka")]
        public IFormFile? formImage { get; set; }

    }
}

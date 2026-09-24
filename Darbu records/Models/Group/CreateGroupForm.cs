using System.ComponentModel.DataAnnotations;

namespace Darbu_records.Models.Group
{
    public class CreateGroupForm
    {

        public int created_by { get; set; }

        public int department_id { get; set; }

        public int department_access {  get; set; }

        
        [Required(ErrorMessage = "Įveskite grupės pavadinimą")]
        public string group_name { get; set; }



       
        public IFormFile formImage { get; set; }


    }
}

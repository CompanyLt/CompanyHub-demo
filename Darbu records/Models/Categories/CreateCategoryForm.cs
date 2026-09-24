using System.ComponentModel.DataAnnotations;

namespace Darbu_records.Models.Categories
{
    public class CreateCategoryForm
    {

        public int created_by { get; set; }

        public int department_id { get; set; }

        public int department_access { get; set; }


        [Required(ErrorMessage = "Įveskite Kategorijos pavadinimą")]
        public string category_name { get; set; }




        public IFormFile formImage { get; set; }
    }
}

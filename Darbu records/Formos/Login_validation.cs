
using System.ComponentModel.DataAnnotations;

namespace Darbu_records.Data
{
    public class Login_validation
    {
        [Display(Name ="Iveskite varda:")]
        [Required(ErrorMessage ="Vardas neivestas")]
        public string? Name {  get; set; }

        [Display(Name = "Iveskite slaptazodi:")]
        [Required(ErrorMessage ="slaptazodis neivestas")]
        public string? Password { get; set; }
       
       public bool rememberMe { set; get; }

  
    }
}

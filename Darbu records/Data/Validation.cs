
using System.ComponentModel.DataAnnotations;

namespace Darbu_records.Data
{
    public class Validation
    {
        [Display(Name ="Iveskite varda:")]
        [Required(ErrorMessage ="Vardas neivestas")]
        public string? Name {  get; set; }

        [Display(Name = "Iveskite slaptazodi:")]
        [Required(ErrorMessage ="slaptazodis neivestas")]
        public string? Password { get; set; }
        [Key]
      //  public int Id {  get; set; }

        [Display(Name="Iveskite el pasta:")]
        [Required(ErrorMessage ="el pastas neivestas")]
        [EmailAddress(ErrorMessage = "tai ne el pastas")]
        public string? Email { get; set; }

        [Display(Name ="Pakartokite slaptazodi: ")]
        [Required(ErrorMessage ="slaptazodis neivestas")]
        public string? Password_repeat { get; set; }



  
    }
}

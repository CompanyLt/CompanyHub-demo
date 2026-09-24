
using Darbu_records.Models;
using System.ComponentModel.DataAnnotations;

namespace Darbu_records.Formos
{
    public class IncidentForm
    {
        public int iraso_id { set; get; }
        [Display(Name = "Iveskite gedimo pavadinima:")]
        [Required(ErrorMessage ="Neivestas pavadinimas")]
        public string? pavadinimas { set; get; }
        [Display(Name ="Aprasykite gedima")]
        [Required(ErrorMessage ="Neivestas aprasymas:")]
        public string? aprasymas { set; get; }
        [Display(Name ="Aprasykite sprendima")]
        [Required(ErrorMessage ="Neivestas sprendimas:")]
        public string? sprendimas { set; get; }
        //images naudojamas prideti irasui

        public List<string>? images { set; get; }

        //testuoju be initializerio
        public List<string>? files { set; get; } = new List<string>();
        public string? nuotrauka { set; get;}

       public string? file_name { set; get; }


        public string? category { set; get; }

        public int categoryId { set; get; }

    

        public List<Photos>? photos { set; get; }


        public List<RecordFile> filescollection { set; get; } = new List<RecordFile>();
        public List<RecordFile> pdfcollection { set; get; } = new List<RecordFile>();

        public IncidentForm()
        {
            pavadinimas = "";
            aprasymas = "";
            sprendimas = "";
            nuotrauka = "";
            category = "";
          
        }

        public void images_initialize()
        {
            images = new List<string>();
        }
        public void photos_initialize()
        {
            photos = new List<Photos>();
        }

     

      
        //naudojamas perziurai kai nereikia pilnos info
        public IncidentForm(string? _pavadinimas, string? _aprasymas, int _iraso_id, string? _sprendimas)
        {
            iraso_id = _iraso_id;
            pavadinimas = _pavadinimas;
            aprasymas = _aprasymas;
            sprendimas = _sprendimas;        
            category = "";
            
        }

        public IncidentForm(string? _pavadinimas, string? _aprasymas, int _iraso_id, string? _sprendimas, string? _file_name)
        {
            iraso_id = _iraso_id;
            pavadinimas = _pavadinimas;
            aprasymas = _aprasymas;
            sprendimas = _sprendimas;
        
            file_name = _file_name;
         
        }



       


    }
}

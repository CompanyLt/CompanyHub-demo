using Darbu_records.Models;
using System.ComponentModel.DataAnnotations;

namespace Darbu_records.Formos
{
    public class InstructionForm
    {
        [Required(ErrorMessage ="Neivestas pavadinimas")]
        [Display(Name= "Iveskite instrukcijos pavadinima")]
        public string? pavadinimas { set; get; }

        [Required(ErrorMessage ="Neivestas aprasymas")]
        [Display(Name ="Iveskite aprasymas")]
        public string? aprasymas { set;get; }
        
        
        public int iraso_id { set; get; }

        public string file_name { set; get; }=string.Empty;

       //public string? category { set; get; }

        public int categoryId { set; get; }

        public int? department_access {  set; get; }

        public int groupId { set; get; }



        public int patinka { set; get; }

       // public List<string>? images { set; get; }

       // public List<string>? files { set; get; } = new List<string>();

       // public List<Photos>? photos { set; get; } = new();
        public FileForm fileForm { set; get; } = new FileForm();

       // public List<RecordFile> filescollection { set; get; } = new List<RecordFile>();
       // public List<RecordFile> pdfcollection { set; get; } = new List<RecordFile>();


        


        public InstructionForm()
        {
            pavadinimas = "";
            aprasymas = "";

            

        }

        //public void images_initialize()
        //{
        //   // images = new List<string>();
        //}

        //public void photos_initialize()
        //{
        //   // photos = new List<Photos>();
        //}

        public InstructionForm(string _pavadinimas, string _aprasymas,int _id)
        {
            pavadinimas =_pavadinimas;
            aprasymas =_aprasymas;
           iraso_id = _id;
          file_name = string.Empty;

        }

        public InstructionForm(string _pavadinimas, string _aprasymas, int _id,int _like)
        {
            pavadinimas = _pavadinimas;
            aprasymas = _aprasymas;
            iraso_id = _id;
            file_name = string.Empty;
            patinka = _like;
        }

        public InstructionForm(string _pavadinimas, string _aprasymas, int _id,int _patinka,string _nuotrauka)
        {
            pavadinimas = _pavadinimas;
            aprasymas = _aprasymas;
            iraso_id = _id;
           file_name = _nuotrauka;
            patinka = _patinka;

        }


        public int Id_Get()
        {
            return iraso_id;
        }

        //public bool CheckExtention()
        //{
        //    if (Path.GetExtension(file_name).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
        //    {
        //        return true;

        //    }

        //    return false;
        //}

    }
}

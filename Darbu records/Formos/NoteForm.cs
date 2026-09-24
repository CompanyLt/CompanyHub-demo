using Darbu_records.Models;
using System.ComponentModel.DataAnnotations;

namespace Darbu_records.Formos
{
    public class NoteForm
    {
        [Required(ErrorMessage ="Neivestas pavadinimas")]
        [Display(Name= "Iveskite instrukcijos pavadinima")]
        public string title { set; get; }= string.Empty;

        [Required(ErrorMessage ="Neivestas aprasymas")]
        [Display(Name ="Iveskite aprasymas")]
        public string description { set;get; } = string.Empty;

        public string solution { set; get; } = string.Empty;
        public int noteId { set; get; }

        public string file_name { set; get; }=string.Empty;

       //public string? category { set; get; }

        public int categoryId { set; get; }

        public int groupId { set; get; }

        public int? department_access {  set; get; }


        public List<string>? images { set; get; }

       // public List<string>? files { set; get; } = new List<string>();

       // public List<Photos>? photos { set; get; } = new();

       // public List<RecordFile> filescollection { set; get; } = new List<RecordFile>();
       // public List<RecordFile> pdfcollection { set; get; } = new List<RecordFile>();

        public FileForm fileForm { set; get; } = new FileForm();


      

        public void images_initialize()
        {
           // images = new List<string>();
        }

        public void photos_initialize()
        {
           // photos = new List<Photos>();
        }

       



 


        public int Id_Get()
        {
            return noteId;
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

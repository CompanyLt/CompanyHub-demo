using System.ComponentModel.DataAnnotations;

namespace Darbu_records.Models.Topic
{
    public class TopicForm
    {
        [Required(ErrorMessage = "Neivestas pavadinimas")]
        [Display(Name = "Iveskite instrukcijos pavadinima")]
        public string title { set; get; } = string.Empty;

        [Required(ErrorMessage = "Neivestas aprasymas")]
        [Display(Name = "Iveskite aprasymas")]
        public string description { set; get; } = string.Empty;

        public string solution { set; get; } = string.Empty;
        public int noteId { set; get; }

        public string file_name { set; get; } = string.Empty;

        //public string? category { set; get; }

        public int categoryId { set; get; }

        public int groupId { set; get; }

        public int department_id {  set; get; }

        public int department_access { set; get; }

        public FileForm fileForm { set; get; } = new FileForm();



        //laikinai
        public IEnumerable<CategoryGroup>? categoryGroup { get; set; } 
        public IEnumerable<Category>? categories { get; set; }







    }
}

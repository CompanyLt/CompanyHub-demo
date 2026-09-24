namespace Darbu_records.Models.Categories
{
    public class CategoryToGroupForm
    {

        public int id { get; set; }

        public int department_id { get; set; }
        public int department_access {  get; set; }
        public List<Category> categories { get; set; }

      


    }
}

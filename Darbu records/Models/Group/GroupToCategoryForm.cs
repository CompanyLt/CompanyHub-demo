namespace Darbu_records.Models.Group
{
    public class GroupToCategoryForm
    {
        public int id { get; set; }

        public int department_id { get; set; }
        public int department_access { get; set; }

        public int? SelectedGroupId { get; set; }


        public List <CategoryGroup> groups { get; set; }
    }
}

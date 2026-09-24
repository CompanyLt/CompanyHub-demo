namespace Darbu_records.Models
{
    public class Category
    {

        public int? Id { get; set; }  
        public int? GroupId { get; set; }
        public string? Name { get; set; }

        public string Image { get; set; }

        public int? DepartmentAccess { get; set; }

        public bool isSelected {  get; set; }

        public RecordFile recordFile { get; set; }


    }
}

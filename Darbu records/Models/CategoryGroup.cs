namespace Darbu_records.Models
{
    public class CategoryGroup
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public int? CategoryId { get; set; }

        public int? DepartmentAccess {  get; set; }

        public bool isSelected { get; set; }
        public RecordFile recordFile { get; set; }


    }
}

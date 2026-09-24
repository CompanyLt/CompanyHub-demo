namespace Darbu_records.Models.Department
{
    public class DepartmentItem
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public int? created_by { get; set; }

        public RecordFile recordFile { get; set; }
    }
}

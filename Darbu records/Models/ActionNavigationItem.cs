namespace Darbu_records.Models
{
    public class ActionNavigationItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int DepartmentId { get; set; }   

        public bool isSelected { get; set; }

   
    }
}

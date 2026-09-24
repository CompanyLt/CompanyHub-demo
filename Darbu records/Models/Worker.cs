using Darbu_records.Interfaces;
using Darbu_records.Models.User;

namespace Darbu_records.Models
{
    public class Worker:IWorker
    {
        public int id { get; set; }
        public string? name { set; get; }
        public string? surname { set; get; }

         public string? jobTitle { set; get; }
        public int? tableNumber { set; get; }
        public int progress { set; get; }
        public int expierence { set; get; }
        public string? DepartmentName {  set; get; }

       public int DepartmentId { set; get; }

        public string? status {  set; get; }

        public Role role { set; get; }

        public string avatar { set; get; }

        public RecordFile recordFile { set; get; }

        public List<Achievment> achievments { set; get; } = new();
     

    }
}

using Darbu_records.Models;
using Darbu_records.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Darbu_records.Interfaces
{
    public interface IWorker
    {

        public int id { get; set; }
        public string? name { set; get; }
        public string? surname { set; get; }

        public string? jobTitle { set; get; }


        public int? tableNumber { set; get; }
       
        public int DepartmentId { set; get; }

        public string? DepartmentName { set; get; }

        public string? status { set; get; }


        public int expierence {  set; get; }

        public int progress { set; get; }

        public Role role { set; get; }

        RecordFile recordFile { set; get; }

        public string avatar { set; get; }

        public List<Achievment> achievments { set; get; }
    }
}

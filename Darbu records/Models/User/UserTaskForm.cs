using Darbu_records.Enums;
using Darbu_records.Interfaces;

namespace Darbu_records.Models.User
{
    public class UserTaskForm
    {

     public int id { get; set; }

     public int departmentId {  get; set; }

     public Status status;
   
    
     public  List<IWorker> workers {  get; set; }=new List<IWorker>();



    



    }
}

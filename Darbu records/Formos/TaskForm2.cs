using Darbu_records.Models;
using Darbu_records.Models.Topic;

namespace Darbu_records.Formos
{
    public class TaskForm2
    {

    

        public int? id { get; set; }

        public int departmentId { get; set; }

        public int? workerId { get; set; }

        public int department_access {  get; set; }

       

        public List<TopicForm>? topicCollection { get; set; }=new List<TopicForm>();

        public TopicForm? topicForm { get; set; }

        public Comment comment { get; set; }    

        public IEnumerable<IFormFile>? filesCollection { get; set; }

        /// <summary>
        /// ///
        /// </summary>


        /// public List<IncidentForm>? incidentCollection {  get; set; }

        /// public List<NoteForm>? noteCollection { get; set; }

        //public IncidentForm? incidentForm { get; set; }

        //public NoteForm? noteForm { get; set; }

       // public InstructionForm? instructionForm{ get; set; }


       // public List<InstructionForm>? instructionCollection { get; set; }

      


       

    }
}

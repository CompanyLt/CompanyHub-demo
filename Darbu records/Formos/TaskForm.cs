namespace Darbu_records.Formos
{
    public class TaskForm
    {

        public string? category { get; set; }

        public int? noteId { get; set; }

        public int departmentId { get; set; }   

        public int department_access {  get; set; }

        public int categoryId {  get; set; }

       
        public List<IncidentForm>? incidentCollection {  get; set; }

        public List<NoteForm>? noteCollection { get; set; }

        public IncidentForm? incidentForm { get; set; }

        public NoteForm? noteForm { get; set; }

        public InstructionForm? instructionForm{ get; set; }


        public List<InstructionForm>? instructionCollection { get; set; }

        public IEnumerable<IFormFile>? filesCollection { get; set; }


        //Constructors
        public TaskForm() {
            incidentCollection = new List<IncidentForm>();
            instructionCollection= new List<InstructionForm>();
            noteCollection = new List<NoteForm>();
        
        }

        public TaskForm(string category) { this.category = category; }
        /////
        //Search variables
       public string? searchText { get; set; }

       


        public IFormFile? file { get; set; }
    }
}

namespace Darbu_records.Data
{
    public class Edit_Form
    {
        private int _id;
        public string Name {  get; set; }
        public string Description {  get; set; }

        public string Solution { get; set; }

       public Edit_Form(int id, string name, string description)
        {
            _id = id;
            Name = name;
            Description = description;

        }

        public Edit_Form(int id, string name, string description, string solution)
        {
            _id = id;
            Name = name;
            Description = description;
            Solution = solution;
        }
        public Edit_Form() {
        }
      

        public int Id_Get()
        {
            return _id;
        }
    }
}

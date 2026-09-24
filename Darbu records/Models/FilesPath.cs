using Darbu_records.Interfaces;

namespace Darbu_records.Models
{
    public class FilesPath : IFilesPath
    {
        public string instructionFilePath { get; set; } = "A:\\companyhubFiles\\instruction\\files";
        
        
        public string noteFilePath { get ; set ; } = "A:\\companyhubFiles\\notes\\files";


        public string dBoardFilePath { get; set; } = "A:\\companyhubFiles\\DepartmentDesk\\files";

        public string workerFilePath { get; set; } = "A:\\companyhubFiles\\Worker\\Images";

        public string groupFilePath { get; set; } = "A:\\companyhubFiles\\Group\\Images";

        public string categoryFilePath { get; set; } = "A:\\companyhubFiles\\Category\\Images";

        public string departmentFilePath { get; set; }= "A:\\companyhubFiles\\Departments\\Images";



        public string GetInstructionFilePath()
        {
            return instructionFilePath;
        }

       

        public string GetNoteFilePath()
        {
          return noteFilePath;
        }


       public string GetDBoardFilePath()
        {
            return dBoardFilePath;
        }

        public string GetWorkerFilePath()
        {
            return workerFilePath;
        }

        public string GetGroupFilePath()
        {
            return groupFilePath;
        }

        public string GetCategoryFilePath()
        {
            return categoryFilePath;
        }

        public string GetDepartmentFilePath()
        {
            return departmentFilePath;
        }


    }
}

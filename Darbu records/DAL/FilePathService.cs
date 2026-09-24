using Darbu_records.Interfaces;
using Darbu_records.Models;

namespace Darbu_records.DAL
{
    public class FilePathService
    {

        WorkerLoader _workerLoader;
        IFilesPath _filesPath;

        public FilePathService(WorkerLoader workerLoader,IFilesPath filesPath)
        { 
            _workerLoader = workerLoader;
            _filesPath = filesPath;
        
        }




        public string getInstructionFilePath(string group,string category)
        {
            IWorker worker = _workerLoader.Get();
            string f =  _filesPath.GetInstructionFilePath();
           // Console.WriteLine($"{group}    {category}    {worker.DepartmentName} {worker.name} {f}");
            string filePath = Path.Combine(f,worker.DepartmentName,worker.name,group,category);        
            return filePath;

        }

        public string getNoteFilePath(string group, string category)
        {
            IWorker worker = _workerLoader.Get();
            string f = _filesPath.GetNoteFilePath();
            // Console.WriteLine($"{group}    {category}    {worker.DepartmentName} {worker.name} {f}");
            string imagePath = Path.Combine(f, worker.DepartmentName, worker.name, group, category);
           // Console.WriteLine(filePath);
            return imagePath;

        }


        public string getDBoardFilePath(string group, string category)
        {
            IWorker worker = _workerLoader.Get();
            string f = _filesPath.GetDBoardFilePath();
             Console.WriteLine($" Grupe: {group}  category:  {category}  department  {worker.DepartmentName} {worker.name} {f}");
            string imagePath = Path.Combine(f, worker.DepartmentName, worker.name, group, category);
            // Console.WriteLine(filePath);
            return imagePath;

        }





    }
}

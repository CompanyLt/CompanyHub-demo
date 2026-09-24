using Darbu_records.DAL;
using Darbu_records.Interfaces;
using Darbu_records.Models;

namespace Darbu_records.DepartmentBoardManagement
{
    public class DboardFilePathBuilder : IFilePathBuilder
    {


        WorkerLoader _workerLoader;
        IFilesPath _filesPath;

        public DboardFilePathBuilder(WorkerLoader workerLoader, IFilesPath filesPath)
        {
            _workerLoader = workerLoader;
            _filesPath = filesPath;

        }

        public string GetFilePath(string group, string category,string topic)
        {

            IWorker worker = _workerLoader.Get();
            string f = _filesPath.GetDBoardFilePath();
            // Console.WriteLine($"{group}    {category}    {worker.DepartmentName} {worker.name} {f}");
            string imagePath = Path.Combine(f, worker.DepartmentName, worker.name,topic);
          
            return imagePath;
           
        }
    }
}

using Darbu_records.DAL;
using Darbu_records.Interfaces;

namespace Darbu_records.InstructionManagement
{
    public class InstructionFilePathBuilder:IFilePathBuilder
    {
        WorkerLoader _workerLoader;
        IFilesPath _filesPath;

        public InstructionFilePathBuilder(WorkerLoader workerLoader, IFilesPath filesPath)
        {
            _workerLoader = workerLoader;
            _filesPath = filesPath;

        }

        public string GetFilePath(string group, string category, string topic)
        {

            IWorker worker = _workerLoader.Get();
            string f = _filesPath.GetInstructionFilePath();

            string imagePath = Path.Combine(f, worker.DepartmentName, worker.name, group, category, topic);

            return imagePath;

        }





    }
}

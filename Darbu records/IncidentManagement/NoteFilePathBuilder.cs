using Darbu_records.DAL;
using Darbu_records.Interfaces;
using Darbu_records.Models;

namespace Darbu_records.IncidentManagement
{
    public class NoteFilePathBuilder : IFilePathBuilder
    {


        WorkerLoader _workerLoader;
        IFilesPath _filesPath;

        public NoteFilePathBuilder(WorkerLoader workerLoader, IFilesPath filesPath)
        {
            _workerLoader = workerLoader;
            _filesPath = filesPath;

        }

        public string GetFilePath(string group, string category, string topic)
        {

            IWorker worker = _workerLoader.Get();
            string f = _filesPath.GetNoteFilePath();
           
            string imagePath = Path.Combine(f, worker.DepartmentName, worker.name, group, category, topic);
           
            return imagePath;

        }
    }
}

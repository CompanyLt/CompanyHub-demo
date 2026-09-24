using Darbu_records.DAL;
using Darbu_records.Interfaces;

namespace Darbu_records.UserManagement
{
    public class WorkerFilePathBuilder:IFilePathBuilder
    {
     
           private readonly IFilesPath _filesPath;

            public WorkerFilePathBuilder(IFilesPath filesPath)
            {
               
                _filesPath = filesPath;

            }

            public string GetFilePath(string group, string category, string topic)
            {


                string f = _filesPath.GetWorkerFilePath();

                string imagePath = Path.Combine(f, group, category, topic);

                return imagePath;

            }





        
    }
}

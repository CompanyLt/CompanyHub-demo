using Darbu_records.Interfaces;

namespace Darbu_records.DepartmentManagement
{
    public class DepartmentFilePathBuilder:IFilePathBuilder
    {
        private readonly IFilesPath _filesPath;

        public DepartmentFilePathBuilder(IFilesPath filesPath)
        {

            _filesPath = filesPath;

        }

        public string GetFilePath(string group, string category, string topic)
        {


            string f = _filesPath.GetDepartmentFilePath();

            string imagePath = Path.Combine(f, group, category, topic);

            return imagePath;

        }

    }
}

using Darbu_records.Interfaces;

namespace Darbu_records.CategoryManagement
{
    public class CategoryFilePathBuilder:IFilePathBuilder
    {
        private readonly IFilesPath _filesPath;

        public CategoryFilePathBuilder(IFilesPath filesPath)
        {

            _filesPath = filesPath;

        }

        public string GetFilePath(string group, string category, string topic)
        {


            string f = _filesPath.GetCategoryFilePath();

            string imagePath = Path.Combine(f, group, category, topic);

            return imagePath;

        }
    }
}

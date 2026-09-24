using Darbu_records.Interfaces;

namespace Darbu_records.GroupManagement
{
    public class GroupFilePathBuilder : IFilePathBuilder
    {
        private readonly IFilesPath _filesPath;

        public GroupFilePathBuilder(IFilesPath filesPath)
        {

            _filesPath = filesPath;

        }

        public string GetFilePath(string group, string category, string topic)
        {


            string f = _filesPath.GetGroupFilePath();

            string imagePath = Path.Combine(f, group, category, topic);

            return imagePath;

        }

    }
}

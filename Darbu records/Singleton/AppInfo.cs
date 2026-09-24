using Darbu_records.Models;
using System.Text.RegularExpressions;

namespace Darbu_records.Singleton
{
    public class AppInfo
    {

        public List<CategoryGroup> groups { get;  set; } = new();
        public List<Category> categories { get;  set; } = new();

        public List<NavigationItem> navigationItems { get; set; } = new();

        public List<ActionNavigationItem> actionNavigationItems { get; set; } = new();




       
    }
}

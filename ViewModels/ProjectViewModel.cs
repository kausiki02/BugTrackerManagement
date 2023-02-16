using BugTrackerManagement.Models;

namespace BugTrackerManagement.ViewModels
{
    public class ProjectViewModel
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public List<Bug> CurrentBugs { get; set; }
    }
}

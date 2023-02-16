using BugTrackerManagement.Models;

namespace BugTrackerManagement.ViewModels
{
    public class BugViewModel
    {
        public int BugID { get; set; }
        public string BugInfo { get; set; }
        public BugStates BugState { get; set; }
        public List<Message> CurrentMessages { get; set; }
        public int ProjectID { get; set; }
    }
}

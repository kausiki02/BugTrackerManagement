using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerManagement.Models
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public List<Bug> CurrentBugs { get; set; }
    }
}

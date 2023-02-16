using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerManagement.Models
{
    public class Bug
    {
        
        public string BugInfo { get; set; }
        public BugStates BugState { get; set; }
        public List<Message> CurrentMessages{get;set;}
        public int ProjectID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BugID { get; set; }
    }
    public enum BugStates
    {
        open,
        working,
        resolved,
    }
}

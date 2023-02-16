using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerManagement.Models
{
    public class Message
    {
        
        public string MessageContext { get; set; }
        public bool MessageFlag { get; set; }
        public int BugID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }
    }
    
}

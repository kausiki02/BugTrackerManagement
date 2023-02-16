namespace BugTrackerManagement.Models
{
    public class UserRole
    {
        public User User { get; set; }
        public int UserID { get; set; }

        public Role Role { get; set; }
        public int RoleID { get; set; }
    }
}

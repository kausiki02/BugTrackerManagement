namespace BugTrackerManagement.Models
{
    public class Role
    {
        public int RoleID { get; set; }
        public string Name { get; set; }

        public IList<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}

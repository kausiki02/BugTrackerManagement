namespace BugTrackerManagement.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public IList<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}

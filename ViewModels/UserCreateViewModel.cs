using Microsoft.Build.Framework;

namespace BugTrackerManagement.ViewModels
{
    public class UserCreateViewModel
    {
        [Required]
        public string Username { get; set; }
        public string FullName { get; set; }
        [Required]
        public string Password { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}

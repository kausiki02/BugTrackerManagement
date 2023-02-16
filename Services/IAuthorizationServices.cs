using BugTrackerManagement.ViewModels;

namespace BugTrackerManagement.Services
{
    public interface IAuthorizationServices
    {
        Task Register(UserCreateViewModel user);
        Task<JwtViewModel> LoginAsync(LoginViewModel login);
    }
}

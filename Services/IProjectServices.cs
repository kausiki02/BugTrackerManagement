using BugTrackerManagement.ViewModels;
namespace BugTrackerManagement.Services
{
    public interface IProjectServices
    {
        Task<IEnumerable<ProjectViewModel>> GetAllAsync();
        Task<ProjectViewModel> GetByIdAsync(int id);
        Task<ProjectViewModel> CreateAsync(ProjectCreateViewModel model);
        
    }
}

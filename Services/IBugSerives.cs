using BugTrackerManagement.ViewModels;

namespace BugTrackerManagement.Services
{
    public interface IBugServices
    {
        Task<IEnumerable<BugViewModel>> GetAllAsync();
        Task<BugViewModel> GetByIdAsync(int id);
        Task<BugViewModel> CreateAsync(int id, BugCreateViewModel bug);
    }
}

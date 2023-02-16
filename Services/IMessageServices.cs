using BugTrackerManagement.ViewModels;

namespace BugTrackerManagement.Services
{
    public interface IMessageServices
    {
        Task<IEnumerable<MessageViewModel>> GetAllAsync();
        Task<MessageViewModel> GetLastAsync();
        Task<MessageViewModel> CreateAsync(int proID, int bugID, MessageCreateViewModel message);
        Task<MessageViewModel> GetByIdAsync(int id);
    }
}

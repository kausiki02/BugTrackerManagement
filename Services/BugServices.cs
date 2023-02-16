using Microsoft.EntityFrameworkCore;
using BugTrackerManagement.DAL;
using BugTrackerManagement.Models;
using BugTrackerManagement.ViewModels;
using System;

namespace BugTrackerManagement.Services
{
    public class BugServices:IBugServices
    {
        private readonly BugTrackerCatalogContext _context;

        public BugServices(BugTrackerCatalogContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BugViewModel>> GetAllAsync()
        {
            return await _context.Bugs
                .Select(p => new BugViewModel
                {
                    BugID = p.BugID,
                    BugInfo = p.BugInfo,
                    BugState = p.BugState,
                    CurrentMessages = p.CurrentMessages,
                    ProjectID = p.ProjectID,
                })
                .ToListAsync();
        }
        public async Task<BugViewModel> GetByIdAsync(int id)
        {
            var t = await _context.Bugs
                .FirstAsync(x => x.BugID == id);

            var ans = new BugViewModel
            {
                BugID = t.BugID,
                BugInfo = t.BugInfo,
                BugState = t.BugState,
                CurrentMessages = t.CurrentMessages,
                ProjectID = t.ProjectID
            };

            return ans ;
        }
        public async Task<BugViewModel> CreateAsync(int id, BugCreateViewModel bug)
        {
            var p = ToEntity(id,bug);
            var pc = ToViewModel(p);
            var t = await _context.Projects.FirstAsync(x => x.ProjectID == id);
            if(t.CurrentBugs != null)
            {
                t.CurrentBugs.Add(p);

            }
            else
            {
                t.CurrentBugs=new List<Bug> { p };  
            }
            await _context.AddAsync(p);
            await _context.SaveChangesAsync();
            return pc;
        }
        private BugViewModel ToViewModel(Bug p)
        {
            return new BugViewModel
            {
                BugID = p.BugID,
                BugInfo = p.BugInfo,
                BugState = p.BugState,
                CurrentMessages = p.CurrentMessages,
                ProjectID = p.ProjectID
            };
        }
        private Bug ToEntity(int id,BugCreateViewModel p)
        {
            return new Bug
            {
                BugInfo = p.BugInfo,
                CurrentMessages = new List<Message>(),
                BugState = 0,
                ProjectID = id,
            };
        }
    }
}

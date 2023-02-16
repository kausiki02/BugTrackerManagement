using Microsoft.EntityFrameworkCore;
using BugTrackerManagement.DAL;
using BugTrackerManagement.Models;
using BugTrackerManagement.ViewModels;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BugTrackerManagement.Services
{
    public class ProjectServices:IProjectServices
    {
        private readonly BugTrackerCatalogContext _context;

        public ProjectServices(BugTrackerCatalogContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProjectViewModel>> GetAllAsync()
        {
            return await _context.Projects
                .Select(p => new ProjectViewModel
                {
                    ProjectID=p.ProjectID,
                    ProjectName=p.ProjectName,
                    CurrentBugs=p.CurrentBugs,
                })
                .ToListAsync();
        }
        public async Task<ProjectViewModel> GetByIdAsync(int id)
        {
            var t = await _context.Projects
                .FirstAsync(x => x.ProjectID == id);

            var ans = new ProjectViewModel
            {
                ProjectID= t.ProjectID,
                ProjectName=t.ProjectName,
                CurrentBugs=t.CurrentBugs,
            };

            return ans;
        }
        public async Task<ProjectViewModel> CreateAsync(ProjectCreateViewModel model)
        {
            var p = ToEntity(model);
            
            await _context.AddAsync(p);
            await _context.SaveChangesAsync();
            return ToViewModel(p);
        }
        

        
        private ProjectViewModel ToViewModel(Project p)
        {
            return new ProjectViewModel
            {
                ProjectID=p.ProjectID,
                ProjectName=p.ProjectName,
                CurrentBugs=p.CurrentBugs,
            };
        }
        private Project ToEntity(ProjectCreateViewModel p)
        {
            return new Project
            {
                ProjectName = p.ProjectName,
                CurrentBugs = new List<Bug>(),
            };
        }
    }
}

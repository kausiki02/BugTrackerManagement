using Microsoft.EntityFrameworkCore;
using BugTrackerManagement.DAL;
using BugTrackerManagement.Models;
using BugTrackerManagement.ViewModels;
namespace BugTrackerManagement.Services
{

    public class ReturnObject
    {
        public int open { get; set; }
        public int resolved { get; set; }

        public int totalmessages { get; set; }

        public double resolution_rate { get; set; }

    }
    public class AdminServices:IAdminServices
    {
        private readonly BugTrackerCatalogContext _context;

        public AdminServices(BugTrackerCatalogContext context)
        {
            _context = context;
        }
        public async Task<ReturnObject> DashboardStats()
        {
            var p = await _context.Projects.Select(p => new ProjectViewModel 
            { ProjectID = p.ProjectID,
                ProjectName = p.ProjectName,
                CurrentBugs = p.CurrentBugs })
                .ToListAsync();

            int count1 = 0;
            foreach (var pro in p)
            {
                foreach (var b in pro.CurrentBugs)
                {
                    if (b.BugState == BugStates.open)
                    {
                        count1++;
                    }

                }
            }
            var p1 = await _context.Projects.Select(p => new ProjectViewModel
            { ProjectID = p.ProjectID, ProjectName = p.ProjectName, CurrentBugs = p.CurrentBugs })
                .ToListAsync();

            int count2 = 0;
            foreach (var pro in p1)
            {
                foreach (var b in pro.CurrentBugs)
                {
                    if (b.BugState == BugStates.resolved)
                    {
                        count2++;
                    }

                }
            }
            var p2 = await _context.Projects
                .Select(p => new ProjectViewModel
                {
                    ProjectID = p.ProjectID,
                    ProjectName = p.ProjectName,
                    CurrentBugs = p.CurrentBugs,
                })
                .ToListAsync();

            int count3 = 0;
            foreach (var pro in p2)
            {
                foreach (var b in pro.CurrentBugs)
                {

                    if (b.CurrentMessages != null)
                        count3 += b.CurrentMessages.Count;

                }
            }
            

            var p3 = await _context.Projects
                .Select(p => new ProjectViewModel
                {
                    ProjectID = p.ProjectID,
                    ProjectName = p.ProjectName,
                    CurrentBugs = p.CurrentBugs,
                })
                .ToListAsync();

            int count4 = 0;
            foreach (var pro in p3)
            {
                count4 += pro.CurrentBugs.Count;
            }

            var count5=((double)count2 / (double)count4);


            return new ReturnObject
            {
                open = count1,
                resolved = count2,
                totalmessages = count3,
                resolution_rate = count5,
            };
            
        }
    }
}

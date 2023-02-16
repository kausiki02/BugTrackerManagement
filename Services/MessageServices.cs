using Microsoft.EntityFrameworkCore;
using BugTrackerManagement.DAL;
using BugTrackerManagement.Models;
using BugTrackerManagement.ViewModels;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace BugTrackerManagement.Services
{
    public class MessageServices : IMessageServices
    {
        private readonly BugTrackerCatalogContext _context;

        public MessageServices(BugTrackerCatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MessageViewModel>> GetAllAsync()
        {
            return await _context.Messages
                .Select(p => new MessageViewModel
                {
                    MessageID=p.MessageID,
                    MessageContext=p.MessageContext,
                    MessageFlag=p.MessageFlag,
                    BugID=p.BugID,
                })
                .ToListAsync();
        }
        public async Task<MessageViewModel> CreateAsync(int proID, int bugID, MessageCreateViewModel message)
        {
            var p = ToEntity(bugID,message);
            await _context.AddAsync(p);
            var t = await _context.Bugs.FirstAsync(y=> y.ProjectID==proID && y.BugID == bugID);
            if (t.CurrentMessages!= null)
            {
                t.CurrentMessages.Add(p);

            }
            else
            {
                t.CurrentMessages = new List<Message> { p };
            }
            if (t.CurrentMessages.Count == 1) 
            {
                t.BugState = BugStates.working;
            }
            if(t.CurrentMessages.Last().MessageFlag==true)
            {
                t.BugState = BugStates.resolved;
            }
            await _context.SaveChangesAsync();
            return ToViewModel(p);
        }
        public async Task<MessageViewModel> GetByIdAsync(int id)
        {
            var t = await _context.Messages
                .FirstAsync(x => x.MessageID == id);

            var ans = new MessageViewModel
            {
                MessageID= t.MessageID,
                MessageContext=t.MessageContext,
                MessageFlag=t.MessageFlag,
                BugID=t.BugID,
            };

            return ans;
        }
        public async Task<MessageViewModel> GetLastAsync()
        {
            var AllMessages= await _context.Messages
                .Select(p => new MessageViewModel
                {
                    MessageID = p.MessageID,
                    MessageContext = p.MessageContext,
                    MessageFlag = p.MessageFlag,
                    BugID = p.BugID,
                })
                .ToListAsync();
            return AllMessages.Last();
        }
        

        private MessageViewModel ToViewModel(Message p)
        {
            return new MessageViewModel
            {
                MessageID = p.MessageID,
                MessageContext=p.MessageContext,
                MessageFlag=p.MessageFlag,
                BugID=p.BugID,
            };
        }

        private Message ToEntity(int bugID,MessageCreateViewModel p)
        {
            return new Message
            {
                MessageFlag= p.MessageFlag,
                MessageContext= p.MessageContext,
                BugID=bugID,
            };
        }
    }
}

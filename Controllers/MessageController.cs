using BugTrackerManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BugTrackerManagement.Services;
using BugTrackerManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BugTrackerManagement.Controllers
{
    [Authorize(Policy = "Developer")]
    [Route("api/v1/Project/Bug/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageServices _service;

        public MessageController(IMessageServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageViewModel>>> GetAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MessageViewModel>> GetByIdAsync(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<MessageViewModel>> CreateAsync(int proID, int bugID, MessageCreateViewModel message)
        {
            return Ok(await _service.CreateAsync(proID, bugID, message));
        }
    }
}
using BugTrackerManagement.Services;
using BugTrackerManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerManagement.Controllers
{
    [Authorize(Policy="Developer")]
    [Route("api/v1/Project/[controller]")]
    [ApiController]
    public class BugController : ControllerBase
    {
        private readonly IBugServices _service;

        public BugController(IBugServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BugViewModel>>> GetAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BugViewModel>> GetByIdAsync(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<BugViewModel>> CreateAsync(int id, BugCreateViewModel message)
        {
            return Ok(await _service.CreateAsync(id,message));
        }
    }
}

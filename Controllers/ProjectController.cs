using BugTrackerManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BugTrackerManagement.Services;
using BugTrackerManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BugTrackerManagement.Controllers
{
    [Authorize(Policy = "Developer")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _service;

        public ProjectController(IProjectServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectViewModel>>> GetAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProjectViewModel>> GetByIdAsync(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<ProjectViewModel>> CreateAsync(ProjectCreateViewModel product)
        {
            return Ok(await _service.CreateAsync(product));
        }
    }
}
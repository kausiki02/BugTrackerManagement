using BugTrackerManagement.Services;
using BugTrackerManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerManagement.Controllers
{
    [Authorize(Policy ="Admin")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _service;

        public AdminController(IAdminServices service)
        {
            _service = service;
        }

        [HttpGet("/Dashboard")]
        public async Task<ActionResult<int>> DashboardStats()
        {
            return Ok(await _service.DashboardStats());
        }
    }
}
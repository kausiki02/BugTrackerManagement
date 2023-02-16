using BugTrackerManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using BugTrackerManagement.Services;
namespace BugTrackerManagement.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthorizationController:ControllerBase
    {
        private readonly IAuthorizationServices _authService;

        public AuthorizationController(IAuthorizationServices authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            return Ok(await _authService.LoginAsync(viewModel));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateViewModel viewModel)
        {
            await _authService.Register(viewModel);
            return Ok();
        }
    }
}

using Application.AdminServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
         [HttpPost("TimeoutUser")]
        public async Task<IActionResult> TimeOutUser(string UserId, short AmountOfDays)
        {
            var Response = await _adminService.TimeOutUser(UserId, AmountOfDays);
            if (!Response.banSuccess)
            {
                return BadRequest(Response);
            }
            return Ok(Response);
        }

        [HttpPost("BanUser")]
        public async Task<IActionResult> BanUser(string UserId)
        { 
            var Response = await _adminService.BanUser(UserId);
            if (!Response.banSuccess)
            {
                return BadRequest(Response); 
                    
            }
            return Ok(Response);
        }

        [HttpPost("UnbanUser")]
        public async Task<IActionResult> UnbanUser(string UserId)
        {
            var Response = await _adminService.UnbanUser(UserId);
            if (!Response.unbanSuccess)
            {
                return BadRequest(Response);
                
            }
            return Ok(Response);
        }

    }
}


using Backend.Application.DTOs;
using Backend.Domain.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Backend.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;
      

        public AuthController(IAuthServices authServices )
        {
            _authServices = authServices;
           
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var Result = await _authServices.RegisterUserAsync(registerDTO);
                     if (!Result.IsAuthenticated)
                     {
                         return BadRequest(Result.Message);
                     }
                    
                      
            return Ok(Result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var Result = await _authServices.LoginUserAsync(loginDTO);
            if (!Result.IsAuthenticated)
            {
                return BadRequest(Result.Message);
            }
            return Ok(Result);
        }
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRole([FromBody] RoleDTO roleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           var Result= await _authServices.AddRoleAsync(roleDTO);
           if (Result is not null)
           {
               return NotFound(Result);
           }
            return Ok();
        }

    }
}
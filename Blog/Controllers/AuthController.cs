using Application.Authentication.DTO;
using Application.Authentication.Interface;
using Application.Common.enums;
using Application.EmailServices.DTO;
using Application.EmailServices.Interface;
using Application.PostSev;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = Application.Authentication.DTO.LoginRequest;
using RegisterRequest = Application.Authentication.DTO.RegisterRequest;

namespace Blog.Controllers.AuthController;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthServices _authServices;
    private readonly IConfrimEmailServices _confrimemailServices;
    private readonly IResetPasswordServices _resetPasswordServices;

    public AuthController(IAuthServices authServices ,IConfrimEmailServices confrimemailServices , IResetPasswordServices resetPasswordServices)
    {
        _authServices = authServices;
        _confrimemailServices = confrimemailServices;
        _resetPasswordServices = resetPasswordServices;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest  loginRequest)
    {
        var Response =  await _authServices.LoginAsync(loginRequest);
        if (!Response.IsAuthenticated)
        {
            return Unauthorized(Response.Message);
           
        }

        return Ok(Response);
       
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        var Response = await _authServices.RegisterAsync(registerRequest);
        if (!Response.IsAuthenticated)
        {
            return BadRequest(Response.Message);
           
        }

        return Ok(Response);
    }
    [HttpPost("AddAdmin")]
    public async Task<IActionResult> AddAdmin([FromBody] string Email)
    {
       var Response = await _authServices.AddAdminAsync(Email);
       if (!Response.IsAuthenticated)
       {
           return BadRequest(Response.Message);
       } 
       return Ok(Response);
    }
    // [HttpPost("addrole")]
    // [Authorize(Roles = "Admin")]
    // public async Task<IActionResult> AddRole([FromBody] RoleRequest roleRequest)
    // {
    //     var Response = await _authServices.AddRoleAsync(roleRequest);
    //     if (Response == string.Empty)
    //     {
    //         return BadRequest();
    //     }
    //     return Ok(Response);
    //          
    // }

    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromQuery]string user_id,[FromQuery] string token)
    {
        var Response = await _confrimemailServices.ConfirmEmailAsync(user_id,token);
        if (Response == false)
        {
            return BadRequest("No confirmation email provided");
        }
        return Ok("Confrim Email Success");
    }

    [HttpPost("ResendEmail")]
    public async Task<IActionResult> ResendEmail([FromQuery] string user_id)
    {
        var Response = await _confrimemailServices.SendConfromationEmailAsync(user_id, "http://localhost:5140");
        if (!Response)
        {
            return NotFound("No confirmation email provided");
        }


        return Ok("Email Sent Sucssefuly");
    }
    
    [HttpPost("SendResetPassword")]
    public async Task<IActionResult> SendResetPassword([FromBody]string Email)
    {
        var response = await _resetPasswordServices.SendResetPasswordAsync(Email ,"http://localhost:5140");
        if (!response)
        {
            return BadRequest("No reset password provided"); 
        }
        return Ok("Reset Password Sent Sucssefuly");
    }
    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody]  ResetPasswordDTO resetPassword)
    { 
      var response =  await _resetPasswordServices.ResetPasswordAsync(resetPassword.user_id, resetPassword.token, resetPassword.newPassword);
      if(!response)
          return BadRequest("Password Reset Failed");
      return Ok("Reset Password done successfully");
      
      
    }
    
}
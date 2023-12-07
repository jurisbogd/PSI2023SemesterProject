using DatabaseAPI.Models;
using DatabaseAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    [Route("CreateUser")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserModel model)
    {
        User newUser = _userService.CreateNewUser(model.username, model.email, model.password);
        await _userService.AddUserToTheDatabaseAsync(newUser);
        return Ok();
    }
    
    
    [HttpPost]
    [Route("UserLogIn")]
    public async Task<IActionResult> UserLogIn([FromBody] LoginRequestModel model)
    {
        var retrievedUser = await _userService.FindUserByEmailAsync(model.Email);

        if (retrievedUser != null && _userService.VerifyPassword(model.Password, retrievedUser.Salt, retrievedUser.PasswordHash))
        {
            return Ok(retrievedUser.UserID.ToString());
        }

        return BadRequest("Invalid email or password. Please try again.");
    }
}
public class CreateUserModel
{ 
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
}

public class LoginRequestModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}
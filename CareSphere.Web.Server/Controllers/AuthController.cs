using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CareSphere.Services.Users.Interfaces;
using CareSphere.Domains.Core;
using CareSphere.Web.Server.Requests;
using System.Data;
using Google.Apis.Auth;
using CareSphere.Web.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService; 

    public AuthController(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        try
        {
           var user= await Authenticate(login);
            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
        catch(ArgumentException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterReuest request)
    {
        try
        {
            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                Username = request.Email
            };
            var password = request.Password;
            var newUser = await _userService.CreateUser(user, password, true);
            return Ok(new { UserId=newUser.UserId });
        }
       
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    private async Task<User> Authenticate(LoginModel login)
    {
      return await _userService.GetUserByCredential(login.Username, login.Password);
    }

    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        try
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!result.Succeeded)
            return BadRequest();

        var claims = result.Principal.Identities
            .FirstOrDefault()?.Claims
            .Select(claim => new
            {
                claim.Type,
                claim.Value
            });
      var user=  await CreateUser(result);

        var token = GenerateJwtToken(user);
        return Redirect($"http://localhost:49907/auth/callback?token={token}");
    }
    [HttpPost("verify-google-token")]
    public async Task<IActionResult> VerifyGoogleToken([FromBody] GoogleTokenValidationRequest request)
    {
        if (string.IsNullOrEmpty(request.Credential))
        {
            return BadRequest(new { message = "Token is required." });
        }

        try
        {
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["Authentication:Google:ClientId"] } // Ensure it matches your Google Client ID
            };

            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(request.Credential, settings);

            // Get or create the user
            var user = await CreateOrGetUser(payload.Email, payload.Email, payload.GivenName, payload.FamilyName);
            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    Name = payload.Name,
                    Username = payload.Email
                };
                user = await _userService.CreateUser(user);
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return Ok(new
            {               
                Token = token
            });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = "Invalid token.", error = ex.Message });
        }
    }

    private async Task<User> CreateUser(AuthenticateResult result)
    {
        var username = result.Principal.Identity.Name;
        var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
        var firstName = result.Principal.FindFirst(ClaimTypes.GivenName)?.Value;
        var lastName = result.Principal.FindFirst(ClaimTypes.Surname)?.Value;    
       
            var newUser = new User
            {
                Username = username,
                Email = email,
                Name = $"{firstName} {lastName}",                
            };
            await _userService.CreateUser(newUser);
        return newUser;
    }
    private async Task<User> CreateOrGetUser(string userName, string email, string firstName, string lastName)
    {
        

        var newUser = new User
        {
            Username = userName,
            Email = email,
            Name = $"{firstName} {lastName}",
        };
        await _userService.CreateUser(newUser);
        return newUser;
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Name ?? string.Empty),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
        new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
    };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}
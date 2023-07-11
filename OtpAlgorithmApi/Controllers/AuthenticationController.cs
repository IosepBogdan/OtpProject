
using Microsoft.AspNetCore.Mvc;
using OtpAlgorithmServices.Interfaces;

namespace OtpAlgorithm.Controllers;

[Route("api/Authentication")]
public class AuthenticationController : Controller
{
    private IPasswordGenerator _passwordGenerator;
    private IPasswordValidator _passwordValidator;

    public AuthenticationController(IPasswordGenerator passwordGenerator, IPasswordValidator passwordValidator)
    {
        _passwordGenerator = passwordGenerator;
        _passwordValidator = passwordValidator;
    }
    
    [HttpGet]
    [Route("GetAuthenticationCode")]
    public IActionResult GetAuthenticationCode(int userId)
    {
        if (userId == 0)
        {
            return BadRequest();
        }
        
        var result = _passwordGenerator.GeneratePassword(userId, DateTime.UtcNow);
        
        return Ok(result);
    }

    [HttpGet]
    [Route("VerifyAuthenticationCode")]
    public IActionResult VerifyAuthenticationCode(int userId, string authenticationCode)
    {
        if (userId == 0 || string.IsNullOrEmpty(authenticationCode))
        {
            return BadRequest();
        }
        var result = _passwordValidator.ValidatePassword(userId, authenticationCode);

        return Ok(result);
    }
}

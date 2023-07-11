using Microsoft.Extensions.Caching.Memory;
using OtpAlgorithmCore.Models.Shared;
using OtpAlgorithmServices;
using OtpAlgorithmServices.Interfaces;

namespace OtpAlgorithmUnitTest;

[Parallelizable(ParallelScope.Self)]
public class Tests
{
    private IPasswordGenerator _passwordGenerator;
    private IPasswordValidator _passwordValidator;
    
    [SetUp]
    public void SetUp()
    {
        var cache = new MemoryCache(new MemoryCacheOptions());
        
        _passwordGenerator = new PasswordGenerator(cache);
        _passwordValidator = new PasswordValidator(cache);
    }
    
    [Test]
    public void GeneratePasswordTest()
    {
        var password = _passwordGenerator.GeneratePassword(5, DateTime.UtcNow);
        
        Assert.Multiple(() =>
        {
            Assert.That(password.Code, Is.Not.Empty);
            Assert.That(password.ResponseType, Is.EqualTo(ApiResponse.Success));
        });
    }

    [Test]
    public void ValidatePasswordWhenUserCreatesPassword()
    {
        var password = _passwordGenerator.GeneratePassword(6, DateTime.UtcNow);
        var result = _passwordValidator.ValidatePassword(6, password.Code);
        
        Assert.That(result.ResponseType, Is.EqualTo(ApiResponse.Success));
    }

    [Test]
    public void ValidatePasswordWhenUserCreatesMultiplePasswords()
    {
        var password1 = _passwordGenerator.GeneratePassword(6, DateTime.UtcNow);
        var password2 = _passwordGenerator.GeneratePassword(6, DateTime.UtcNow);
        
        var result1 = _passwordValidator.ValidatePassword(6, password1.Code);
        var result2 = _passwordValidator.ValidatePassword(6, password2.Code);
        
        Assert.Multiple(() =>
        {
            Assert.That(result1.ResponseType, Is.EqualTo(ApiResponse.Invalid));
            Assert.That(result2.ResponseType, Is.EqualTo(ApiResponse.Success));
        });
    }

    [Test]
    public void ValidatePasswordWhenUserCreatedNoPassword()
    {
        var result = _passwordValidator.ValidatePassword(9, string.Empty);
        
        Assert.That(result.ResponseType, Is.EqualTo(ApiResponse.NotFound));
    }

    [Test]
    [Parallelizable(ParallelScope.Self)]
    public async Task ValidatePasswordAfterExpiration()
    {
        var password = _passwordGenerator.GeneratePassword(15, DateTime.UtcNow);
        
        await Task.Delay(31000);
        
        var result1 = _passwordValidator.ValidatePassword(15, password.Code);
        
        Assert.That(result1.ResponseType, Is.EqualTo(ApiResponse.NotFound));
    }
}
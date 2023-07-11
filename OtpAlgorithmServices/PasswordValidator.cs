
using Microsoft.Extensions.Caching.Memory;
using OtpAlgorithmCore.Models.Shared;
using OtpAlgorithmServices.Interfaces;

namespace OtpAlgorithmServices;

public class PasswordValidator : IPasswordValidator
{
    private IMemoryCache _memoryCache;

    public PasswordValidator(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public Response ValidatePassword(int userId, string valid)
    {
        var response = new Response();
        
        if (!_memoryCache.TryGetValue(userId, out string hashedValue))
        {
            response.ResponseType = ApiResponse.NotFound;
            response.Message = "No code found, please request a code again.";

            return response;
        }

        var equal = hashedValue.Equals(valid);
        
        if (equal)
        {
            response.ResponseType = ApiResponse.Success;
            response.Message = "Key found.";
        }
        else
        {
            response.ResponseType = ApiResponse.Invalid;
            response.Message = "Keys don't match.";
        }
        
        return response;
    }
}
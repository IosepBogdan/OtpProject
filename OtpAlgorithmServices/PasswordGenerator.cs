using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using OtpAlgorithmCore.Models;
using OtpAlgorithmCore.Models.Shared;
using OtpAlgorithmServices.Interfaces;

namespace OtpAlgorithmServices;

public class PasswordGenerator : IPasswordGenerator
{
    private IMemoryCache _memoryCache;

    public PasswordGenerator(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public GetAuthenticationCodeResponse GeneratePassword(int userId, DateTime dateTime)
    {
        var response = new GetAuthenticationCodeResponse
        {
            ResponseType = ApiResponse.Success
        };

        try
        {
            int saltSize = 128 / 8; 
            int PBKDF2SubkeyLength = 256 / 8; 
            byte[] salt;
            byte[] subkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(userId.ToString() + dateTime, saltSize, 1000))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }

            byte[] outputBytes = new byte[1 + saltSize + PBKDF2SubkeyLength];
            Buffer.BlockCopy(salt, 0, outputBytes, 1, saltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + saltSize, PBKDF2SubkeyLength);

            if (_memoryCache.TryGetValue(userId, out _))
            {
                _memoryCache.Remove(userId);
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
            var base64OutputBytes = Convert.ToBase64String(outputBytes);
            _memoryCache.Set(userId, base64OutputBytes, cacheEntryOptions);
            
            response.Code = base64OutputBytes;
            response.Message = "Code successfully generated";
            
            return response;
        }
        catch (Exception e)
        {
            response.ResponseType = ApiResponse.Failure;
            response.Message = $"Something went wrong: {e.Message}";
            
            return response;
        }
    }
}
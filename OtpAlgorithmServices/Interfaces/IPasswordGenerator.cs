using OtpAlgorithmCore.Models;

namespace OtpAlgorithmServices.Interfaces;

public interface IPasswordGenerator
{
    GetAuthenticationCodeResponse GeneratePassword(int userId, DateTime dateTime);
}
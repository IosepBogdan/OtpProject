using OtpAlgorithmCore.Models.Shared;

namespace OtpAlgorithmServices.Interfaces;

public interface IPasswordValidator
{
    Response ValidatePassword(int userId, string valid);
}
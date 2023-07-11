using OtpAlgorithmCore.Models.Shared;

namespace OtpAlgorithmCore.Models;

public class GetAuthenticationCodeResponse : Response
{
    public string Code { get; set; }
}
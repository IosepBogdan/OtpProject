using System.Text.Json.Serialization;

namespace OtpAlgorithmCore.Models.Shared;

public enum ApiResponse
{
    Success = 1,
    Failure = 2,
    BadRequest = 3,
    Invalid = 4,
    NotFound = 5
}
    
public class Response
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("responseType")]
    public ApiResponse ResponseType { get; set; } = ApiResponse.Success;    
}
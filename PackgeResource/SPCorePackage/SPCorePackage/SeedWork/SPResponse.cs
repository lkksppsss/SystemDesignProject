using System.Text.Json.Serialization;

namespace SPCorePackage.SeedWork;

public class SPResponse<T>
{
    [JsonPropertyName("code")]
    public string Code { get; private set; }

    [JsonPropertyName("message")]
    public string Message { get; private set; }

    [JsonPropertyName("data")]
    public T Data { get; private set; }

    public SPResponse()
    {
    }

    public SPResponse(T data)
    {
        Code = ResponseException.SUCCESS_CODE;
        Message = ResponseException.SUCCESS_MESSAGE;
        Data = data;
    }

    public void SetSuccess()
    {
        Code = ResponseException.SUCCESS_CODE;
        Message = ResponseException.SUCCESS_MESSAGE;
    }

    public void SetSuccess(T data)
    {
        Code = ResponseException.SUCCESS_CODE;
        Message = ResponseException.SUCCESS_MESSAGE;
        Data = data;
    }

    public void SetFailure(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public void SetUnCatchException()
    {
        Code = ResponseException.DEFAULT_ERROR_CODE;
        Message = ResponseException.DEFAULT_ERROR_MESSAGE;
    }
}

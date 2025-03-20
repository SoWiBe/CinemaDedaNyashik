using System.Runtime.Serialization;

namespace Back.Api.Infrastructure.Exceptions;

[Serializable]
public class CustomException : Exception
{
    public int StatusCode { get; } = 500;
    public string ErrorCode { get; }
    public string? Details { get; }
    
    public CustomException(string message) 
        : base(message)
    {
    }
    
    public CustomException(string message, int statusCode, string errorCode, string? details = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        Details = details;
    }
    
    public CustomException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    
    public CustomException(string message, int statusCode, string errorCode, string? details, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        Details = details;
    }
    
    protected CustomException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        StatusCode = info.GetInt32(nameof(StatusCode));
        ErrorCode = info.GetString(nameof(ErrorCode));
        Details = info.GetString(nameof(Details));
    }
    
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(StatusCode), StatusCode);
        info.AddValue(nameof(ErrorCode), ErrorCode);
        info.AddValue(nameof(Details), Details);
    }
}
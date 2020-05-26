using System;
using System.Collections.Generic;

namespace Assets.Exceptions
{
    public class ApiException : Exception
    {
        public ApiErrorCode ErrorCode { get; set; }
        public Dictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();

        public ApiException(ApiErrorCode code, string message) : base(message)
        {
            ErrorCode = code;
        }

        public static ApiException Create(ApiErrorCode code, string message)
        {
            return new ApiException(code, message);
        }
    }

    public static class ApiExceptionExtensions
    {
        public static ApiException AddField(this ApiException exception, string fieldName, string message)
        {
            exception.Fields.Add(fieldName, message);
            return exception;
        }
    }
}

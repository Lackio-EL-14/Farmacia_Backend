using System;
using System.Net;

namespace Farmacia.Core.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public object? Errors { get; }

        public ApiException(string message, int statusCode = (int)HttpStatusCode.BadRequest, object? errors = null)
            : base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}
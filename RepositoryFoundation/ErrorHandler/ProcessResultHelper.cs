using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HelperFoundation.ErrorHandler
{
    public static class ProcessResultHelper
    {
        public static ProcessResult<T> GetResult<T>(this T result)
        {
            return new ProcessResult<T> { Result = result };
        }

        public static ProcessResult<T> GetResult<T>(this T result, string message)
        {
            return new ProcessResult<T> { Result = result, Message = message };
        }

        public static ProcessResult<T> GetResult<T>(this T result, string message, HttpStatusCode statusCode)
        {
            return new ProcessResult<T> { Result = result, Message = message, StatusCode = statusCode };
        }

        public static ProcessResult<T> GetNegativeResult<T>(string errorMessage, HttpStatusCode statusCode)
        {
            return new ProcessResult<T> { Errors = new List<string> { errorMessage }, StatusCode = statusCode };
        }

        public static ProcessResult<T> GetNegativeResult<T>(List<string> errorMessages, HttpStatusCode statusCode)
        {
            return new ProcessResult<T> { Errors = errorMessages, StatusCode = statusCode };
        }

        public static ProcessResult GetNegativeResult(string errorMessage, HttpStatusCode statusCode)
        {
            return new ProcessResult { Errors = new List<string> { errorMessage }, StatusCode = statusCode };
        }

        public static ProcessResult GetNegativeResult(List<string> errorMessages, HttpStatusCode statusCode)
        {
            return new ProcessResult { Errors = errorMessages, StatusCode = statusCode };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryFoundation.Helper.ErrorHandler
{
    public class ServerValidationException : Exception
    {
        public ErrorType ErrorType { get; set; }
        public ResponseStatusCode StatusCode { get; set; }

        public ServerValidationException(string message) : this(message, ErrorType.Validation)
        {
        }

        public ServerValidationException(string message, ResponseStatusCode statusCode) : this(message, ErrorType.Validation, statusCode)
        {
        }

        public ServerValidationException(string message, ErrorType errorType) : this(message, errorType, GetStatusCode(errorType))
        {
        }

        public ServerValidationException(string message, ErrorType errorType, ResponseStatusCode statusCode) : base(message)
        {
            ErrorType = errorType;
            StatusCode = statusCode;
        }

        private static ResponseStatusCode GetStatusCode(ErrorType errorType)
        {
            switch (errorType)
            {
                case ErrorType.Validation:
                    return ResponseStatusCode.PreconditionFailed;
                case ErrorType.Fatal:
                    return ResponseStatusCode.BadRequest;
                case ErrorType.Unknown:
                default:
                    return ResponseStatusCode.BadRequest;
            }
        }
    }
}

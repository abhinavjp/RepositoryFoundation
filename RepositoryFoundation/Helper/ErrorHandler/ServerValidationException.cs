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
        public ErrorStatusCode StatusCode { get; set; }

        public ServerValidationException(string message) : this(message, ErrorType.Validation)
        {
        }

        public ServerValidationException(string message, ErrorStatusCode statusCode) : this(message, ErrorType.Validation, statusCode)
        {
        }

        public ServerValidationException(string message, ErrorType errorType) : this(message, errorType, ErrorStatusCode.NoStatus)
        {
        }

        public ServerValidationException(string message, ErrorType errorType, ErrorStatusCode statusCode) : base(message)
        {
            ErrorType = errorType;
            StatusCode = statusCode;
        }
    }
}

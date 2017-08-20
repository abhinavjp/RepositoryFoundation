using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryFoundation.Helper.ErrorHandler
{
    public class ErrorResult
    {
        public ErrorResult(string errorMessage, ErrorType errorType)
            : this(new List<string> { errorMessage}, null, string.Empty, errorType)
        {

        }
        public ErrorResult(List<string> errorMessages, List<string> stackTrace, string targetMethod, ErrorType errorType)
        {
            ErrorMessage = errorMessages;
            StackTrace = stackTrace;
            TargetMethod = targetMethod;
            ErrorType = errorType;
        }
        public List<string> ErrorMessage { get; set; }
        public List<string> StackTrace { get; set; }
        public string TargetMethod { get; set; }
        public ErrorType ErrorType { get; set; }
        internal List<ErrorResult> ErrorResultList => new List<ErrorResult> { this };
    }
}

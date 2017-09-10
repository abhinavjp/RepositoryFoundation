using RepositoryFoundation.Helper.ErrorHandler;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HelperFoundation.ErrorHandler
{
    public static class ProcessResultHelper
    {
        private const ResponseStatusCode DefaultValidationStatusCode = ResponseStatusCode.PreconditionFailed;
        private const ResponseStatusCode FatalValidationStatusCode = ResponseStatusCode.BadRequest;
        private const ErrorType DefaultValidationErrorType = ErrorType.Validation;
        private const ErrorType FatalValidationErrorType = ErrorType.Fatal;
        #region Postive Results
        public static ProcessResult<T> GetResult<T>(this T result)
        {
            return new ProcessResult<T> { Result = result };
        }

        public static ProcessResult<T> GetResult<T>(this T result, string message)
        {
            return new ProcessResult<T> { Result = result, SuccessMessage = message };
        }

        public static ProcessResult<T> GetResult<T>(this T result, string message, ResponseStatusCode statusCode)
        {
            return new ProcessResult<T> { Result = result, SuccessMessage = message, StatusCode = statusCode };
        }
        #endregion


        #region Negative Results
        public static ProcessResult<T> GetNegativeResult<T>(string errorMessage)
        {
            return GetNegativeResult<T>(DefaultValidationStatusCode, DefaultValidationErrorType, errorMessage);
        }

        public static ProcessResult<T> GetNegativeResult<T>(ErrorType errorType, string errorMessage)
        {
            return GetNegativeResult<T>(DefaultValidationStatusCode, errorType, errorMessage);
        }

        public static ProcessResult<T> GetNegativeResult<T>(ResponseStatusCode statusCode, string errorMessage)
        {
            return GetNegativeResult<T>(statusCode, DefaultValidationErrorType, errorMessage);
        }

        public static ProcessResult<T> GetNegativeResult<T>(ResponseStatusCode statusCode, ErrorType errorType, string errorMessage)
        {
            return GetNegativeResult<T>(new ErrorResult(errorMessage, errorType).ErrorResultList, statusCode);
        }

        public static ProcessResult<T> GetNegativeResult<T>(List<ErrorResult> errorList, ResponseStatusCode statusCode)
        {
            return new ProcessResult<T> { Errors = errorList, StatusCode = statusCode };
        }

        public static ProcessResult GetNegativeResult(string errorMessage)
        {
            return GetNegativeResult(DefaultValidationStatusCode, DefaultValidationErrorType, errorMessage);
        }

        public static ProcessResult GetNegativeResult(ErrorType errorType, string errorMessage)
        {
            return GetNegativeResult(DefaultValidationStatusCode, errorType, errorMessage);
        }

        public static ProcessResult GetNegativeResult(ResponseStatusCode statusCode, string errorMessage)
        {
            return GetNegativeResult(statusCode, DefaultValidationErrorType, errorMessage);
        }

        public static ProcessResult GetNegativeResult(ResponseStatusCode statusCode, ErrorType errorType, string errorMessage)
        {
            return GetNegativeResult(new ErrorResult(errorMessage, errorType).ErrorResultList, statusCode);
        }

        public static ProcessResult GetNegativeResult(List<ErrorResult> errorList, ResponseStatusCode statusCode)
        {
            return new ProcessResult { Errors = errorList, StatusCode = statusCode };
        }
        #endregion

        #region Exception Results
        public static ProcessResult GetExceptionResult(this Exception ex)
        {
            var errors = new List<string>();
            if (ex is DbEntityValidationException)
                errors.AddRange(ex.ExtractEntityValidationErrors());
            errors.AddRange(ex.ExtractErrors());
            var errorType = ex is ServerValidationException ? (ex as ServerValidationException).ErrorType : FatalValidationErrorType;
            var error = new ErrorResult(errors, ex.ExtractStackTrace(), ex.TargetSite.Name, FatalValidationErrorType);
            return new ProcessResult { Errors = new List<ErrorResult> { error }, StatusCode = FatalValidationStatusCode };
        }

        public static ProcessResult<T> GetExceptionResult<T>(this Exception ex)
        {
            var errors = new List<string>();
            if (ex is DbEntityValidationException)
                errors.AddRange(ex.ExtractEntityValidationErrors());
            errors.AddRange(ex.ExtractErrors());
            var error = new ErrorResult(errors, ex.ExtractStackTrace(), ex.TargetSite.Name, FatalValidationErrorType);
            return new ProcessResult<T> { Errors = new List<ErrorResult> { error }, StatusCode = FatalValidationStatusCode };
        }
        #endregion

        public static List<string> ExtractErrors<TException>(this TException ex) where TException : Exception
        {
            return ExtractInfo(ex, (exc) => exc.Message);
        }

        public static List<string> ExtractStackTrace<TException>(this TException ex) where TException : Exception
        {
            return ExtractInfo(ex, (exc) => exc.StackTrace);
        }

        public static List<string> ExtractEntityValidationErrors(this Exception ex)
        {
            if (ex is DbEntityValidationException)
                return ExtractInfo(ex as DbEntityValidationException, (exc) => string.Join("\n", exc.EntityValidationErrors.SelectMany(s => s.ValidationErrors.Select(ss => $"{ss.PropertyName}: {ss.ErrorMessage}"))));
            return new List<string>();
        }

        public static List<string> ExtractInfo<TException>(this TException ex, Func<TException, string> propertyGetter) where TException : Exception
        {
            var errors = new List<string>();
            errors.AddRange(propertyGetter?.Invoke(ex).Split('\n'));

            while (ex.InnerException != null)
            {
                if (ex.InnerException is TException)
                    errors.AddRange(propertyGetter?.Invoke(ex.InnerException as TException).Split('\n'));
            }

            return errors;
        }
    }
}

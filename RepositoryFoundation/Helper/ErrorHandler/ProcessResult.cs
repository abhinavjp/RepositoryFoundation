using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using HelperFoundation.ErrorHandler;
using RepositoryFoundation.Helper.ErrorHandler;

namespace HelperFoundation.ErrorHandler
{
    public class ProcessResult<T> : ProcessResult
    {
        public T Result { get; set; }
    }
    public class ProcessResult
    {
        internal ProcessResult()
        {

        }
        public int ResultId { get; set; }
        public ResponseStatusCode StatusCode { get; set; }
        public string SuccessMessage { get; set; }
        public List<ErrorResult> Errors { get; set; }
        public static ProcessResult SuccessResult => new ProcessResult();
        public static ProcessResult GetSuccessResult(string message)
        {
            return GetSuccessResult(default(int), message);
        }
        public static ProcessResult GetSuccessResult(int id)
        {
            return GetSuccessResult(id, null);
        }
        public static ProcessResult GetSuccessResult(int id, string message)
        {
            return new ProcessResult { ResultId = id, SuccessMessage = message };
        }
        public bool Success
        {
            get
            {
                return Errors == null || Errors.Any();
            }
        }
        public Dictionary<string, object> ExternalData { get; set; }
    }
}

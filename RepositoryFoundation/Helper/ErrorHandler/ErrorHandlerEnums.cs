using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryFoundation.Helper.ErrorHandler
{
    public enum ErrorType
    {
        Validation,
        Fatal,
        Unknown
    }

    public enum ResponseStatusCode
    {
        NoStatus = 0,
        OK = 200,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        Conflict = 409,
        Gone = 410,
        PreconditionFailed = 412,
        UnsupportedMediaType = 415,
        RequestedRangeNotSatisfiable = 416,
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        HttpVersionNotSupported = 505
    }
}

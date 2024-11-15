using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Handler
{
    public class Constant
    {
        public struct HandleResponse
        {
            // Success
            public const string Success = "Successfully Submitted";
            public const string SuccessCode = "200";
            public const string ErrorCode = "400";
            public const string SuccessImport = "Thank you for confirming that your data has been imported successfully. We will synchronize it to the Sitecoresend system at midnight.";
            public const string Delete = "Successfully Deleted";

            //// Client Errors
            public const int BadRequest = 400;
            public const string BadRequestMessage = "Bad Request! Something went wrong, please try again later.";
            public const string InsertionFailed = "Bad Request: Invalid data provided, insertion failed.";
            public const string UpdateFailed = "Bad Request: Invalid data provided, updation failed.";
            public const string DeleteFailed = "Bad Request: Invalid data provided, Deletion failed.";

            public const int Unauthorized = 401;
            public const string UnauthorizedMessage = "Unauthorized. Please be authenticate first.";

            public const int AccessDenied = 403;
            public const string AccessDeniedMessage = "Access Denied: You are not allowed to access this.";

            public const int NotFound = 404;
            public const string NotFoundMessage = "Content Not Found! Please try again.";

            public const int MethodNotAllowed = 405;
            public const string MethodNotAllowedMessage = "Method Not Allowed: The method specified in the request is not allowed. Please try again.";

            public const int TooManyRequests = 429;
            public const string TooManyRequestsMessage = "Too Many Requests from client.";

            // Server Errors
            public const int InternalServerError = 500;
            public const string InternalServerErrorMessage = "Something went wrong, please try again later.";

            // others
            public const string Update = "Successfully Updated.";

            public const string MissingSecurityCode = "Please enter valid security token.";

            public const string EmptyField = "Required field cannot be empty.";

            public const string DuplicateList = "This name already has been inserted, Please try another.";

            public const string EmptyData = "No data is available.";

            public const string ListNameInvalid = "Please enter correct list name.";

            public const string SecurityCodeMessage = "The provided token is valid for the next 1 hour.";

            public const string InvalidToken = "Token is invalid. Please ensure the correct token or request a new one. ";



        }
    }
}

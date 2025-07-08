using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GitHubActionDemo.Extensions
{
    public static class ResultsExtensions
    {
        //public static IResult HandleFailure(Result result)
        //{
        //    return result switch
        //    {
        //        { IsSuccess: true } => throw new InvalidOperationException(),
        //        //var validationResult => Results.BadRequest(
        //        //        CreateProblemDetails(
        //        //            "Validation Error",
        //        //            StatusCodes.Status400BadRequest,
        //        //            result.Error,
        //        //            validationResult.Error)),
        //        //_ => Results.BadRequest(CreateProblemDetails(
        //        //    "Bad Request",
        //        //    StatusCodes.Status400BadRequest,
        //        //    result.Error))
        //    };
        //}

        //public static ProblemDetails CreateProblemDetails(
        //    string title,
        //    int status,
        //    Error error,
        //    Error[]? errors = null)
        //{
        //    return new ProblemDetails()
        //    {
        //        Title = title,
        //        Type = error.Code,
        //        Detail = error.Description,
        //        Status = status,
        //        Extensions = { { nameof(errors), errors } }
        //    };
        //}

        public static IResult ToProblemDetails<T>(this Result<T> results)
        {
            return Results.Problem(
                statusCode: GetStatusCode(results.Error?.ErrorType),
                title: results.Error?.Code,
                extensions: new Dictionary<string, object?>
                {
                    { "errors",results.Error}
                });
        }

        static int GetStatusCode(ErrorType? errorType) =>
            errorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.BadRequest => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError,
            };
    }

}


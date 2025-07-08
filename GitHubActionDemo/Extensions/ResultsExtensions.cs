using Domain.Shared;

namespace GitHubActionDemo.Extensions
{
    public static class ResultsExtensions
    {
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


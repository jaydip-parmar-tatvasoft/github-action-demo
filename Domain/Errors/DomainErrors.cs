using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{

    public static class User
    {
        public static Error UserNotFound(string identifier) => Error.NotFound(
            "UserNotFound",
            $"The user with the identifier {identifier} was not found.");

        public static Error InvalidCredentials => Error.AccessUnAuthorized(
          "User.InvalidCredentials",
          $"The credentials provided could not be authenticated.");
    }
}
﻿namespace Domain.Shared
{
    public enum ErrorType
    {
        Failure = 0,
        NotFound = 1,
        Validation = 2,
        Conflict = 3,
        AccessUnAuthorized = 4,
        AccessForbidden = 5,
        BadRequest = 6,
        ServerError = 7,
        InvalidCredential = 8,
    }
}

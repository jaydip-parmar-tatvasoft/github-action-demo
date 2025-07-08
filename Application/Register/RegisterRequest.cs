namespace Application.Register
{
    public sealed record RegisterRequest(
          string Email,
          string UserName,
          string Password
          );
}

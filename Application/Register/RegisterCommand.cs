using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.Register
{
    public sealed record RegisterCommand(
       string Email,
       string UserName, 
       string Password
       ) : ICommand<Guid>;
}

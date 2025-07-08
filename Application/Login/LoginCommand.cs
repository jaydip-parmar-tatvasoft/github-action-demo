using Application.Abstractions.Messaging;

namespace Application.Login;
public record LoginCommand(string Email,string Password) : ICommand<LoginResponse>;

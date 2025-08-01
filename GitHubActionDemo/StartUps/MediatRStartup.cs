﻿using MediatR;

namespace GitHubActionDemo.StartUps;

public static class MediatorStartup
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(option =>
        {
            option.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);
            option.RegisterServicesFromAssembly(GitHubActionDemo.AssemblyReference.Assembly);
        });
        //services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}

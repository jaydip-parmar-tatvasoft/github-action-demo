using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Startup
{
    /// <summary>
    /// Initializes the Data Access Layer using Entity Framework
    /// </summary>
    public static class DataAccessStartup
    {
        public static IServiceCollection AddDataAccessEntityFramework(this IServiceCollection services,
            IConfiguration configuration)
        {
            //TODO:  Is there a way to do this with IOptions???
            DataAccessDatabaseStartup.Implement(services);
            DataAccessRepositoryStartup.Register(services);
            return services;
        }
    }
}

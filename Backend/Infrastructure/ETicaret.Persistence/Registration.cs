using ETicaret.ApplicationAndDomain.Interfaces;
using ETicaret.Persistence.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Persistence
{
    public static class Registration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbConnection, ApplicationDbConnection>();

            return services;
        }
    }
}

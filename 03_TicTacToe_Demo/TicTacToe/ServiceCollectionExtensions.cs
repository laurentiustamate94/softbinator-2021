using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace TicTacToe
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTicTacToe(this IServiceCollection services)
        {
            services.AddSingleton<AppState>();
            services.AddFeatureManagement();

            return services;
        }
    }
}

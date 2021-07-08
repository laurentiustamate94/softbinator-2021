using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace TicTacToe
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTicTacToe(this IServiceCollection services)
        {
            return services;
        }
    }
}

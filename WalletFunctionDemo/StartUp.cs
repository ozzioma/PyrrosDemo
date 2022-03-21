using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using WalletDomain;

[assembly: FunctionsStartup(typeof(WalletFunctionDemo.Startup))]

namespace WalletFunctionDemo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");


            builder.Services.AddDbContext<WalletDbContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));


        }
    }
}


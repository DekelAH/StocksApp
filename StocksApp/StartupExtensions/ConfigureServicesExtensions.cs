using Entities.DbContextModels;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts.Contracts;
using Services;
using StocksApp.OptionsModels;
using StocksApp.ServiceContracts;
using StocksApp.Services;

namespace StocksApp
{
    public static class ConfigureServicesExtensions
    {
        #region Methods

        public static IServiceCollection ConfigureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddControllersWithViews();
            IServiceCollection serviceCollection = services.Configure<TradingOptions>(configuration.GetSection("TradingOptions"));
            services.AddHttpClient();
            services.AddScoped<IFinnhubService, FinnhubService>();
            services.AddScoped<IStocksService, StocksService>();
            services.AddScoped<IFinnhubRepository, FinnhubRepository>();
            services.AddScoped<IStocksRepository, StocksRepository>();
            services.AddDbContext<StockMarketDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties |
                                        Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            return services;
        }

        #endregion
    }
}

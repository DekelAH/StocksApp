using Entities.DbContextModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTesting
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        #region Methods

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                var descripter = services.SingleOrDefault(temp => 
                temp.ServiceType == typeof(DbContextOptions<StockMarketDbContext>));

                if (descripter != null)
                {
                    services.Remove(descripter);
                }

                services.AddDbContext<StockMarketDbContext>(temp => temp.UseInMemoryDatabase("DataBaseTest"));
            });
        }

        #endregion
    }
}

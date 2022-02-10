using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Indra.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using TradingApp.Application.Services.Helpers;
using TradingApp.Infrastructure.Persistence;
using TradingApp.Web.Api.Extensions;

namespace TradingApp.Web.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly string _policyName = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }
        public async void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.ConfigureAutoMapper();
            services.ConfigureApplicationServices();
            services.ConfigureRepositories();
            services.AddHttpClient();

            //healthcheck
            services.AddHealthChecks().AddMySql(Configuration.GetConnectionString("TradingAppDataContextString"));

            //Graphical Health
            //
            services.AddHealthChecksUI(options =>
            {
                var conf = Configuration.GetConnectionString("HealthCheckUri");
                options.SetEvaluationTimeInSeconds(5); //Sets the time interval in which HealthCheck will be triggered
                options.MaximumHistoryEntriesPerEndpoint(10); //Sets the maximum number of records displayed in history
                options.AddHealthCheckEndpoint("Health Checks API", conf); //Sets the Health Check endpoint
            }).AddInMemoryStorage(); //Here is the memory bank configuration
           
            //FluentValidation
            services.AddFluentValidation(
                s => 
                {
                    s.RegisterValidatorsFromAssemblyContaining<Startup>();
                }
            );

            //CORS Enabled 
            services.AddCors(opt =>
            {
                opt.AddPolicy(name: _policyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TradingApp.Web.Api", Version = "v1" });
            });

            //await StockDbSeeder.FetchStocks(services.BuildServiceProvider(), 1000);
        }

        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider )
        {

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TradingAppDbContext>();

                context.Database.Migrate();
            }

            StockDbSeeder.FetchStocks(serviceProvider).Wait();


            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TradingApp.Web.Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();
          
            app.UseCors(_policyName);

            app.UseAuthorization();

            app.UseMiddleware<AntiXssMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //HealthGraphicalEndPoint
                endpoints.MapHealthChecks("/health");
            });

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = p => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options => { options.UIPath = "/dashboard"; });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScalableVehicleService;
using ScalableVehicleService.DAL;
using ScalableVehicleService.Model;
using ScalableVehicleService.PersistentQueue;
using ScalableVehicleService.Services.Payload;
using ScalableVehicleService.Services;
using VehicleService.Services;

namespace VehicleManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IAcknowledgePayloadConsumer<VehiclePayload>, VehiclePayloadConsumer>();
            services.AddSingleton<IPersistentConcurrentQueue<VehiclePayload>, PersistentConcurrentQueue<VehiclePayload>>();
            services.AddScoped<IScalableVehicleService, ScalableVehicleService.Services.ScalableVehicleService>();
            services.AddScoped<IVehicleService, ScalableVehicleService.VehicleService>();
            services.AddDbContext<VehicleDbContext>(ServiceLifetime.Singleton);
            services.AddSingleton<IGenericRepository<Vehicle>, GenericRepository<Vehicle>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

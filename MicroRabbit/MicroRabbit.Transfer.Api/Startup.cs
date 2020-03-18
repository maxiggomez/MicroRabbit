using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MicroRabbit.Banking.Domain.Events;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.IoC;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.EventHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MicroRabbit.Transfer.Api
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
            services.AddDbContext<TransferDbContext>(options => {
                //options.UseSqlServer(Configuration.GetConnectionString("BankingDbConnection"));
                options.UseSqlServer("Server=localhost;Database=TransferDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Banking Microservices", Version = "v1" });
            });


            services.AddMediatR(typeof(Startup));

            // Para configurar Inyección de dependencia
            RegisterServices(services);

        }

        private void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transfer Microservice v1");
            });

            app.UseMvc();

            ConfigureEventBus(app);
        }

        // Acá configuro los eventos a los que me quiero subscribir
        private  void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            // Me subscribo al tipo de evento TransfereCreatedEvent
            eventBus.Subscribe<MicroRabbit.Transfer.Domain.Events.TransfereCreatedEvent, TransferEventHandler>();

        }
    }
}
